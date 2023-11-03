namespace Domain.Exceptions;

public class EntityNotFoundException : Exception
{
    public EntityNotFoundException(string entityType, string id)
        : base($"The resource of type {entityType} with identifier {id} was not found.")
    {
    }
}