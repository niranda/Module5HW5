namespace Catalog.Host.Models.Response
{
    public class TypesResponse<T>
    {
        public IEnumerable<T> Data { get; set; } = null!;
        public long TotalCount { get; init; }
    }
}
