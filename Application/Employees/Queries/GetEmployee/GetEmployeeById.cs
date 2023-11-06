using Application.Common.Interfaces;
using Application.Employees.Commands.CreateEmployee;
using Application.Helpers;
using Domain.Entities;
using Domain.Enums;
using Domain.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Application.Employees.Queries.GetEmployee;

public record GetEmployeeByIdCommand(int EmployeeId) : IRequest<GetEmployeeByIdDto>;

public sealed class GetEmployeeByIdCommandHandler : IRequestHandler<GetEmployeeByIdCommand, GetEmployeeByIdDto>
{
    private readonly IApplicationDbContext _context;
    private readonly IMediator _mediator;
    private readonly ILogger<GetEmployeeByIdCommandHandler> _logger;

    public GetEmployeeByIdCommandHandler(IApplicationDbContext context, IMediator mediator, ILogger<GetEmployeeByIdCommandHandler> logger)
    {
        _context = context;
        _mediator = mediator;
        _logger = logger;
    }

    public async Task<GetEmployeeByIdDto> Handle(GetEmployeeByIdCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.Employees
            .AsNoTracking()
            .Include(e => e.Companies)
            .FirstOrDefaultAsync(e => e.Id == request.EmployeeId);

        if (entity is null)
        {
            _logger.LogError($"Employee with id {request.EmployeeId} is not found in db.");
            throw new EmployeeNotFoundException(request.EmployeeId.ToString());
        }
        var employee = new GetEmployeeByIdDto(
                entity.Id,
                $"{entity.FirstName} {entity.LastName}",
                entity.Email,
                entity.Title.ToString());

        var systemLogCommand = SystemLogHelper.PrepareEmployeeSystemLogCommand(
           entity,
            $"% retrieving employee with email {employee.Email}%",
            EventType.Get);

        await _mediator.Send(systemLogCommand);

        return employee;
    }
}