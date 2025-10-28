namespace SampleApi.Presentation.Common.Responses
{
  public class ApiResponse<T>
  {
    public bool Success { get; set; }
    public string Message { get; set; } = string.Empty;
    public string? Code { get; set; }
    public T? Data { get; set; }
    public string? TraceId { get; set; }  // 🧭 add this

    public static ApiResponse<T> SuccessResponse(T data, string message = "Success", string? traceId = null) =>
        new() { Success = true, Message = message, Code = "SUCCESS", Data = data, TraceId = traceId };

    public static ApiResponse<T> Fail(string message, string code = "UNKNOWN_ERROR", string? traceId = null) =>
        new() { Success = false, Message = message, Code = code, TraceId = traceId };
  }

  public class ErrorResponse
  {
    public bool Success { get; set; } = false;
    public string Message { get; set; } = string.Empty;
    public string? Code { get; set; }
    public Dictionary<string, string[]>? Fields { get; set; }
    public string? TraceId { get; set; }
  }
}
