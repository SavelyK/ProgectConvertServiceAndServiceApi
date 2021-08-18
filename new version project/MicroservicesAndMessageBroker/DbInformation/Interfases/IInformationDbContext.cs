using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DbInformation.Interfases
{
    public interface IInformationDbContext
    {
        DbSet<FileInformation> FileInformations { get; set; }
        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}
