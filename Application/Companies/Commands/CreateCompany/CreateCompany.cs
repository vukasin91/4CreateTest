using Application.Common.Interfaces;
using Domain.Entities;
using MediatR;

namespace Application.Companies.Commands.CreateCompany;

public record CreateCompanyCommand(string CompanyName) : IRequest;

public sealed class CreateCompanyCommandHandler : IRequestHandler<CreateCompanyCommand>
{
    private readonly IApplicationDbContext _context;

    public CreateCompanyCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task Handle(CreateCompanyCommand request, CancellationToken cancellationToken)
    {
        var newCompany = new Company
        {
            Name = request.CompanyName
        };

        _context.Companies.Add(newCompany);

        await _context.SaveChangesAsync(cancellationToken);
    }
}