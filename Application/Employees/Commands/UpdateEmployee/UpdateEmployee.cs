using Application.Common.Interfaces;
using Domain.Enums;
using MediatR;

namespace Application.Employees.Commands.UpdateEmployee;

public record UpdateEmployeeCommand : IRequest
{
    public Guid Id { get; init; }
    public required string FirstName { get; init; }
    public required string LastName { get; init; }
    public required string Email { get; init; }
    public required string Title { get; init; }
}

public sealed class UpdateEmployeeCommandHandler : IRequestHandler<UpdateEmployeeCommand>
{
    private readonly IApplicationDbContext _context;

    public UpdateEmployeeCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task Handle(UpdateEmployeeCommand request, CancellationToken cancellationToken)
    {
        var employee = await _context.Employees.FindAsync(request.Id);

        if (employee is null)
        {
            return;
        }

        employee.FirstName = request.FirstName; 
        employee.LastName = request.LastName; 
        employee.FirstName = request.Email;
        employee.Title = Enum.Parse<EmployeeType>(request.Title);
        
        _context.Employees.Update(employee);
        await _context.SaveChangesAsync(cancellationToken);
    }
}