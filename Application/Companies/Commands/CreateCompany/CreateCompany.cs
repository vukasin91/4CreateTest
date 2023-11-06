using Application.Common.Interfaces;
using Application.Employees.Commands.CreateEmployee;
using Application.Helpers;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Application.Companies.Commands.CreateCompany;

public record CreateCompanyCommand(
    string CompanyName,
    IEnumerable<Employee> Employees) : IRequest;

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