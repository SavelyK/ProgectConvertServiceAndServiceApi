using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ServicePersistence;
using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using ServiceWebApi;

namespace ConvertDocxToPdfService
{
    public static class Program
    {
        public static async Task<int> Main(string[] args)
        {
            var host = CreateHostBuilder();
            await host.RunConsoleAsync();
            return Environment.ExitCode;

        }

        public static IHostBuilder CreateHostBuilder()
        {
            return Host.CreateDefaultBuilder()
                .ConfigureServices(services =>
                {
                    services.AddHostedService<Start>()
                    .AddDbContext<MyDbContext>(options => options.UseSqlServer("Server=localhost;Database=Data54;Trusted_Connection=True;"));
                    services.AddSingleton<IStartService, StartService>();
                    services.AddSingleton<IMethods, Methods>();
                })
                  .ConfigureWebHostDefaults(webBuilder =>
                  {
                      webBuilder.UseStartup<Startup>();
                      webBuilder.UseUrls("http://localhost:5000/");
                  });
        }

                

    }
}
