using Calabonga.Configuration.Json;
using ConvertService.Interfases;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Repository_Application.Common.Mappings;
using RepositoryApplication;
using RepositoryApplication.Interfases;
using RepositoryPersistence;
using System;
using System.Reflection;

namespace ConvertService
{
    public class Program
    {

        static void Main(string[] args)
        {
            var appConfiguration = new AppConfiguration(new JsonConfigurationSerializer());
            var appConfigurationConfig = appConfiguration.Config;
            var hostConvert = CreateConvertHostBuilder(args).Build(); //host convert service
            using (var scope = hostConvert.Services.CreateScope())
            {
                var serviceProvider = scope.ServiceProvider;
                try
                {
                    var context = serviceProvider.GetRequiredService<RepositoryDbContext>();
                    DbInitializer.Initialize(context);
                }
                catch (Exception exeption)
                {


                }

                hostConvert.Run();

                   

            }

             IHostBuilder CreateConvertHostBuilder(string[] args)
            {
                
                return Host.CreateDefaultBuilder()

                    .ConfigureServices(services =>
                    {

                        services.AddHostedService<Start>();
                        services.AddSingleton<IStartService, StartService>();
                        services.AddAutoMapper(config =>
                        {
                            config.AddProfile(new AssemblyMappingProfile(Assembly.GetExecutingAssembly()));
                            config.AddProfile(new AssemblyMappingProfile(typeof(IRepositoryDbContext).Assembly));
                        });
                        services.AddApplication();
                        services.AddConvertPersistence(appConfigurationConfig.ConnectionString);


                    });

            }

        }

    }
}
