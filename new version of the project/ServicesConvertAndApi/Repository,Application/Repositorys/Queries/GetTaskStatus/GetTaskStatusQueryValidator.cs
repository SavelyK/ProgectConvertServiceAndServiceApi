using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Repository_Application.Repositorys.Queries.GetTaskStatus
{
    public class GetTaskStatusQueryValidator : AbstractValidator<GetTaskStatusQuery>
    {
        public GetTaskStatusQueryValidator()
        {
            RuleFor(repository => repository.UserId).NotEqual(Guid.Empty);

        }
    }
}
