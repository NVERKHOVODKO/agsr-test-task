using Patient.API.Exceptions.Base;

namespace Patient.API.Exceptions;

public class NotFoundException : ApiException
{
    public NotFoundException(string message, object? additionalData = null) 
        : base(message, 404, additionalData)
    {
    }
}