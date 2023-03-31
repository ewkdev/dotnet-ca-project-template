using FluentValidation;

namespace CAWebProject.Application.Features.Example.Commands;

public class CreateExampleCommandValidator : AbstractValidator<CreateExampleCommand>
{
    public CreateExampleCommandValidator()
    {
        RuleFor(x => x.Topic).NotEmpty();
        RuleFor(x => x.Content).NotEmpty()
            .NotEmpty()
            .MaximumLength(255);
    }
}