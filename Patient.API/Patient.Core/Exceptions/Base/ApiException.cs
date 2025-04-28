namespace Patient.Core.Exceptions.Base;

/// <summary>
/// Base exception class for API-related errors with HTTP status code support.
/// </summary>
public abstract class ApiException : Exception
{
    /// <summary>
    /// HTTP status code associated with this exception.
    /// </summary>
    public int StatusCode { get; }

    /// <summary>
    /// Additional error data to include in the API response.
    /// </summary>
    public object? AdditionalData { get; }

    /// <summary>
    /// Initializes a new instance of the API exception.
    /// </summary>
    /// <param name="message">Error message describing the exception.</param>
    /// <param name="statusCode">HTTP status code (default: 500 Internal Server Error).</param>
    /// <param name="additionalData">Optional additional error data.</param>
    protected ApiException(string message, int statusCode = 500, object? additionalData = null) 
        : base(message)
    {
        StatusCode = statusCode;
        AdditionalData = additionalData;
    }
}