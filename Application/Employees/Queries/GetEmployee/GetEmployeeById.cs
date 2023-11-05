using Application.Common.Interfaces;
using Application.Helpers;
using Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Employees.Queries.GetEmployee;

public record GetEmployeeByIdCommand(int EmployeeId) : IRequest;

public sealed class GetEmployeeByIdCommandHandler : IRequestHandler<GetEmployeeByIdCommand>
{
    private readonly IApplicationDbContext _context;
    private readonly IMediator _mediator;

    public GetEmployeeByIdCommandHandler(IApplicationDbContext context, IMediator mediator)
    {
        _context = context;
        _mediator = mediator;
    }

    public async Task Handle(GetEmployeeByIdCommand request, CancellationToken cancellationToken)
    {
        var employee = await _context.Employees
            .Include(e => e.Companies)
            .FirstOrDefaultAsync(e => e.Id == request.EmployeeId);

        if (employee is null)
        {
            return;
        }

        //map to dto
        //return dot

        var systemLogCommand = SystemLogHelper.PrepareEmployeeSystemLogCommand(
            employee,
            $"% retrieving employee with email {employee.Email}%",
            EventType.Get);

        await _mediator.Send(systemLogCommand);
    }
}