using Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Companies.Commands.DeleteCompany;

public record DeleteCompanyCommand(Guid CompanyId) : IRequest;

public sealed class DeleteCompanyCommandHandler : IRequestHandler<DeleteCompanyCommand>
{
    private readonly IApplicationDbContext _context;

    public DeleteCompanyCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task Handle(DeleteCompanyCommand request, CancellationToken cancellationToken)
    {
        await _context.Companies
            .Where(c => c.Id == request.CompanyId)
            .ExecuteDeleteAsync(cancellationToken);

    }
}