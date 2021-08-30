using ConvertService.Models;
using DbInformation;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ConvertService.Interfases
{
   public interface IMethods
    {
        Task SaveDocxModelAsync(InformationDbContext context, ConcurrentQueue<DocxItemModel> nameQueue, ConcurrentQueue<DocxItemModel> complitedQueue);
        Task Convert(InformationDbContext context, ConcurrentQueue<DocxItemModel> nameQueue, int maxCount, ConcurrentQueue<DocxItemModel> complitedQueue);
        Task EnqueConvert(InformationDbContext context, ConcurrentQueue<DocxItemModel> nameQueue);
        void ServiceStart(InformationDbContext context, ConcurrentQueue<DocxItemModel> nameQueue);
        Task SaveFileDbAsync(InformationDbContext context);
    }
}
