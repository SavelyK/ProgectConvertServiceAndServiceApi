using Calabonga.Configuration.Json;
using ConvertService.Interfases;
using DbInformation;
using DbInformation.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;

namespace ConvertService
{
    public class Program
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
                        services.AddSingleton<IMethods, Methods>();
                        services.AddDbInformation(appConfigurationConfig.ConnectionString);
                    });

            }

        }

    }
}
