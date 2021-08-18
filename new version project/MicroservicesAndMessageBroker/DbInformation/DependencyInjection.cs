using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using DbInformation;
using DbInformation.Interfases;

namespace DbInformation.Models
{
   public static class DependencyInjection
    {

        public static IServiceCollection AddDbInformation(this IServiceCollection
            services, string connectionString)
        {

            services.AddDbContext<InformationDbContext>(options =>
            {
                options.UseSqlServer(connectionString);
            });
            services.AddScoped<IInformationDbContext>(provider =>
            provider.GetService<InformationDbContext>());
            return services;
        }
    }
}
