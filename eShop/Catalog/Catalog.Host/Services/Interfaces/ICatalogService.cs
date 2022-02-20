using Catalog.Host.Models.Dtos;
using Catalog.Host.Models.Enums;
using Catalog.Host.Models.Response;
using Catalog.Host.Data.Entities;

namespace Catalog.Host.Services.Interfaces;

public interface ICatalogService
{
    Task<PaginatedItemsResponse<CatalogItemDto>?> GetCatalogItemsAsync(int pageSize, int pageIndex, Dictionary<CatalogTypeFilter, int>? filters);
    Task<IdItemsResponse<CatalogItemDto>> GetByIdAsync(int id);
    Task<BrandedItemsResponse<CatalogItemDto>> GetByBrandAsync(CatalogBrand catalogBrand);
    Task<TypedItemsResponse<CatalogItemDto>> GetByTypeAsync(CatalogType catalogType);
    Task<BrandsResponse<CatalogBrandDto>> GetBrandsAsync();
    Task<TypesResponse<CatalogTypeDto>> GetTypesAsync();
}