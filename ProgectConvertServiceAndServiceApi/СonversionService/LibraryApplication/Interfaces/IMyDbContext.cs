using System.Threading;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using LibraryModels;

namespace LibraryApplication.Interfaces
{
    public interface IMyDbContext
    {
        DbSet<DbModel> DbModels { get; set; }
        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}
