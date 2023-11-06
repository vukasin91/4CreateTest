using Application.SystemLogs.Commands.CreateSystemLog;
using Domain.Entities;
using Domain.Enums;

namespace Application.Helpers;

public static class SystemLogHelper
{
    public static CreateSystemLogCommand PrepareEmployeeSystemLogCommand(
        Employee? entity,
        string comment,
        EventType eventType)
    {
        Dictionary<string, object>? attributes = null;
        if (entity is not null)
        {
            attributes = new Dictionary<string, object>
            {
                { nameof(entity.FirstName),  entity.FirstName },
                { nameof(entity.LastName),  entity.LastName },
                { nameof(entity.Email),  entity.Email },
                { nameof(entity.Title),  entity.Title.ToString() }
            };
        }

        return new CreateSystemLogCommand(
            nameof(Employee),
            entity!.Id,
            eventType,
            attributes,
            comment);
    }

    public static CreateSystemLogCommand PrepareCompanySystemLogCommand(
        Company? entity,
        string comment,
        EventType eventType)
    {
        Dictionary<string, object>? attributes = null;
        if (entity is not null)
        {
            attributes = new Dictionary<string, object>
            {
                { nameof(entity.Name),  entity.Name },
            };
        }

        return new CreateSystemLogCommand(
            nameof(Employee),
            entity!.Id,
            eventType,
            attributes,
            comment);
    }
}