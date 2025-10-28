namespace SampleApi.Application.DTOs
{
  public class PagedResponse<T> : ApiResponse<IEnumerable<T>>
  {
    public long Page { get; set; }
    public long PageSize { get; set; }
    public long TotalItems { get; set; }
    public long TotalPages => (long)Math.Ceiling((decimal)TotalItems / PageSize);

    public PagedResponse(IEnumerable<T> items, long page, long pageSize, long totalItems)
    {
      Data = items;
      Page = page;
      PageSize = pageSize;
      TotalItems = totalItems;
      Success = true;
    }
  }
}
