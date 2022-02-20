namespace Catalog.Host.Data
{
    public class Types<T>
    {
        public IEnumerable<T> Data { get; set; } = null!;
        public long TotalCount { get; init; }
    }
}
