using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Test.Application.Services;
using Test.Domain.Interfaces;
using Test.Domain.Interfaces.Repositories;
using Test.Infrastructure.Persistence.Repositories;
using Test.Infrastructure.Persistence;

namespace Test.Infrastructure;

public static class Bootstrap
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IEmployeeRepository, EmployeeRepository>();
        services.AddScoped<EmployeeService>();

        return services;
    }

    public static IServiceCollection AddDatabase(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton<IDbConnectionFactory>(sp => new PostgresConnectionFactory(configuration));
        
        return services;
    }
}