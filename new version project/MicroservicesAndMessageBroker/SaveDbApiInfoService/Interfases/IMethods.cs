
using DbInformation;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SaveDbApiInfoService.Interfases
{
   public interface IMethods
    {
        Task SaveFileInformationAsync(InformationDbContext context);
     

    }
}
