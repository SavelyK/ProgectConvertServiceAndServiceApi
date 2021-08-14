using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using RepositoryApplication.Interfases;

namespace RepositoryPersistence
{
   public static class DependencyInjectionConvert
    {

        public static IServiceCollection AddConvertPersistence(this IServiceCollection
            services, string connectionString)
        {
            
            services.AddDbContext<RepositoryDbContext>(options =>
            {
                options.UseSqlServer(connectionString);
            });
            services.AddScoped<IRepositoryDbContext>(provider =>
            provider.GetService<RepositoryDbContext>());
            return services;
        }
    }
}
