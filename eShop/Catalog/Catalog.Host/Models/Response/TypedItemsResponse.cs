using Catalog.Host.Data.Entities;

namespace Catalog.Host.Models.Response
{
    public class TypedItemsResponse<T>
    {
        public CatalogType Type { get; init; } = null!;

        public IEnumerable<T> Data { get; init; } = null!;

        public long TotalCount { get; init; }
    }
}
