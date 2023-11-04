using Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Employees.Queries.GetEmployee;

public record GetEmployeeByIdCommand(Guid EmployeeId) : IRequest;

public sealed class GetEmployeeByIdCommandHandler : IRequestHandler<GetEmployeeByIdCommand>
{
    private readonly IApplicationDbContext _context;

    public GetEmployeeByIdCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task Handle(GetEmployeeByIdCommand request, CancellationToken cancellationToken)
    {
        var employee = await _context.Employees
            .Include(e => e.Companies)
            .FirstOrDefaultAsync(e => e.Id == request.EmployeeId);
    }
}