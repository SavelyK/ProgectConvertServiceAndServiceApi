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
        public static int countIndex = 0;


        public async Task Run()
        {

            var appConfiguration = new AppConfiguration(new JsonConfigurationSerializer());
            var appConfigurationConfig = appConfiguration.Config;

            Methods start = new Methods();
            start.ServiceStart(_context, convertQueue);
            Task saveDocx = Task.Run(async () =>
            {
                await start.SaveDocxModelAsync(_context);

            });
            Task convertDocx = Task.Run(async () =>
            {

                await start.Convert(_context, convertQueue, appConfigurationConfig.MaxCount);
            });
            Task EnqueDocx = Task.Run(async () =>
            {

                await start.EnqueConvert(_context, convertQueue);
            });

        }

    }
}
