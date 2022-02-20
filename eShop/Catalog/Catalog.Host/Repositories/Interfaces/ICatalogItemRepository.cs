using Catalog.Host.Data;
using Catalog.Host.Data.Entities;

namespace Catalog.Host.Repositories.Interfaces;

public interface ICatalogItemRepository
{
    Task<PaginatedItems<CatalogItem>> GetByPageAsync(int pageIndex, int pageSize, int? brandFilter, int? typeFilter);
    Task<IdItems<CatalogItem>> GetByIdAsync(int id);
    Task<BrandedItems<CatalogItem>> GetByBrandAsync(CatalogBrand brand);
    Task<TypedItems<CatalogItem>> GetByTypeAsync(CatalogType type);
    Task<Brands<CatalogBrand>> GetBrandsAsync(string brandsQueryMessage);
    Task<Types<CatalogType>> GetTypesAsync(string typeQueryMessage);
    Task<int?> Add(string name, string description, decimal price, int availableStock, int catalogBrandId, int catalogTypeId, string pictureFileName);
    Task<int?> Remove(int id);
    Task<int?> Update(int id, string name, string description, decimal price, int availableStock, int catalogBrandId, int catalogTypeId, string pictureFileName);
}