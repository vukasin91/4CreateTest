using Application.Common.Interfaces;
using Application.Employees.Commands.CreateEmployee;
using Application.Helpers;
using Domain.Exceptions;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Employees.Commands.DeleteEmployee;

public record DeleteEmployeeCommand(int EmployeeId) : IRequest;

public sealed class DeleteEmployeeCommandHandler : IRequestHandler<DeleteEmployeeCommand>
{
    private readonly IApplicationDbContext _context;
    private readonly IMediator _mediator;
    private readonly ILogger<DeleteEmployeeCommandHandler> _logger;

    public DeleteEmployeeCommandHandler(IApplicationDbContext context, IMediator mediator, ILogger<DeleteEmployeeCommandHandler> logger)
    {
        _context = context;
        _mediator = mediator;
        _logger = logger;
    }

    public async Task Handle(DeleteEmployeeCommand request, CancellationToken cancellationToken)
    {
        var employee = await _context.Employees.FindAsync(request.EmployeeId);
        if (employee is null)
        {
            _logger.LogError($"Employee with id {request.EmployeeId} is not found in db");
            throw new EmployeeNotFoundException(request.EmployeeId.ToString());
        }

        _context.Employees.Remove(employee);
        await _context.SaveChangesAsync(cancellationToken);

        var systemLogCommand = SystemLogHelper.PrepareEmployeeSystemLogCommand(
            employee,
            $"% employee with id: {request.EmployeeId} was deleted %",
            Domain.Enums.EventType.Delete);

        await _mediator.Send(systemLogCommand, cancellationToken);
    }
}