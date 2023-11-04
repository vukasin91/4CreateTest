using Domain.Abstractions;

namespace Domain.Entities;

public sealed class Company : Entity
{
    public Company(Guid id, string name, DateTime createdAt)
        : base(id)
    {
        Name = name;
        CreatedAt = createdAt;
    }

    private Company()
    {
    }

    public string Name { get; set; }
    public DateTime CreatedAt { get; set; }

    public List<Employee> Employees { get; } = new();
}