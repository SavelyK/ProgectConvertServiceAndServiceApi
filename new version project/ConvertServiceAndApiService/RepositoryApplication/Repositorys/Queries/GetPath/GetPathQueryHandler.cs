﻿using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using RepositoryApplication.Common.Exceptions;
using RepositoryApplication.Interfases;
using RepositoryDomain;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RepositoryApplication.Repositorys.Queries.GetPath
{
    public class GetPathQueryHandler
            : IRequestHandler<GetPathQuery, PathVm>
    {
        private readonly IRepositoryDbContext _dbContext;
        private readonly IMapper _mapper;
        public GetPathQueryHandler(IRepositoryDbContext dbContext,
            IMapper mapper) => (_dbContext, _mapper) = (dbContext, mapper);
        public async Task<PathVm> Handle(GetPathQuery request,
            CancellationToken cancellationToken)
        {
            var entity = await _dbContext.Repositorys
                .FirstOrDefaultAsync(repository =>
                repository.FileName == request.FileName, cancellationToken);

            if (entity == null || entity.UserId != request.UserId)
            {
                throw new NotFoundException(nameof(Repository), request.FileName);
            }
            return _mapper.Map<PathVm>(entity);
        }
    }
}