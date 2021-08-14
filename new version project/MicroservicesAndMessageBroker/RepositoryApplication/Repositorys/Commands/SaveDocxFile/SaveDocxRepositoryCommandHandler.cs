﻿using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using RepositoryApplication.Interfases;
using RepositoryDomain;

namespace Repository_Application.Repositorys.Commands.SaveDocxFile
{
    public class SaveDocxRepositoryCommandHandler
        :IRequestHandler<SaveDocxRepositoryCommand, Guid>
    {
        private readonly IRepositoryDbContext _dbContext;
        public SaveDocxRepositoryCommandHandler(IRepositoryDbContext dbContext) =>
            _dbContext = dbContext;
        public async Task<Guid> Handle(SaveDocxRepositoryCommand request,
            CancellationToken cancellationToken)
        {
            var repository = new Repository
            {
                UserId = request.UserId,
                Id = Guid.NewGuid(),
                Path = request.Path,
            };
            await _dbContext.Repositorys.AddAsync(repository, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);
            return repository.Id;
        }
    }
}