using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;
using RepositoryDomain;
namespace RepositoryApplication.Interfases
{
    public interface IRepositoryDbContext
    {
        DbSet<Repository> Repositorys { get; set; }
        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}
