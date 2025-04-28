using Patient.Core.Exceptions.Base;

namespace Patient.Core.Exceptions;

public class BadRequestException : ApiException
{
    public BadRequestException(string message, object? additionalData = null) 
        : base(message, 400, additionalData)
    {
    }
}