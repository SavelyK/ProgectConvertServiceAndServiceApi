using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ServicePersistence;
using System.Threading.Tasks;
using ConvertDocxToPdfService;


namespace ServiceWebApi
{
    public static class Program
    {
        public static async Task Main(string[] args)
        {
            var hostConvert = CreateConvertHostBuilder(args).Build(); //host convert service
               
            var hostApi = CreateApiHostBuilder(args).Build();  // host web api
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
                    services.AddHostedService<Start>() 
                    .AddDbContext<MyDbContext>(options => options.UseSqlServer("Server=localhost;Database=Data58;Trusted_Connection=True;"));
                    services.AddSingleton<IStartService, StartService>();
                    services.AddSingleton<IMethods, Methods>();

                });

        }
        public static IHostBuilder CreateApiHostBuilder(string[] args) 
        {
            return Host.CreateDefaultBuilder()
            .ConfigureWebHostDefaults(webBuilder =>
            {
               
                webBuilder.UseStartup<Startup>();
                webBuilder.UseUrls("http://localhost:5000/");
            });
        }



    }
}
