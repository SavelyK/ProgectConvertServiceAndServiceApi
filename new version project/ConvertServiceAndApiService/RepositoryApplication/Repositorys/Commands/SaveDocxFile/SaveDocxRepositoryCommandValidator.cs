using FluentValidation;
using System;


namespace RepositoryApplication.Repositorys.Commands.SaveDocxFile
{
    public class SaveDocxRepositoryCommandValidator : AbstractValidator<SaveDocxRepositoryCommand>
    {
        public SaveDocxRepositoryCommandValidator()
        {
            RuleFor(saveDocxRepositoryCommand =>
                saveDocxRepositoryCommand.FileName).NotEmpty().MaximumLength(250);
            RuleFor(saveDocxRepositoryCommand =>
                saveDocxRepositoryCommand.UserId).NotEqual(Guid.Empty);
        }

    }
}
