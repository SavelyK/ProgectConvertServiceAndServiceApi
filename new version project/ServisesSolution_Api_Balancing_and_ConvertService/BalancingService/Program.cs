using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RepositoryPersistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BalancingService
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var hostBalancing = CreateApiHostBuilder(args).Build();
            
            hostBalancing.Run();
        }

        public static IHostBuilder CreateApiHostBuilder(string[] args) =>
              Host.CreateDefaultBuilder()
                 .ConfigureWebHostDefaults(webBuilder =>
                 {
                     webBuilder.UseStartup<Startup>();
                 });

    }
}
