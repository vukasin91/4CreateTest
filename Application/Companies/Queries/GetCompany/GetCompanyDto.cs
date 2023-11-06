using Domain.Entities;

namespace Application.Companies.Queries.GetCompany;

public record GetCompanyDto(
    int Id,
    string Name,
    IEnumerable<Employee> Employees,
    DateTime CreatedAt);