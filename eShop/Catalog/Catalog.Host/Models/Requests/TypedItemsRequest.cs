using Catalog.Host.Data.Entities;

namespace Catalog.Host.Models.Requests
{
    public class TypedItemsRequest
    {
        public CatalogType Type { get; init; } = null!;
    }
}
