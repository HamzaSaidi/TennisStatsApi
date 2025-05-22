namespace TennisStatsApi.Exception;

public class NotFoundException:ApiException
{
    public NotFoundException(string message, string errorCode = null)
        : base(message, 404, errorCode ?? "not_found") { }

}