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
        private readonly InformationDbContext _context2;
        private readonly InformationDbContext _context3;
        public StartService(InformationDbContext context, InformationDbContext context2, InformationDbContext context3)
        {
            _context = context;
            _context2 = context2;
            _context3 = context3;
        }
        

        ConcurrentQueue<DocxItemModel> convertQueue = new ConcurrentQueue<DocxItemModel>();
        ConcurrentQueue<DocxItemModel> complitedQueue = new ConcurrentQueue<DocxItemModel>();
        public static int count = 0;
        public static int countIndex = 0;


        public async Task Run()
        {

            var appConfiguration = new AppConfiguration(new JsonConfigurationSerializer());
            var appConfigurationConfig = appConfiguration.Config;

            Methods start = new Methods();
           // start.ServiceStart(_context, convertQueue);

            Task saveDocx = Task.Run(async () =>
            {
                await start.SaveFileDbAsync(_context);
            });
            Task EnqueDocx = Task.Run(async () =>
            {

                await start.EnqueConvert(_context2, convertQueue);
            });
            //Task saveDocx = Task.Run(async () =>
            //{
            //    await start.SaveDocxModelAsync(_context, convertQueue, complitedQueue);

            //});
            Task convertDocx = Task.Run(async () =>
            {

                await start.Convert(_context3, convertQueue, appConfigurationConfig.MaxCount, complitedQueue);
            });

        }

    }
}
