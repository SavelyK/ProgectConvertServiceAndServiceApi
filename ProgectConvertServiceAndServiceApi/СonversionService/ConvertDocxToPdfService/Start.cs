using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace ConvertDocxToPdfService
{

    public  class Start : IHostedService
    {
        private readonly IStartService _myService;
        public Start(IStartService service)
        {
            _myService = service ?? throw new ArgumentNullException(nameof(service));
        }
        public Task StartAsync(CancellationToken cancellationToken)
        {
           return  _myService.Run();
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            Console.WriteLine("Service stopped");
            return Task.CompletedTask;
        }

    }
}
        
