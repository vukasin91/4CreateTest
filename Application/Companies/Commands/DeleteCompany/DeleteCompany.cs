using Application.Common.Interfaces;
using Application.Helpers;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Companies.Commands.DeleteCompany;

public record DeleteCompanyCommand(int CompanyId) : IRequest;

public sealed class DeleteCompanyCommandHandler : IRequestHandler<DeleteCompanyCommand>
{
    private readonly IApplicationDbContext _context;
    private readonly IMediator _mediator;

    public DeleteCompanyCommandHandler(IApplicationDbContext context, IMediator mediator)
    {
        _context = context;
        _mediator = mediator;
    }

    public async Task Handle(DeleteCompanyCommand request, CancellationToken cancellationToken)
    {
        await _context.Companies
            .Where(c => c.Id == request.CompanyId)
            .ExecuteDeleteAsync(cancellationToken);

        var systemLogCommand = SystemLogHelper.PrepareCompanySystemLogCommand(
            null,
            $"% company with id: {request.CompanyId} was deleted %",
            Domain.Enums.EventType.Delete);

        await _mediator.Send(systemLogCommand, cancellationToken);
    }
}