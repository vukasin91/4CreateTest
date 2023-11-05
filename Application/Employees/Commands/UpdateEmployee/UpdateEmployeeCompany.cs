using Application.Common.Interfaces;
using Application.Helpers;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Employees.Commands.UpdateEmployee;

public record UpdateEmployeeCompanyCommand(
    int EmployeeId,
    IEnumerable<int> CompanyIds) : IRequest;

public sealed class UpdateEmployeeCompanyCommandHandler : IRequestHandler<UpdateEmployeeCompanyCommand>
{
    private readonly IApplicationDbContext _context;
    private readonly IMediator _mediator;

    public UpdateEmployeeCompanyCommandHandler(IApplicationDbContext context, IMediator mediator)
    {
        _context = context;
        _mediator = mediator;
    }

    public async Task Handle(UpdateEmployeeCompanyCommand request, CancellationToken cancellationToken)
    {
        var employee = await _context.Employees
            .Include(e => e.Companies)
            .FirstOrDefaultAsync(e => e.Id == request.EmployeeId);

        if (employee is null)
        {
            return;
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