namespace Catalog.Host.Models.Response
{
    public class BrandsResponse<T>
    {
        public IEnumerable<T> Data { get; set; } = null!;
        public long TotalCount { get; init; }
    }
}
