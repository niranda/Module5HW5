using Catalog.Host.Data;
using Catalog.Host.Models.Dtos;
using Catalog.Host.Models.Enums;
using Catalog.Host.Models.Response;
using Catalog.Host.Repositories.Interfaces;
using Catalog.Host.Services.Interfaces;
using Catalog.Host.Data.Entities;

namespace Catalog.Host.Services;

public class CatalogService : BaseDataService<ApplicationDbContext>, ICatalogService
{
    private readonly ICatalogItemRepository _catalogItemRepository;
    private readonly IMapper _mapper;

    public CatalogService(
        IDbContextWrapper<ApplicationDbContext> dbContextWrapper,
        ILogger<BaseDataService<ApplicationDbContext>> logger,
        ICatalogItemRepository catalogItemRepository,
        IMapper mapper)
        : base(dbContextWrapper, logger)
    {
        _catalogItemRepository = catalogItemRepository;
        _mapper = mapper;
    }

    public async Task<PaginatedItemsResponse<CatalogItemDto>?> GetCatalogItemsAsync(int pageSize, int pageIndex, Dictionary<CatalogTypeFilter, int>? filters)
    {
        return await ExecuteSafeAsync(async () =>
        {
            int? brandFilter = null;
            int? typeFilter = null;

            if (filters != null)
            {
                if (filters.TryGetValue(CatalogTypeFilter.Brand, out var brand))
                {
                    brandFilter = brand;
                }

                if (filters.TryGetValue(CatalogTypeFilter.Type, out var type))
                {
                    typeFilter = type;
                }
            }

            var result = await _catalogItemRepository.GetByPageAsync(pageIndex, pageSize, brandFilter, typeFilter);
            if (result == null)
            {
                return null;
            }

            return new PaginatedItemsResponse<CatalogItemDto>()
            {
                Count = result.TotalCount,
                Data = result.Data.Select(s => _mapper.Map<CatalogItemDto>(s)).ToList(),
                PageIndex = pageIndex,
                PageSize = pageSize
            };
        });
    }

    public async Task<IdItemsResponse<CatalogItemDto>> GetByIdAsync(int id)
    {
        return await ExecuteSafeAsync(async () =>
        {
            var result = await _catalogItemRepository.GetByIdAsync(id);
            var data = _mapper.Map<CatalogItemDto>(result.Data);
            var temp = new IdItemsResponse<CatalogItemDto>()
            {
                Id = result.Id,
                Data = _mapper.Map<CatalogItemDto>(result.Data)
            };
            return temp;
        });
    }

    public async Task<BrandedItemsResponse<CatalogItemDto>> GetByBrandAsync(CatalogBrand catalogBrand)
    {
        return await ExecuteSafeAsync(async () =>
        {
            var result = await _catalogItemRepository.GetByBrandAsync(catalogBrand);
            return new BrandedItemsResponse<CatalogItemDto>()
            {
                Brand = result.Brand,
                Data = result.Data.Select(s => _mapper.Map<CatalogItemDto>(s)).ToList(),
                TotalCount = result.TotalCount,
            };
        });
    }

    public async Task<TypedItemsResponse<CatalogItemDto>> GetByTypeAsync(CatalogType catalogType)
    {
        return await ExecuteSafeAsync(async () =>
        {
            var result = await _catalogItemRepository.GetByTypeAsync(catalogType);
            return new TypedItemsResponse<CatalogItemDto>()
            {
                Type = result.Type,
                Data = result.Data.Select(s => _mapper.Map<CatalogItemDto>(s)).ToList(),
                TotalCount = result.TotalCount,
            };
        });
    }

    public async Task<BrandsResponse<CatalogBrandDto>> GetBrandsAsync()
    {
        return await ExecuteSafeAsync(async () =>
        {
            var result = await _catalogItemRepository.GetBrandsAsync("test");
            return new BrandsResponse<CatalogBrandDto>()
            {
                Data = result.Data.Select(s => _mapper.Map<CatalogBrandDto>(s)).ToList(),
                TotalCount = result.TotalCount,
            };
        });
    }

    public async Task<TypesResponse<CatalogTypeDto>> GetTypesAsync()
    {
        return await ExecuteSafeAsync(async () =>
        {
            var result = await _catalogItemRepository.GetTypesAsync("test");
            return new TypesResponse<CatalogTypeDto>()
            {
                Data = result.Data.Select(s => _mapper.Map<CatalogTypeDto>(s)).ToList(),
                TotalCount = result.TotalCount,
            };
        });
    }
}