using ConvertDocxToPdfService;
using ConvertDocxToPdfService.Interfases;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RepositoryPersistence;
using System;
using System.Threading.Tasks;

namespace RepositoryWebApi
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
         
            var hostApi = CreateApiHostBuilder(args).Build();
            using (var scope = hostApi.Services.CreateScope())
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
            }
            var hostConvert = CreateConvertHostBuilder(args).Build();

            await Task.WhenAny
               (
                hostConvert.RunAsync(),
                hostApi.RunAsync()
               );
        }
        public static IHostBuilder CreateConvertHostBuilder(string[] args)
        {
            return Host.CreateDefaultBuilder()
                .ConfigureServices(services =>
                {
                    services.AddHostedService<ConvertService>();
                    services.AddSingleton<IStartConvertService, StartConvertService>();
                    services.AddSingleton<IMethods, Methods>();

                });
        }
            public static IHostBuilder CreateApiHostBuilder(string[] args)
        {
           return Host.CreateDefaultBuilder()
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });

        }
    }
}
