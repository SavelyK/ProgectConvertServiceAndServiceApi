using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BalancingService.Models
{
    public class DocxItemModel
    {
        public Guid Id { get; set; }
        public string Path { get; set; }
        public DateTime LoadTime { get; set; }
        public int Priority { get; set; }
        public long FileLength { get; set; }
        public DocxItemModel(Guid id, string path, DateTime loadTime, int priority, long fileLength)
        {
           Id = id;
           Path  = path;
           LoadTime  = loadTime;
           Priority  = priority;
           FileLength  = fileLength;
        }

    }
}
