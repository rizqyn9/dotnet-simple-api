namespace SampleApi.Presentation.Common.Responses
{
  /// <summary>
  /// Standard response for successful or failed API operations.
  /// </summary>
  public sealed class ApiResponse<T> : BaseResponse
  {
    public T? Data { get; init; }
  }
}
