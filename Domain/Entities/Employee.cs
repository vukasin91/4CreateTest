using Domain.Abstractions;
using Domain.Enums;

namespace Domain.Entities;

/// <summary>
/// Employee
/// </summary>
public sealed class Employee : Entity
{
    public string FirstName { get; private set; }
    public string LastName { get; private set; }
    public string Email { get; private set; }
    public EmployeeType Title { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime? LastModifiedAt { get; private set; }

    private readonly List<Company> _companies = new();

    public IReadOnlyCollection<Company> Companies => _companies.AsReadOnly();

    private Employee(string firstName,
        string lastName,
        string email,
        EmployeeType title
        )
    {
        FirstName = firstName;
        LastName = lastName;
        Email = email;
        Title = title;
        CreatedAt = DateTime.UtcNow;
    }

    private Employee()
    { }

    /// <summary>
    /// Creates new instance of Employee
    /// </summary>
    /// <param name="firstName"></param>
    /// <param name="lastName"></param>
    /// <param name="email"></param>
    /// <param name="companies"></param>
    /// <returns>Employee object</returns>
    public static Employee Create(string firstName,
        string lastName,
        string email,
        EmployeeType title,
        IEnumerable<Company> companies)
    {
        ArgumentException.ThrowIfNullOrEmpty(firstName, "First Name of the employee could not be null or empty");
        ArgumentException.ThrowIfNullOrEmpty(lastName, "Last Name of the employee could not be null or empty");
        ArgumentException.ThrowIfNullOrEmpty(email, "Email of the employee could not be null or empty");
        ArgumentException.ThrowIfNullOrEmpty(nameof(title), "Employee title is invalid");
        ArgumentException.ThrowIfNullOrEmpty(nameof(companies), "Companies could not be null or empty");

        var employee = new Employee(
            firstName,
            lastName,
            email,
            title);

        employee._companies.AddRange(companies);

        return employee;
    }

    /// <summary>
    /// Adds new company to employee companies list
    /// </summary>
    /// <param name="company"></param>
    /// <exception cref="InvalidOperationException"></exception>
    public void AddCompany(Company company)
    {
        ArgumentException.ThrowIfNullOrEmpty(nameof(company), "Employee cannot be null or empty.");

        if (_companies.Any(c => company.Id == c.Id))
            throw new InvalidOperationException($"An company with the id {company.Id} already exists in the employee companies list.");

        _companies.Add(company);
    }

    public void RemoveCompany(Company company) 
    {
        ArgumentException.ThrowIfNullOrEmpty(nameof(company), "Employee cannot be null or empty.");

        _companies.Remove(company);
    }
}