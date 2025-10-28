namespace SampleApi.Presentation.Common.Responses
{
  /// <summary>
  /// Represents an error or validation failure response.
  /// </summary>
  public sealed class ErrorResponse : BaseResponse
  {
    public Dictionary<string, string[]>? Fields { get; init; }
  }
}
