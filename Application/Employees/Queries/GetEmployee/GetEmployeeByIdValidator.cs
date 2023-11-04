using FluentValidation;

namespace Application.Employees.Queries.GetEmployee;

public class GetEmployeeByIdValidator : AbstractValidator<GetEmployeeByIdCommand>
{
    public GetEmployeeByIdValidator()
    {
        RuleFor(v => v.EmployeeId)
            .NotNull()
            .NotEmpty();
    }
}