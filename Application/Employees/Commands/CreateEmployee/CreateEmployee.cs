using Application.Common.Interfaces;
using Application.Helpers;
using Domain.Entities;
using Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Employees.Commands.CreateEmployee;

public record CreateEmployeeCommand(
    IEnumerable<int> CompaniesIds,
    string FirstName,
    string LastName,
    string Email,
    string Title
    ) : IRequest;


public sealed class CreateEmployeeCommandHandler : IRequestHandler<CreateEmployeeCommand>
{
    private readonly IApplicationDbContext _context;
    private readonly IMediator _mediator;

    public CreateEmployeeCommandHandler(IApplicationDbContext context, IMediator mediator)
    {
        _context = context;
        _mediator = mediator;
    }

    public async Task Handle(CreateEmployeeCommand request, CancellationToken cancellationToken)
    {
        if (await _context.Employees.AnyAsync(e => e.Email.ToLower() == request.Email.ToLower()))
        {
            throw new InvalidOperationException("An employee with this email already exists.");
        }

        var employeeTitle = Enum.Parse<EmployeeType>(request.Title);
        var companies = await _context.Companies
            .Where(c => request.CompaniesIds.Any(rc => rc == c.Id))
            .ToListAsync(cancellationToken);

        foreach (var company in companies)
        {
            if (company.Employees.Any(e => e.Title == employeeTitle))
            {
                //log
                throw new InvalidOperationException($"A {request.Title} already exists in one of the selected companies.");
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
}