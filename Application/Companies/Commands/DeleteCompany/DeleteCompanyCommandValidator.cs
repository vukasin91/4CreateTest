using FluentValidation;

namespace Application.Companies.Commands.DeleteCompany;

public class DeleteCompanyCommandValidator : AbstractValidator<DeleteCompanyCommand>
{
    public DeleteCompanyCommandValidator()
    {
        RuleFor(x => x.CompanyId)
            .NotNull()
            .NotEmpty();
    }
}