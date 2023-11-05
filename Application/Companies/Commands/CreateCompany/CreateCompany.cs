using Application.Common.Interfaces;
using Application.Employees.Commands.CreateEmployee;
using Application.Helpers;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Companies.Commands.CreateCompany;

public record CreateCompanyCommand(
    string CompanyName,
    IEnumerable<Employee> Employees) : IRequest;

public sealed class CreateCompanyCommandHandler : IRequestHandler<CreateCompanyCommand>
{
    private readonly IApplicationDbContext _context;
    private readonly IMediator _mediator;

    public CreateCompanyCommandHandler(IApplicationDbContext context, IMediator mediator)
    {
        _context = context;
        _mediator = mediator;
    }

    public async Task Handle(CreateCompanyCommand request, CancellationToken cancellationToken)
    {
        if (await _context.Companies.AnyAsync(c => c.Name.ToLower() == request.CompanyName.ToLower()))
        {
            throw new InvalidOperationException("An company with this name already exists.");
        }

        var nonExisitngEmployees = request.Employees
            .Where(x => !string.IsNullOrWhiteSpace(x.Email)).ToList();

        //now in request we only have existing employees
        request.Employees
            .ToList()
            .RemoveAll(e => string.IsNullOrWhiteSpace(e.Email));

        var newCompany = Company.Create(request.CompanyName);

        foreach (var employee in request.Employees)
        {
            newCompany.AddEmployee(employee);
        }

        _context.Companies.Add(newCompany);

        await _context.SaveChangesAsync(cancellationToken);

        var systemLogCompanyCreate = SystemLogHelper.PrepareCompanySystemLogCommand(
            newCompany,
            $"% company with name {newCompany.Name} has been created %",
            Domain.Enums.EventType.Create);

        await _mediator.Send(systemLogCompanyCreate, cancellationToken);

        var companyIds = new List<int> { newCompany.Id };

        await CreateNewEmployees(nonExisitngEmployees, companyIds, cancellationToken);
    }

    private async Task CreateNewEmployees(List<Employee> nonExisitngEmployees, IEnumerable<int> companyId, CancellationToken cancellationToken)
    {
        foreach (var employee in nonExisitngEmployees)
        {
            var createEmployeeCommand = new CreateEmployeeCommand(
                companyId,
                employee.FirstName,
                employee.LastName,
                employee.Email,
                employee.Title.ToString()
                );

            await _mediator.Send(createEmployeeCommand, cancellationToken);
        }
    }
}