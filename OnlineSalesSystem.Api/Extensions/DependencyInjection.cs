using OnlineSalesSystem.Core.Interfaces;
using OnlineSalesSystem.Infrastructure.Repositories;
using OnlineSalesSystem.Core.Services;

namespace OnlineSalesSystem.Api.Extensions;

public static class DependencyInjection
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        // Registro dos repositórios
        services.AddScoped<ICustomerRepository, CustomerRepository>();
        services.AddScoped<IOrderRepository, OrderRepository>();

        // Registro dos serviços
        services.AddScoped<CustomerService>();
        services.AddScoped<OrderService>();

        services.AddScoped<AuthService>();
        services.AddScoped<IUserRepository, UserRepository>();

        return services;
    }
}