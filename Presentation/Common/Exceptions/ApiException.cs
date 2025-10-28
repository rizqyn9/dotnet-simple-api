namespace SampleApi.Presentation.Common.Exceptions
{
  public abstract class ApiException : Exception
  {
    public string Code { get; }
    public int StatusCode { get; }

    protected ApiException(string message, string code, int statusCode)
      : base(message)
    {
      Code = code;
      StatusCode = statusCode;
    }
  }

  public class ConflictException : ApiException
  {
    public ConflictException(string message, string code = "NAME_CONFLICT")
      : base(message, code, StatusCodes.Status409Conflict) { }
  }

  public class NotFoundException : ApiException
  {
    public NotFoundException(string message, string code = "NOT_FOUND")
      : base(message, code, StatusCodes.Status404NotFound) { }
  }

  public class ValidationException : ApiException
  {
    public Dictionary<string, string[]> Errors { get; }

    public ValidationException(Dictionary<string, string[]> errors)
      : base("Validation failed.", "VALIDATION_FAILED", StatusCodes.Status400BadRequest)
    {
      Errors = errors;
    }
  }
}
