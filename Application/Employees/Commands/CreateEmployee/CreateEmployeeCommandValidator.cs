using FluentValidation;

namespace Application.Employees.Commands.CreateEmployee;

public class CreateEmployeeCommandValidator : AbstractValidator<CreateEmployeeCommand>
{
    public CreateEmployeeCommandValidator()
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