using FluentValidation;

namespace Application.Companies.Commands.CreateCompany;

public class CreateCompanyCommandValidator : AbstractValidator<CreateCompanyCommand>
{
    public CreateCompanyCommandValidator()
    {
        RuleFor(v => v.CompanyName)
            .MinimumLength(3)
            .MaximumLength(150)
            .NotNull()
            .NotEmpty();
    }
}