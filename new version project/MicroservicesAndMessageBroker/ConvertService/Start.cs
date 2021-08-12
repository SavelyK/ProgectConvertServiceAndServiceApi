
using ConvertService.Interfases;
using Microsoft.Extensions.Hosting;
using System;
using System.Threading;
using System.Threading.Tasks;


namespace ConvertService
{
    public class Start : IHostedService
    {
        private readonly IStartService _myService;
        public Start(IStartService service)
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
