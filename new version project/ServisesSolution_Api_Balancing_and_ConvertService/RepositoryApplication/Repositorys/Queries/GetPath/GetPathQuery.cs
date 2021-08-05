using MediatR;
using System;


namespace Repository_Application.Repositorys.Queries.GetPath
{
    public class GetPathQuery : IRequest<PathVm>
    {
        public Guid UserId { get; set; }
        public string FileName { get; set; }
    }
}
