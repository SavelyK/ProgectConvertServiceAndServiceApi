using ConvertService.Interfases;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;


namespace ConvertService
{
    class Program
    {
        static void Main(string[] args)
        {
            var hostConvert = CreateConvertHostBuilder(args).Build(); //host convert service


            hostConvert.Run();


        }

        public static IHostBuilder CreateConvertHostBuilder(string[] args)
        {
            return Host.CreateDefaultBuilder()
                .ConfigureServices(services =>
                {
                    services.AddHostedService<Start>();
                    services.AddSingleton<IStartService, StartService>();


                });

        }

    }

}
