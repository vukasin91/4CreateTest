namespace Domain.Exceptions;

public class EmployeeNotFoundException : Exception
{
    public EmployeeNotFoundException(string id)
        : base($"The employee with identifier {id} was not found.")
    {
    }
}