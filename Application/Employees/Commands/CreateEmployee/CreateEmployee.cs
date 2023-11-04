using Application.Common.Interfaces;
using Domain.Entities;
using Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Application.Employees.Commands.CreateEmployee;

public record CreateEmployeeCommand() : IRequest<int>
{
    public Guid CompanyId { get; init; }

    public string FirstName { get; init; }
    public string LastName { get; init; }
    public string Email { get; init; }
    public string Title { get; init; }
}

public sealed class CreateEmployeeCommandHandler : IRequestHandler<CreateEmployeeCommand, int>
{
    private readonly IApplicationDbContext _context;

    public CreateEmployeeCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<int> Handle(CreateEmployeeCommand request, CancellationToken cancellationToken)
    {
        var entity = new Employee
        (
            request.FirstName,
            request.LastName,
            request.Email,
            Enum.Parse<EmployeeType>(request.Title)
        );

        _context.Employees.Add(entity);

        await _context.SaveChangesAsync(cancellationToken);

        return entity.Id;
    
    }
}