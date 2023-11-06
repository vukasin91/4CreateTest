using FluentValidation;

namespace Application.SystemLogs.Commands.CreateSystemLog;

public class CreateSystemLogCommandValidator : AbstractValidator<CreateSystemLogCommand>
{
    public CreateSystemLogCommandValidator()
    {
        RuleFor(v => v.EntityType)
            .NotEmpty()
            .NotNull();

        RuleFor(v => v.EntityId)
            .NotEmpty()
            .NotNull();

        RuleFor(v => v.EventType)
            .NotNull();

        RuleFor(v => v.EntityAttributes)
            .NotEmpty()
            .NotNull();

        RuleFor(v => v.Comment)
            .NotEmpty()
            .NotNull();
    }
}