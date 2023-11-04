using FluentValidation;

namespace Application.Employees.Commands.UpdateEmployee;

public class UpdateEmployeeCommandValidator : AbstractValidator<UpdateEmployeeCommand>
{
    public UpdateEmployeeCommandValidator()
    {
        RuleFor(v => v.FirstName)
             .MaximumLength(50)
             .NotEmpty()
             .NotNull();

        RuleFor(v => v.LastName)
            .MaximumLength(50)
            .NotEmpty()
            .NotNull();

        RuleFor(v => v.Email)
            .EmailAddress()
            .NotEmpty()
            .NotNull();

        RuleFor(v => v.Title)
            .IsInEnum()
            .NotNull()
            .NotEmpty();
    }
}