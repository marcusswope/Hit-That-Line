using HitThatLine.Endpoints.Thread.Models;
using HitThatLine.Infrastructure.Validation;
using FluentValidation;

namespace HitThatLine.Endpoints.Thread.Validation
{
    public class NewThreadCommandValidator : ConventionBasedValidator<NewThreadCommand>
    {
        public NewThreadCommandValidator()
        {
            RuleFor(x => x.TagInput)
                .Must((command, input) => 
                    !string.IsNullOrWhiteSpace(command.Tags) ||
                    !string.IsNullOrWhiteSpace(command.TagInput))
                .WithMessage("required");
        }
    }
}