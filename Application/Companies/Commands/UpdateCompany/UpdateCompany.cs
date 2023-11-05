using Application.Common.Interfaces;
using Application.Helpers;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Companies.Commands.UpdateCompany;

public record UpdateCompanyCommand(
    int CompanyId,
    IEnumerable<Employee> Employees) : IRequest;

public sealed class UpdateCompanyCommandHandler : IRequestHandler<UpdateCompanyCommand>
{
    private readonly IApplicationDbContext _context;
    private readonly IMediator _mediator;

    public UpdateCompanyCommandHandler(IApplicationDbContext context, IMediator mediator)
    {
        _context = context;
        _mediator = mediator;
    }

    public async Task Handle(UpdateCompanyCommand request, CancellationToken cancellationToken)
    {
        var company = await _context.Companies
            .Include(c => c.Employees)
            .FirstOrDefaultAsync(c => c.Id == request.CompanyId);

        if (company == null)
        {
            return;
        }

        var toBeDeleted = company.Employees
           .Where(e => !request.Employees.Any(re => re.Id == e.Id))
           .ToList();

        if (toBeDeleted.Any())
        {
            foreach (var employee in toBeDeleted)
            {
                company.RemoveEmployee(employee);
            }
        }

        var toBeAdded = request.Employees
            .ToList()
            .RemoveAll(e => company.Employees.Any(ce => ce.Id == e.Id));

        var foundCompaniesToBeAdded = _context.Employees.Where(c => request.Employees.Any(tba => tba.Id == c.Id));

        if (foundCompaniesToBeAdded.Any())
        {
            foreach (var employee in foundCompaniesToBeAdded)
            {
                company.AddEmployee(employee);
            }
        }

        _context.Companies.Update(company);
        await _context.SaveChangesAsync(cancellationToken);

        var systemLogCommand = SystemLogHelper.PrepareCompanySystemLogCommand(
            company,
            $"% employee with id {company.Id} has been updated%",
            Domain.Enums.EventType.Update
            );

        await _mediator.Send(systemLogCommand, cancellationToken);
    }
}