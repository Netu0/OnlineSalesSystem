using Microsoft.EntityFrameworkCore;
using OnlineSalesSystem.Infrastructure.Data;
using OnlineSalesSystem.Core.Interfaces;
using OnlineSalesSystem.Infrastructure.Repositories;
using OnlineSalesSystem.Core.Services;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using OnlineSalesSystem.Api.Extensions;
using OnlineSalesSystem.Api.Middlewares;
using AutoMapper;
using OnlineSalesSystem.Api.MappingProfile;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// ========== CONFIGURAÇÃO JWT ==========
var jwtConfig = builder.Configuration.GetSection("Jwt");

// Debug: Mostrar configurações carregadas
Console.WriteLine("=== CONFIGURAÇÕES JWT ===");
Console.WriteLine($"Key: {jwtConfig["Key"]}");
Console.WriteLine($"Issuer: {jwtConfig["Issuer"]}");
Console.WriteLine($"Audience: {jwtConfig["Audience"]}");
Console.WriteLine($"ExpireHours: {jwtConfig["ExpireHours"]}");
Console.WriteLine("==========================");

if (string.IsNullOrEmpty(jwtConfig["Key"]))
{
    throw new InvalidOperationException(
        "Chave JWT não configurada. Verifique:\n" +
        "1. Se a seção JWT existe em appsettings.json\n" +
        "2. Se o arquivo appsettings.json está na pasta correta\n" +
        "3. Se a propriedade 'Copy to Output Directory' está como 'Copy always'");
}

var jwtKey = jwtConfig["Key"]!;
if (jwtKey.Length < 32)
{
    throw new InvalidOperationException("A chave JWT deve ter no mínimo 32 caracteres!");
}

var key = Encoding.UTF8.GetBytes(jwtKey);
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(key),
            ValidateIssuer = true,
            ValidIssuer = jwtConfig["Issuer"],
            ValidateAudience = true,
            ValidAudience = jwtConfig["Audience"],
            ValidateLifetime = true,
            ClockSkew = TimeSpan.Zero
        };
    });

// ========== CONFIGURAÇÃO DO BANCO DE DADOS ==========
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
if (string.IsNullOrEmpty(connectionString))
{
    throw new Exception("Connection string 'DefaultConnection' not found.");
}

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(connectionString));

// ========== CONFIGURAÇÕES DA APLICAÇÃO ==========
builder.Services.AddAuthorization();
builder.Services.AddApplicationServices();
builder.Services.AddAutoMapper(typeof(OrderMappingProfile));

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

// Configuração do Swagger com suporte a JWT
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "OnlineSalesSystem API", Version = "v1" });
    
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });
    
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

// Registro de serviços
builder.Services.AddScoped<AuthService>();
builder.Services.AddScoped<IUserRepository, UserRepository>();

var app = builder.Build();

// ========== CONFIGURAÇÃO DO PIPELINE ==========
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c => 
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "OnlineSalesSystem API v1");
        c.DisplayRequestDuration();
    });
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

//app.UseMiddleware<ExceptionMiddleware>();
app.MapControllers();

// ========== APLICAÇÃO DE MIGRAÇÕES ==========
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var logger = services.GetRequiredService<ILogger<Program>>();

    try
    {
        var context = services.GetRequiredService<ApplicationDbContext>();
        logger.LogInformation("Aguardando PostgreSQL iniciar...");
        
        // Implementação com retry pattern
        var maxRetryAttempts = 5;
        var delayBetweenRetries = TimeSpan.FromSeconds(10);

        for (int i = 0; i < maxRetryAttempts; i++)
        {
            try
            {
                await context.Database.MigrateAsync();
                logger.LogInformation("Migração concluída com sucesso");
                break;
            }
            catch (Exception ex) when (i < maxRetryAttempts - 1)
            {
                logger.LogWarning($"Falha na tentativa {i + 1} de aplicar migrações: {ex.Message}");
                await Task.Delay(delayBetweenRetries);
            }
        }
    }
    catch (Exception ex)
    {
        logger.LogError(ex, "Erro durante a aplicação de migrações");
        throw;
    }
}

app.Run();