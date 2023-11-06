namespace Application.Employees.Queries.GetEmployee;

public record GetEmployeeByIdDto(int Id, string FullName, string Email, string Title);