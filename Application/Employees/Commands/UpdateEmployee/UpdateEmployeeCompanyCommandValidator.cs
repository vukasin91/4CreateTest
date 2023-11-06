using FluentValidation;

namespace Application.Employees.Commands.UpdateEmployee;

public class UpdateEmployeeCompanyCommandValidator : AbstractValidator<UpdateEmployeeCompanyCommand>
{
    public UpdateEmployeeCompanyCommandValidator()
    {
        RuleFor(v => v.EmployeeId)
             .NotEmpty()
             .NotNull();

        RuleFor(v => v.CompanyIds)
            .NotNull()
            .NotEmpty();
    }
}