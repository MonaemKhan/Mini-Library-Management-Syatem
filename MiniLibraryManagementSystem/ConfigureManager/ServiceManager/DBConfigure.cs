using DataAccessManager;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ServiceManager.ReturnManagement;

namespace ConfigureManager.ServiceManager
{
    public static partial class ServiceManager
    {
        public static IServiceCollection DBConfigure(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection")??string.Empty;
            // Register AppDBConnection
            services.AddDbContext<AppDBConnection>(options =>
                options.UseSqlServer(connectionString));

            //reginster DataAccessManager
            services.AddScoped(typeof(IEFCoreDataAccessManager<>), typeof(EFCoreDataAccessManager<>));
            DapperDataAccessManager.SetConnectionString(connectionString);

            return services;
        }
    }
}
