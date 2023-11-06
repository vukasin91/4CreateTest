namespace Domain.Exceptions;

public class CompanyNotFoundException : Exception
{
    public CompanyNotFoundException(string id)
        : base($"The company with identifier {id} was not found.")
    {
    }
}