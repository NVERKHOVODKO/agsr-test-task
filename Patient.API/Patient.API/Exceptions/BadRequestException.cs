using Patient.API.Exceptions.Base;

namespace Patient.API.Exceptions;

public class BadRequestException : ApiException
{
    public BadRequestException(string message, object? additionalData = null) 
        : base(message, 400, additionalData)
    {
    }
}