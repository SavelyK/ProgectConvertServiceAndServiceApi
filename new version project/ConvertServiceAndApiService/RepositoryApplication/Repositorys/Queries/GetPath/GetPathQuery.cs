using MediatR;
using System;


namespace RepositoryApplication.Repositorys.Queries.GetPath
{
    public class GetPathQuery : IRequest<PathVm>
    {
        public Guid UserId { get; set; }
        public string FileName { get; set; }
    }
}
