﻿using Application.Common.Interfaces;
using Application.Employees.Queries.GetEmployee;
using Application.Helpers;
using Domain.Entities;
using Domain.Enums;
using Domain.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Application.Companies.Queries.GetCompany;

public record GetCompanyByIdCommand(int CompanyId) : IRequest<GetCompanyDto>;

public sealed class GetCompanyByIdCommandHandler : IRequestHandler<GetCompanyByIdCommand, GetCompanyDto>
{
    private readonly IApplicationDbContext _context;
    private readonly IMediator _mediator;
    private readonly ILogger<GetCompanyByIdCommandHandler> _logger;

    public GetCompanyByIdCommandHandler(IApplicationDbContext context, IMediator mediator, ILogger<GetCompanyByIdCommandHandler> logger)
    {
        _context = context;
        _mediator = mediator;
        _logger = logger;
    }

    public async Task<GetCompanyDto> Handle(GetCompanyByIdCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.Companies
            .AsNoTracking()
            .Include(x => x.Employees)
            .FirstOrDefaultAsync(c => c.Id == request.CompanyId);

        if (entity is null)
        {
            _logger.LogError($"Company with id {request.CompanyId} is not found in db.");
            throw new EmployeeNotFoundException(request.CompanyId.ToString());
        }

        var company = new GetCompanyDto(
                entity.Id,
                entity.Name,
                entity.Employees,
                entity.CreatedAt);

        var systemLogCommand = SystemLogHelper.PrepareCompanySystemLogCommand(
            entity,
            $"% retrieving company with Id {company.Id}%",
            EventType.Get);

        await _mediator.Send(systemLogCommand, cancellationToken);

        return company;
    }
}