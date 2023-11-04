using FluentValidation;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Application.Employees.Commands.DeleteEmployee;

public class DeleteEmployeeCommandValidator : AbstractValidator<DeleteEmployeeCommand>
{
    public DeleteEmployeeCommandValidator()
    {
        RuleFor(x => x.EmployeeId)
            .NotEmpty()
            .NotNull();
    }
}