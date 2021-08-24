using Calabonga.Configuration.Json;
using ConvertService.Interfases;
using ConvertService.Models;
using DbInformation;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;

namespace ConvertService
{
    public class StartService : IStartService
    {

        private readonly InformationDbContext _context;
        public StartService(InformationDbContext context)
        {
            _context = context;
        }

        ConcurrentQueue<DocxItemModel> convertQueue = new ConcurrentQueue<DocxItemModel>();
        public static int count = 0;
     


        public async Task Run()
        {
            
            var appConfiguration = new AppConfiguration(new JsonConfigurationSerializer());
            var appConfigurationConfig = appConfiguration.Config;

            Methods start = new Methods();
            Task saveDocx = Task.Run(async() => {
            await start.SaveDocxModelAsync(_context, convertQueue);

        });
            Task convertDocx = Task.Run(async () => {
               
                await start.Convert(_context, convertQueue, appConfigurationConfig.MaxCount);
            });


        }
       
    }
}
