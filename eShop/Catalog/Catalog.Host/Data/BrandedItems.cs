using Catalog.Host.Data.Entities;

namespace Catalog.Host.Data
{
    public class BrandedItems<T>
    {
        public long TotalCount { get; init; }

        public IEnumerable<T> Data { get; init; } = null!;

        public CatalogBrand Brand { get; init; } = null!;
    }
}
