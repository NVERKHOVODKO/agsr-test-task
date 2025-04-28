using Patient.Core.Exceptions.Base;

namespace Patient.Core.Exceptions;

public class NotFoundException : ApiException
{
    public NotFoundException(string message, object? additionalData = null) 
        : base(message, 404, additionalData)
    {
    }
}