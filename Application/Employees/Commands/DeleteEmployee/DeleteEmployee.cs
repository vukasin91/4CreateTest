using Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Employees.Commands.DeleteEmployee;

public record DeleteEmployeeCommand(Guid EmployeeId) : IRequest;

public sealed class DeleteEmployeeCommandHandler : IRequestHandler<DeleteEmployeeCommand>
{
    private readonly IApplicationDbContext _context;

    public DeleteEmployeeCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task Handle(DeleteEmployeeCommand request, CancellationToken cancellationToken)
    {
        await _context.Employees
            .Where(e => e.Id == request.EmployeeId)
            .ExecuteDeleteAsync(cancellationToken);
    }
}