using System.Net;

namespace SampleApi.Presentation.Common.Responses
{
  /// <summary>
  /// Factory helper for generating consistent API responses.
  /// </summary>
  public static class ResponseFactory
  {
    public static ApiResponse<T> Success<T>(T data, string message = "Success", string? traceId = null) =>
      new()
      {
        Success = true,
        Message = message,
        Code = ResponseCodes.Success,
        Data = data,
        TraceId = traceId ?? Guid.NewGuid().ToString()
      };

    public static ApiResponse<T> Failure<T>(
      string message,
      string code = ResponseCodes.UnknownError,
      string? traceId = null,
      T? data = default) =>
      new()
      {
        Success = false,
        Message = message,
        Code = code,
        Data = data,
        TraceId = traceId ?? Guid.NewGuid().ToString()
      };

    public static ErrorResponse ValidationFailed(
      Dictionary<string, string[]> errors,
      string? traceId = null) =>
      new()
      {
        Success = false,
        Message = "Validation failed.",
        Code = ResponseCodes.ValidationFailed,
        TraceId = traceId ?? Guid.NewGuid().ToString(),
        Fields = errors
      };

    public static ErrorResponse FromException(Exception ex, string? traceId = null) =>
      new()
      {
        Success = false,
        Message = ex.Message,
        Code = ResponseCodes.InternalError,
        TraceId = traceId ?? Guid.NewGuid().ToString()
      };
  }

  public static class ResponseCodes
  {
    public const string Success = "SUCCESS";
    public const string ValidationFailed = "VALIDATION_FAILED";
    public const string Unauthorized = "UNAUTHORIZED";
    public const string NotFound = "NOT_FOUND";
    public const string Conflict = "CONFLICT";
    public const string InternalError = "INTERNAL_ERROR";
    public const string UnknownError = "UNKNOWN_ERROR";
  }
}
