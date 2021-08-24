using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using DbInformation;
using DbInformation.Interfases;

namespace DbInformation
{
   public static class DependencyInjection
    {

        public static IServiceCollection AddDbInformationToApi(this IServiceCollection
            services, IConfiguration configuration)
        {
            var connectionString = configuration["DbConnection"];
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
