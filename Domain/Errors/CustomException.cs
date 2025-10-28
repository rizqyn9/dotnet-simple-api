using System.Net;

namespace SampleApi.Domain.Errors;

public class CustomException : Exception
{
  public string Code { get; }
  public int StatusCode { get; }

  public CustomException(string message, string code, int statusCode = (int)HttpStatusCode.BadRequest)
      : base(message)
  {
    Code = code;
    StatusCode = statusCode;
  }
}
