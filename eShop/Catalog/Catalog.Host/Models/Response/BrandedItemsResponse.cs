using Catalog.Host.Data.Entities;

namespace Catalog.Host.Models.Response
{
    public class BrandedItemsResponse<T>
    {
        public CatalogBrand Brand { get; init; } = null!;

        public IEnumerable<T> Data { get; init; } = null!;

        public long TotalCount { get; init; }
    }
}
