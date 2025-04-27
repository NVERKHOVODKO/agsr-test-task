namespace Patient.API.Exceptions.Base;

public abstract class ApiException : Exception
{
    public int StatusCode { get; }
    public object? AdditionalData { get; }

    protected ApiException(string message, int statusCode = 500, object? additionalData = null) 
        : base(message)
    {
        StatusCode = statusCode;
        AdditionalData = additionalData;
    }
}