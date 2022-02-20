using Catalog.Host.Data.Entities;

namespace Catalog.Host.Data
{
    public class TypedItems<T>
    {
        public long TotalCount { get; init; }

        public IEnumerable<T> Data { get; init; } = null!;

        public CatalogType Type { get; init; } = null!;
    }
}
