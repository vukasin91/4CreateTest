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

    public string FirstName { get; private set; }
    public string LastName { get; private set; }
    public string Email { get; private set; }
    public EmployeeType Title { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime? LastModifiedAt { get; private set; }
}