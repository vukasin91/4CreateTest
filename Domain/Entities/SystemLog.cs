using Domain.Abstractions;
using Domain.Enums;

namespace Domain.Entities;

public class SystemLog<T> where T : Entity
{
    public SystemLog(T resourceType, EventType evenType, ChangeSet resourceAttributes, string comment)
    {
        EntityType = resourceType;
        CreatedAt = DateTime.UtcNow;
        Event = evenType;
        EntityAttributes = resourceAttributes;
        Comment = comment;
    }

    public T EntityType { get; set; }
    public DateTime CreatedAt { get; set; }
    public EventType Event { get; set; }
    public ChangeSet EntityAttributes { get; set; }
    public string? Comment { get; set; }
}

public class ChangeSet
{
    public Dictionary<string, object> Changes { get; set; } = new Dictionary<string, object>();
}