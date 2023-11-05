using Domain.Abstractions;
using Domain.Enums;

namespace Domain.Entities;

public class SystemLog : Entity
{
    public string EntityType { get; set; }
    public int EntityId { get; set; }
    public DateTime CreatedAt { get; set; }
    public EventType Event { get; set; }
    public string EntityAttributes { get; set; }
    public string? Comment { get; set; }

    private SystemLog(
        string entityType,
        int entityId,
        EventType evenType,
        string entityAttributes,
        string? comment)
    {
        EntityType = entityType;
        EntityId = entityId;
        Event = evenType;
        EntityAttributes = entityAttributes;
        Comment = comment;
        CreatedAt = DateTime.UtcNow;
    }

    private SystemLog()
    {
    }

    public static SystemLog Create(
        string entityType,
        int entityId,
        EventType eventType,
        string entityAttributes,
        string? comment
        )
    {
        ArgumentException.ThrowIfNullOrEmpty(entityType, "Resource type could not be null or empty");
        ArgumentException.ThrowIfNullOrEmpty(nameof(eventType), "Event type could not be null or empty");
        ArgumentException.ThrowIfNullOrEmpty(entityAttributes, "Change set could not be null or empty");

        return new SystemLog(
            entityType,
            entityId,
            eventType,
            entityAttributes,
            comment);
    }
}