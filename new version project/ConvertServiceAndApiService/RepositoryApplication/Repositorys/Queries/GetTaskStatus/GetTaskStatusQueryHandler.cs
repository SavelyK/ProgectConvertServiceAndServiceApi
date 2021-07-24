using System.Threading;
using System.Threading.Tasks;
using RepositoryApplication.Interfases;
using MediatR;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using RepositoryDomain;
using RepositoryApplication.Common.Exceptions;

namespace RepositoryApplication.Repositorys.Queries.GetTaskStatus
{
    public class GetTaskStatusQueryHandler
         : IRequestHandler<GetTaskStatusQuery, TaskStatusVm>
    {
        private readonly IRepositoryDbContext _dbContext;
        private readonly IMapper _mapper;
        public GetTaskStatusQueryHandler(IRepositoryDbContext dbContext,
            IMapper mapper) => (_dbContext, _mapper) = (dbContext, mapper);
        public async Task<TaskStatusVm> Handle(GetTaskStatusQuery request,
            CancellationToken cancellationToken)
        {
            var entity = await _dbContext.Repositorys
                .FirstOrDefaultAsync(repository =>
                repository.Id == request.Id, cancellationToken);

            if (entity == null || entity.UserId != request.UserId)
            {
                throw new NotFoundException(nameof(Repository), request.Id);
            }
            return _mapper.Map<TaskStatusVm>(entity);
        }
    }
}
