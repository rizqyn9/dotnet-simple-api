namespace SampleApi.Presentation.Common.Responses
{
  /// <summary>
  /// The base structure for all API responses.
  /// </summary>
  public abstract class BaseResponse
  {
    public bool Success { get; init; }
    public string Message { get; init; } = string.Empty;
    public string TraceId { get; init; } = string.Empty;
    public string? Code { get; init; }
  }
}
