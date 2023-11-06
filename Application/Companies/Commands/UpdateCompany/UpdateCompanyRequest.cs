using Domain.Entities;

namespace Application.Companies.Commands.UpdateCompany;

public record UpdateCompanyRequest(IEnumerable<Employee> Employees);