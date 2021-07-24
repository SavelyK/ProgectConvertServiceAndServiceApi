using System;
using MediatR;

namespace RepositoryApplication.Repositorys.Queries.GetTaskStatus
{
    public class GetTaskStatusQuery : IRequest<TaskStatusVm>
    {
        public Guid UserId { get; set; }
        public Guid Id { get; set; }
    }
}