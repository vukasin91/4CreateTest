using Domain.Abstractions;
using Domain.Enums;

namespace Domain.Entities;

/// <summary>
/// Company
/// </summary>
public sealed class Company : Entity
{
    public string Name { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime LastModifiedAt { get; private set; }

    private readonly List<Employee> _employees = new();

    
    public IReadOnlyCollection<Employee> Employees => _employees.AsReadOnly();

   
    private Company(string name)
    {
        Name = name;
        CreatedAt = DateTime.UtcNow;
    }

    private Company()
    {
            
    }

    /// <summary>
    /// Create new instance of Company
    /// </summary>
    /// <param name="name"></param>
    /// <returns>Company object</returns>
    public static Company Create(string name)
    {
        ArgumentException.ThrowIfNullOrEmpty(name, "Name of the company could not be null or empty");

        return new Company(name);
    }

    /// <summary>
    /// Update company name, or update list of employees
    /// </summary>
    /// <param name="newName"></param>
    /// <param name="employees"></param>
    public void UpdateCompanyName(string newName)
    {
        ArgumentException.ThrowIfNullOrEmpty(newName, "Name of the company could not be null or empty");

        Name = newName;

        LastModifiedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Adds new employee to a company
    /// </summary>
    /// <param name="employee"></param>
    /// <exception cref="InvalidOperationException"></exception>
    public void AddEmployee(Employee employee)
    {
        ArgumentException.ThrowIfNullOrEmpty(nameof(employee), "Employee cannot be null or empty.");

        if (_employees.Any(e => e.Title == employee.Title))
            throw new InvalidOperationException($"An employee with the title {employee.Title} already exists in the company.");

        _employees.Add(employee);
    }

    /// <summary>
    /// Removes an employee from company
    /// </summary>
    /// <param name="employee"></param>
    public void RemoveEmployee(Employee employee)
    {
        ArgumentException.ThrowIfNullOrEmpty(nameof(employee), "Employee cannot be null or empty.");

        _employees.Remove(employee);
    }
}