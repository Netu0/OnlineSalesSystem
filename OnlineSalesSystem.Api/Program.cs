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


var builder = WebApplication.CreateBuilder(args);

// Configuração do banco de dados
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
if (string.IsNullOrEmpty(connectionString))
{
    throw new Exception("Connection string 'DefaultConnection' not found.");
}

builder.Services.AddDbContext<ApplicationDbContext>(options => 
    options.UseNpgsql(connectionString));

// Configuração do JWT
var jwtSecret = builder.Configuration["Jwt:Key"];
if (string.IsNullOrEmpty(jwtSecret))
{
    throw new Exception("JWT Secret is not configured.");
}

var key = Encoding.UTF8.GetBytes(jwtSecret);

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.RequireHttpsMetadata = false;
        options.SaveToken = true;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(key),
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            ValidateLifetime = true,
            ClockSkew = TimeSpan.Zero
        };
    });

// Configurações da aplicação
builder.Services.AddAuthorization();
builder.Services.AddApplicationServices();
builder.Services.AddAutoMapper(typeof(OrderMappingProfile));

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<AuthService>();
builder.Services.AddScoped<IUserRepository, UserRepository>();

var app = builder.Build();

// Pipeline de requisições HTTP
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

// Aplicar migrações automaticamente
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var logger = services.GetRequiredService<ILogger<Program>>();
    
    try
    {
        var context = services.GetRequiredService<ApplicationDbContext>();
        logger.LogInformation("Aguardando PostgreSQL iniciar...");
        await Task.Delay(10000); // Espera 10 segundos
        await context.Database.MigrateAsync();
        logger.LogInformation("Migração concluída");
    }
    catch (Exception ex)
    {
        logger.LogError(ex, "Erro durante migração");
    }
}

app.Run();