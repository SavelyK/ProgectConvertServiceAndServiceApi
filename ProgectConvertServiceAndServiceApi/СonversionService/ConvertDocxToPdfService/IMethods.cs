using LibraryModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ConvertDocxToPdfService
{
   public interface IMethods
    {
        public Task QueueLiquidatorAsync();
        public void ServiceRestart();
        public Reserv SelectClient(Queue<Reserv>[] nameArrayQueues);
        public void Convert(Reserv res);
        public Task EnqueueQueueAsync(Queue<Reserv>[] nameArrayQueues);
        

    }
}
