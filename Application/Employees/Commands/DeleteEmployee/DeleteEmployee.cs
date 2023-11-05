using Application.Common.Interfaces;
using Application.Helpers;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Employees.Commands.DeleteEmployee;

public record DeleteEmployeeCommand(int EmployeeId) : IRequest;

public sealed class DeleteEmployeeCommandHandler : IRequestHandler<DeleteEmployeeCommand>
{
    private readonly IApplicationDbContext _context;
    private readonly IMediator _mediator;

    public DeleteEmployeeCommandHandler(IApplicationDbContext context, IMediator mediator)
    {
        _context = context;
        _mediator = mediator;
    }

    public async Task Handle(DeleteEmployeeCommand request, CancellationToken cancellationToken)
    {
        await _context.Employees
            .Where(e => e.Id == request.EmployeeId)
            .ExecuteDeleteAsync(cancellationToken);

        var systemLogCommand = SystemLogHelper.PrepareEmployeeSystemLogCommand(
            null,
            $"% employee with id: {request.EmployeeId} was deleted %",
            Domain.Enums.EventType.Delete);

        await _mediator.Send(systemLogCommand, cancellationToken);
    }
}