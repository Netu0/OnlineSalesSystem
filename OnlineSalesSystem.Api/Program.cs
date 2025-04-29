using Microsoft.EntityFrameworkCore;
using OnlineSalesSystem.Infrastructure;
using OnlineSalesSystem.Infrastructure.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add infrastructure layer
builder.Services.AddInfrastructure(builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

// Apply migrations automatically
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