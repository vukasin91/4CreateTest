using Application.Common.Interfaces;
using Application.Helpers;
using Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Companies.Queries.GetCompany;

public record GetCompanyByIdCommand(int CompanyId) : IRequest;

public sealed class GetCompanyByIdCommandHandler : IRequestHandler<GetCompanyByIdCommand>
{
    private readonly IApplicationDbContext _context;
    private readonly IMediator _mediator;

    public GetCompanyByIdCommandHandler(IApplicationDbContext context, IMediator mediator)
    {
        _context = context;
        _mediator = mediator;
    }

    public async Task Handle(GetCompanyByIdCommand request, CancellationToken cancellationToken)
    {
        var company = await _context.Companies
            .Include(x => x.Employees)
            .FirstOrDefaultAsync(c => c.Id == request.CompanyId);
        //map to dto

        if (company is null)
        {
            return;
        }

        //map dto
        //return dto

        var systemLogCommand = SystemLogHelper.PrepareCompanySystemLogCommand(
            company,
            $"% retrieving company with Id {company.Id}%",
            EventType.Get);

        await _mediator.Send(systemLogCommand, cancellationToken);
    }
}