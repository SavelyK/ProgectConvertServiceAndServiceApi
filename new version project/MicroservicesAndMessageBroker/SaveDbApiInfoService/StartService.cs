
using DbInformation;
using SaveDbApiInfoService.Interfases;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;

namespace SaveDbApiInfoService
{
    public class StartService : IStartService
    {

        private readonly InformationDbContext _context;
        public StartService(InformationDbContext context)
        {
            _context = context;
        }



        public async Task Run()
        {
            Methods start = new Methods();
            Task saveDocx = Task.Run(async () =>
            {
                await start.SaveFileInformationAsync(_context);

            });

        }
    }
}
