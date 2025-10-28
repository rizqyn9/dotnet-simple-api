namespace SampleApi.DTOs
{
  public class ApiResponse<T>
  {
    public bool Success { get; set; } = true;
    public string Message { get; set; } = string.Empty;
    public T? Data { get; set; }

    public ApiResponse() { }

    public ApiResponse(T data, string message = "")
    {
      Data = data;
      Message = message;
    }

    public static ApiResponse<T> SuccessResponse(T data, string message = "")
        => new ApiResponse<T>(data, message);

    public static ApiResponse<T> Fail(string message)
        => new ApiResponse<T> { Success = false, Message = message };
  }
}
