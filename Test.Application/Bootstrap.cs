using Microsoft.Extensions.DependencyInjection;
using Test.Application.Interfaces;
using Test.Application.Services;

namespace Test.Application;

public static class Bootstrap
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<IEmployeeService, EmployeeService>();

        return services;
    }
}