using DbInformation.Models;
using Microsoft.EntityFrameworkCore;
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
