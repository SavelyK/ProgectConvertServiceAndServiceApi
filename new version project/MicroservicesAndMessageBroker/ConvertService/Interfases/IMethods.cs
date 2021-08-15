using ConvertService.Models;
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
        Task SaveDocxModelAsync(ApplicationContext context, ConcurrentQueue<DocxItemModel> nameQueue);
        Task Convert(ApplicationContext context, ConcurrentQueue<DocxItemModel> nameQueue, int maxCount);

    }
}
