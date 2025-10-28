namespace SampleApi.Presentation.Common.Responses
{
  public class ApiResponse<T>
  {
    public bool Success { get; set; }
    public string Message { get; set; } = string.Empty;
    public string? Code { get; set; }
    public T? Data { get; set; }

    public static ApiResponse<T> SuccessResponse(T data, string message = "Success") =>
        new() { Success = true, Message = message, Code = "SUCCESS", Data = data };

    public static ApiResponse<T> Fail(string message, string code = "UNKNOWN_ERROR") =>
        new() { Success = false, Message = message, Code = code };
  }

  public class ErrorResponse
  {
    public bool Success { get; set; } = false;
    public string Message { get; set; } = string.Empty;
    public string? Code { get; set; }
    public Dictionary<string, string[]>? Fields { get; set; }
  }
}
