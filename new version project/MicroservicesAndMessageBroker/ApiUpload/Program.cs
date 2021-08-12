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
        public static void Main(string[] args)
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
            hostApi.Run();
        }

        public static IHostBuilder CreateApiHostBuilder(string[] args) =>
              Host.CreateDefaultBuilder()
                 .ConfigureWebHostDefaults(webBuilder =>
                 {
                     webBuilder.UseStartup<Startup>();
                 });

    }
}

