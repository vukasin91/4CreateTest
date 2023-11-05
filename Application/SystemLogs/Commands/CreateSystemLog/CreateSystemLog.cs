using Application.Common.Interfaces;
using Domain.Entities;
using Domain.Enums;
using MediatR;
using Newtonsoft.Json;

namespace Application.SystemLogs.Commands.CreateSystemLog;

public record CreateSystemLogCommand(
    string EntityType,
    int EntityId,
    EventType EventType,
    Dictionary<string, object>? EntityAttributes,
    string Comment) : IRequest;

public sealed class CreateSystemLogCommandHandler : IRequestHandler<CreateSystemLogCommand>
{
    private readonly IApplicationDbContext _context;

    public CreateSystemLogCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task Handle(CreateSystemLogCommand request, CancellationToken cancellationToken)
    {
        var changeSet = JsonConvert.SerializeObject(request.EntityAttributes);

        var systemLog = SystemLog.Create(
            request.EntityType,
            request.EntityId,
            request.EventType,
            changeSet,
            request.Comment
            );

        _context.SystemLogs.Add(systemLog);
        await _context.SaveChangesAsync(cancellationToken);
    }
}