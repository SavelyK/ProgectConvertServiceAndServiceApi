using System;
using MediatR;

namespace Repository_Application.Repositorys.Commands.SaveDocxFile
{
    public class SaveDocxRepositoryCommand : IRequest<Guid>
    {
        public Guid UserId { get; set; }
        public Guid Id { get; set; }
        public string FileName { get; set; }
        public string Path { get; set; }
        public DateTime LoadTime { get; set; }
        public string Status { get; set; }
        public int Priority { get; set; }
        public long FileLength { get; set; }
 
    }
}
