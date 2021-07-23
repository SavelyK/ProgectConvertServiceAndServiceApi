using MediatR;
using RepositoryApplication.Interfases;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace Repository_Application.Repositorys.Commands.AddQueueDocxFile
{
    public class AddQueueDocxCommandHandler
        : IRequestHandler<AddQueueDocxCommand>
    {
        private readonly IRepositoryDbContext _dbContext;
        public AddQueueDocxCommandHandler(IRepositoryDbContext dbContext) =>
            _dbContext = dbContext;
        public async Task<Unit> Handle(AddQueueDocxCommand request, CancellationToken cancellationToken)
        {
            var entity =
                await _dbContext.Repositorys.FirstOrDefaultAsync(repository => repository.Status == "Wait", cancellationToken);
        if(entity == null || entity.Status != request.Status)
            {
              
            }
            return Unit.Value;
        }
        //private readonly IRepositoryDbContext _dbContext;
        //public AddQueueDocxCommandHandler(IRepositoryDbContext dbContext) =>
        //    _dbContext = dbContext;
        //public async Task<Guid> Handle(AddQueueDocxCommand request,
        //    CancellationToken cancellationToken)
        //{
        //    var repository = new Repository
        //    {
        //        Id = Guid.NewGuid(),   
        //        Path = request.Path,
        //        LoadTime = DateTime.Now,
        //        Status = "Wait",
        //        Priority = request.Priority,
        //        FileLength = request.FileLength,
        //        TaskTime = null
        //    };
        //    await _dbContext.Repositorys.AddAsync(repository, cancellationToken);
        //    await _dbContext.SaveChangesAsync(cancellationToken);
        //    return repository.Id;
        }

}
