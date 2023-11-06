using Application.Common.Interfaces;
using Application.Employees.Commands.DeleteEmployee;
using Application.Helpers;
using Domain.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Application.Employees.Commands.UpdateEmployee;

public record UpdateEmployeeCompanyCommand(
    int EmployeeId,
    IEnumerable<int> CompanyIds) : IRequest;

public sealed class UpdateEmployeeCompanyCommandHandler : IRequestHandler<UpdateEmployeeCompanyCommand>
{
    private readonly IApplicationDbContext _context;
    private readonly IMediator _mediator;
    private readonly ILogger<UpdateEmployeeCompanyCommandHandler> _logger;

    public UpdateEmployeeCompanyCommandHandler(IApplicationDbContext context, IMediator mediator, ILogger<UpdateEmployeeCompanyCommandHandler> logger)
    {
        _context = context;
        _mediator = mediator;
        _logger = logger;
    }

    public async Task Handle(UpdateEmployeeCompanyCommand request, CancellationToken cancellationToken)
    {
        var employee = await _context.Employees
            .Include(e => e.Companies)
            .FirstOrDefaultAsync(e => e.Id == request.EmployeeId);

        if (employee is null)
        {
            _logger.LogError($"Employee with id {request.EmployeeId} is not found in db");
            throw new EmployeeNotFoundException(request.EmployeeId.ToString());
        }

        var toBeDeleted = employee.Companies
            .Where(c => !request.CompanyIds.Any(ci => ci == c.Id))
            .ToList();

        if (toBeDeleted.Any())
        {
            foreach (var company in toBeDeleted)
            {
                employee.RemoveCompany(company);
            }
        }

        var toBeAdded = request.CompanyIds
            .ToList()
            .RemoveAll(c => employee.Companies.Any(ec => ec.Id == c));

        var foundCompaniesToBeAdded = _context.Companies.Where(c => request.CompanyIds.Any(tba => tba == c.Id));

        if (foundCompaniesToBeAdded.Any())
        {
            foreach (var company in foundCompaniesToBeAdded)
            {
                employee.AddCompany(company);
            }
        }

        _context.Employees.Update(employee);
        await _context.SaveChangesAsync(cancellationToken);

        var systemLogCommand = SystemLogHelper.PrepareEmployeeSystemLogCommand
            (
            employee,
            $"% employee with email {employee.Email} has been updated%",
            Domain.Enums.EventType.Update
            );

        await _mediator.Send(systemLogCommand, cancellationToken);
    }
}