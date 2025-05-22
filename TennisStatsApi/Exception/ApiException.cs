namespace TennisStatsApi.Exception;

public class ApiException:System.Exception
{
    public int StatusCode { get; }
    public string ErrorCode { get; }

    public ApiException(string message, int statusCode = 500, string errorCode = null)
        : base(message)
    {
        StatusCode = statusCode;
        ErrorCode = errorCode ?? "api_error";
    }
    
}