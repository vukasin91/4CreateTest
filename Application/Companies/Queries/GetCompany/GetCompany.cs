using Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Companies.Queries.GetCompany;

public record GetCompanyCommand(Guid CompanyId) : IRequest;

public sealed class GetCompanyCommandHandler : IRequestHandler<GetCompanyCommand>
{
    private readonly IApplicationDbContext _context;

    public GetCompanyCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task Handle(GetCompanyCommand request, CancellationToken cancellationToken)
    {
        var company = await _context.Companies
            .Include(x => x.Employees)
            .FirstOrDefaultAsync(c => c.Id == request.CompanyId);
        //map to dto

            
    }
}