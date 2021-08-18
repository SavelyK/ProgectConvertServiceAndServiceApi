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
               
                var hostConvert = CreateConvertHostBuilder(args).Build();
                hostConvert.Run();
                IHostBuilder CreateConvertHostBuilder(string[] args)
                {

                    return Host.CreateDefaultBuilder()

                        .ConfigureServices(services =>
                        {

                            services.AddHostedService<Start>();
                            services.AddSingleton<IStartService, StartService>();
                            //services.AddSingleton<IMethods, Methods>();
                           // services.AddDbContext<ApplicationContext>(options => options.UseSqlServer(appConfigurationConfig.ConnectionString));
                        });

                }

            }

    }
}
