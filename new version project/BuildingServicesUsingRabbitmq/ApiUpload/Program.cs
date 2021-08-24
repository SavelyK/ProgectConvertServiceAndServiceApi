using DbInformation;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;


namespace ApiUpload
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var hostApi = CreateApiHostBuilder(args).Build();
            hostApi.Run();
        }

        public static IHostBuilder CreateApiHostBuilder(string[] args) =>
              Host.CreateDefaultBuilder(args)
                 .ConfigureWebHostDefaults(webBuilder =>
                 {
                     webBuilder.UseStartup<Startup>();
                 });
    }
}
