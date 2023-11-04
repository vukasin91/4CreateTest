using Domain.Abstractions;
using Domain.Enums;

namespace Domain.Entities;

public sealed class Employee : Entity
{
    public Employee(Guid id, string firstName, string lastName, string email, DateTime createdAt)
        :base(id)
    {
        FirstName = firstName;
        LastName = lastName;
        Email = email;
        CreatedAt = createdAt;
    }

    private Employee() { }

    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public EmployeeType Title { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? LastModifiedAt { get; set; }

    public List<Company> Companies { get; } = new();
}