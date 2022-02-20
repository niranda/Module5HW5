namespace Catalog.Host.Data
{
    public class Brands<T>
    {
        public IEnumerable<T> Data { get; set; } = null!;
        public long TotalCount { get; init; }
    }
}
