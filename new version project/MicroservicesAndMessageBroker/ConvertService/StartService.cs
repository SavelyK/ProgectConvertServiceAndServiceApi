using Calabonga.Configuration.Json;
using ConvertService.Interfases;
using ConvertService.Models;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;

namespace ConvertService
{
    public class StartService : IStartService
    {
       
        private readonly ApplicationContext _context;
        public StartService(ApplicationContext context)
        {
            _context = context;
        }

        ConcurrentQueue<DocxItemModel> convertQueue = new ConcurrentQueue<DocxItemModel>();




        public async Task Run()
        {
            var appConfiguration = new AppConfiguration(new JsonConfigurationSerializer());
            var appConfigurationConfig = appConfiguration.Config;

            Methods start = new Methods();
            Task t = Task.Run(async() => {
            await start.SaveDocxModelAsync(_context, convertQueue);
            
        });
            Task v = Task.Run(async () => {
               
                await start.Convert(_context, convertQueue, appConfigurationConfig.MaxCount);
            });


        }
       
    }
}
