using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using UserManagement.Data;
using System;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApplicationDb(
        this IServiceCollection services,
        string connectionString,
        IHostEnvironment env)
    {

        services.AddDbContext<DataContext>(options =>
        {
            if (env.IsEnvironment("Development"))
            {
                // Local dev: InMemory (no VPN/Azure required)
                options.UseInMemoryDatabase("InMemoryDb");
                 Console.WriteLine("Using Inmemorydb");
            }
            else
            {
                // Production: Azure SQL
                options.UseSqlServer(connectionString);
                Console.WriteLine("Using sql");
                
            }
        });

        services.AddScoped<IDataContext, DataContext>();

        return services;
    }
}
