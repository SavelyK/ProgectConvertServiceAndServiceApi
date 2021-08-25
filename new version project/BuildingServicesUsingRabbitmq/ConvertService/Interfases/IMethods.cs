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
        Task SaveDocxModelAsync(InformationDbContext context);
        Task Convert(InformationDbContext context, ConcurrentQueue<DocxItemModel> nameQueue, int maxCount);
        Task EnqueConvert(InformationDbContext context, ConcurrentQueue<DocxItemModel> nameQueue);
        void ServiceStart(InformationDbContext context, ConcurrentQueue<DocxItemModel> nameQueue);
    }
}
