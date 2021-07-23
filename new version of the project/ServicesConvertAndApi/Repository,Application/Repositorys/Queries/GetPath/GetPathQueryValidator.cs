using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Repository_Application.Repositorys.Queries.GetPath
{
    public class GetPathQueryValidator : AbstractValidator<GetPathQuery>
    {
        public GetPathQueryValidator()
        {
            RuleFor(repository => repository.UserId).NotEqual(Guid.Empty);
            RuleFor(repository => repository.FileName).NotEmpty().MaximumLength(200);

        }
    }
}
