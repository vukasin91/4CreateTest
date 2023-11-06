using Application.Common.Interfaces;
using Application.Employees.Commands.CreateEmployee;
using Application.Helpers;
using Domain.Entities;
using Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Dynamic;

namespace Application.Companies.Commands.CreateCompany;

public record CreateCompanyCommand(
    string CompanyName,
    IEnumerable<EmployeeDto> Employees) : IRequest;

public record EmployeeDto(
    string? FirstName,
    string? LastName,
    string? Email,
    string? Title,
    int id);

public sealed class CreateCompanyCommandHandler : IRequestHandler<CreateCompanyCommand>
{
    private readonly IApplicationDbContext _context;
    private readonly IMediator _mediator;
    private readonly ILogger<CreateCompanyCommandHandler> _logger;

    public CreateCompanyCommandHandler(IApplicationDbContext context, IMediator mediator, ILogger<CreateCompanyCommandHandler> logger)
    {
        _context = context;
        _mediator = mediator;
        _logger = logger;
    }

    public async Task Handle(CreateCompanyCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var nonExisitngEmployees = request.Employees
                .Where(x => !string.IsNullOrWhiteSpace(x.Email)).ToList();

            //now in request we only have existing employees in db
            request.Employees
                .ToList()
                .RemoveAll(e => string.IsNullOrWhiteSpace(e.Email) && e.id == 0);

            var existingEmployeesIds = request.Employees.Select(x => x.id);

            var employees = await _context.Employees
                .Where(x => existingEmployeesIds.Any(e => e == x.Id))
                .ToListAsync();

            var newCompany = Company.Create(request.CompanyName);
            _context.Companies.Add(newCompany);

            foreach (var employee in employees)
            {
                newCompany.AddEmployee(employee);
            }

            await _context.SaveChangesAsync(cancellationToken);

            var systemLogCompanyCreate = SystemLogHelper.PrepareCompanySystemLogCommand(
                newCompany,
                $"% company with name {newCompany.Name} has been created %",
                Domain.Enums.EventType.Create);

            await _mediator.Send(systemLogCompanyCreate, cancellationToken);

            var companyIds = new List<int> { newCompany.Id };

            await CreateNewEmployees(nonExisitngEmployees, companyIds, cancellationToken);
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError("There was a problem while creating entities in db", ex.Message);
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError("Ther was an error occured", ex.Message);
            throw;
        }
    }

    private async Task CreateNewEmployees(List<EmployeeDto> nonExisitngEmployees, IEnumerable<int> companyId, CancellationToken cancellationToken)
    {
        foreach (var employee in nonExisitngEmployees)
        {
            var createEmployeeCommand = new CreateEmployeeCommand(
                companyId,
                employee.FirstName,
                employee.LastName,
                employee.Email,
                Enum.Parse<EmployeeType>(employee.Title)
                );

            await _mediator.Send(createEmployeeCommand, cancellationToken);
        }
    }
}