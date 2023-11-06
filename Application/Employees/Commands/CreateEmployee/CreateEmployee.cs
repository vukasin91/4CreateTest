using Application.Common.Interfaces;
using Application.Helpers;
using Domain.Entities;
using Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Application.Employees.Commands.CreateEmployee;

public record CreateEmployeeCommand(
    IEnumerable<int> CompaniesIds,
    string FirstName,
    string LastName,
    string Email,
    EmployeeType Title
    ) : IRequest;


public sealed class CreateEmployeeCommandHandler : IRequestHandler<CreateEmployeeCommand>
{
    private readonly IApplicationDbContext _context;
    private readonly IMediator _mediator;
    private readonly ILogger<CreateEmployeeCommandHandler> _logger;

    public CreateEmployeeCommandHandler(IApplicationDbContext context, IMediator mediator, ILogger<CreateEmployeeCommandHandler> logger)
    {
        _context = context;
        _mediator = mediator;
        _logger = logger;
    }

    public async Task Handle(CreateEmployeeCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var employeeTitle = request.Title;
            var companies = await _context.Companies
                .Where(c => request.CompaniesIds.Any(rc => rc == c.Id))
                .ToListAsync(cancellationToken);

            foreach (var company in companies)
            {
                if (company.Employees.Any(e => e.Title == employeeTitle))
                {
                    _logger.LogError($"An employee with title {request.Title} already exists inside company with name {company.Name}");
                    throw new InvalidOperationException($"A {request.Title} already exists in company {company.Name}.");
                }
            }

            var newEmployee = Employee.Create(
                request.FirstName,
                request.LastName,
                request.Email,
                employeeTitle,
                companies);

            _context.Employees.Add(newEmployee);

            await _context.SaveChangesAsync(cancellationToken);

            var systemLogCommand = SystemLogHelper.PrepareEmployeeSystemLogCommand(
                newEmployee,
               $"new employee %{newEmployee.Email}% was created",
               EventType.Create);

            await _mediator.Send(systemLogCommand, cancellationToken);
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError($"Employee with email: {request.Email} already exists. ", ex.Message);
            throw;
        }
        catch(Exception ex)
        {
            _logger.LogError($"Something went wrong while creating employee with email: {request.Email}", ex.Message );
            throw;
        }
    }
}