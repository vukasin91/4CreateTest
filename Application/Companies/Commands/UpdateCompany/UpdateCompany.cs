using Application.Common.Interfaces;
using Domain.Entities;
using MediatR;

namespace Application.Companies.Commands.UpdateCompany;

public record UpdateCompanyCommand : IRequest
{
    public Guid CompanyId { get; init; }
    public List<Employee> Employees { get; init; }
}

public sealed class UpdateCompanyCommandHandler : IRequestHandler<UpdateCompanyCommand>
{
    private readonly IApplicationDbContext _context;

    public UpdateCompanyCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task Handle(UpdateCompanyCommand request, CancellationToken cancellationToken)
    {
        var company = await _context.Companies.FindAsync(request.CompanyId);

        if (company == null)
        {
            return;
        }

        //todo
    }
}