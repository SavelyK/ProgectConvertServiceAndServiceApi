using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using RepositoryApplication.Interfases;

namespace RepositoryPersistence
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddPersistence(this IServiceCollection
            services, IConfiguration configuration)
        {
            var connectionString = configuration["DbConnection"];
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
