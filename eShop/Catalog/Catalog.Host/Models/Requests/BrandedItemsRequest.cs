using Catalog.Host.Data.Entities;

namespace Catalog.Host.Models.Requests
{
    public class BrandedItemsRequest
    {
        public CatalogBrand Brand { get; init; } = null!;
    }
}
