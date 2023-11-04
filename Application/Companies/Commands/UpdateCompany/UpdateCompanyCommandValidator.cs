using FluentValidation;

namespace Application.Companies.Commands.UpdateCompany;

public class UpdateCompanyCommandValidator : AbstractValidator<UpdateCompanyCommand>
{
    public UpdateCompanyCommandValidator()
    {
        RuleFor(v => v.CompanyId)
            .NotNull()
            .NotEmpty();
    }
}