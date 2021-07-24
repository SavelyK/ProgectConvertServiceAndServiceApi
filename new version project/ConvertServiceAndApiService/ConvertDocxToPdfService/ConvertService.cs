using ConvertDocxToPdfService.Interfases;
using Microsoft.Extensions.Hosting;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace ConvertDocxToPdfService
{
    public class ConvertService : IHostedService
    {
        private readonly IStartConvertService _myService;
        public ConvertService(IStartConvertService service)
        {
            _myService = service ?? throw new ArgumentNullException(nameof(service));
        }
        public Task StartAsync(CancellationToken cancellationToken)
        {
            return _myService.Run();
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            Console.WriteLine("Service stopped");
            return Task.CompletedTask;
        }

    }
}
