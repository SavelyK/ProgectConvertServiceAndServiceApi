using Calabonga.Configuration.Json;
using DbInformation;
using DbInformation.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SaveDbApiInfoService.Interfases;
using System;

namespace SaveDbApiInfoService
{
    class Program
    {


            static void Main(string[] args)
            {
            var appConfiguration = new AppConfiguration(new JsonConfigurationSerializer());
            var appConfigurationConfig = appConfiguration.Config;
            var hostConvert = CreateConvertHostBuilder(args).Build();
            using (var scope = hostConvert.Services.CreateScope())
            {
                var serviceProvider = scope.ServiceProvider;
                try
                {
                    var context = serviceProvider.GetRequiredService<InformationDbContext>();
                    DbInitializer.Initialize(context);
                }
                catch (Exception exeption)
                {


                }
            }
            hostConvert.Run();

                IHostBuilder CreateConvertHostBuilder(string[] args)
                {

                    return Host.CreateDefaultBuilder()

                        .ConfigureServices(services =>
                        {

                            services.AddHostedService<Start>();
                            services.AddSingleton<IStartService, StartService>();
                            services.AddDbInformation(appConfigurationConfig.ConnectionString);
                            services.AddSingleton<IMethods, Methods>();

                        });

                }

            }

    }
}
