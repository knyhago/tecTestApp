using Microsoft.EntityFrameworkCore;
using UserManagement.Data;

namespace Microsoft.Extensions.DependencyInjection;

public static class ServiceCollectionExtensions
{
   public static IServiceCollection AddDataAccess(this IServiceCollection services, string connectionString)
        {
            // Register DbContext with Azure SQL
            services.AddDbContext<DataContext>(options =>
                options.UseSqlServer(connectionString),
            ServiceLifetime.Singleton);
                

            // Register IDataContext
            services.AddScoped<IDataContext, DataContext>();

            return services;
        }
   
}
