using Calabonga.Configuration.Json;
using ConvertService.Interfases;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace ConvertService
{
    public class Program
    {

        static void Main(string[] args)
        {
            var appConfiguration = new AppConfiguration(new JsonConfigurationSerializer());
            var appConfigurationConfig = appConfiguration.Config;
            var hostConvert = CreateConvertHostBuilder(args).Build(); 
                hostConvert.Run();
             IHostBuilder CreateConvertHostBuilder(string[] args)
            {
                
                return Host.CreateDefaultBuilder()

                    .ConfigureServices(services =>
                    {

                        services.AddHostedService<Start>();
                        services.AddSingleton<IStartService, StartService>();
                        services.AddSingleton<IMethods, Methods>();
                        services.AddDbContext<ApplicationContext>(options => options.UseSqlServer(appConfigurationConfig.ConnectionString));
                    });

            }

        }

    }
}
