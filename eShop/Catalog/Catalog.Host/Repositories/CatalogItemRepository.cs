using Catalog.Host.Data;
using Catalog.Host.Data.Entities;
using Catalog.Host.Repositories.Interfaces;

namespace Catalog.Host.Repositories;

public class CatalogItemRepository : ICatalogItemRepository
{
    private readonly ApplicationDbContext _dbContext;
    private readonly ILogger<CatalogItemRepository> _logger;

    public CatalogItemRepository(
        IDbContextWrapper<ApplicationDbContext> dbContextWrapper,
        ILogger<CatalogItemRepository> logger)
    {
        _dbContext = dbContextWrapper.DbContext;
        _logger = logger;
    }

    public async Task<PaginatedItems<CatalogItem>> GetByPageAsync(int pageIndex, int pageSize, int? brandFilter, int? typeFilter)
    {
        IQueryable<CatalogItem> query = _dbContext.CatalogItems;

        if (brandFilter.HasValue)
        {
            query = query.Where(w => w.CatalogBrandId == brandFilter.Value);
        }

        if (typeFilter.HasValue)
        {
            query = query.Where(w => w.CatalogTypeId == typeFilter.Value);
        }

        var totalItems = await query.LongCountAsync();

        var itemsOnPage = await query.OrderBy(c => c.Name)
           .Include(i => i.CatalogBrand)
           .Include(i => i.CatalogType)
           .Skip(pageSize * pageIndex)
           .Take(pageSize)
           .ToListAsync();

        return new PaginatedItems<CatalogItem>() { TotalCount = totalItems, Data = itemsOnPage };
    }

    public async Task<IdItems<CatalogItem>> GetByIdAsync(int id)
    {
        var totalItems = await _dbContext.CatalogItems
            .LongCountAsync();

        var itemsOnPage = await _dbContext.CatalogItems
            .Include(i => i.CatalogBrand)
            .Include(i => i.CatalogType)
            .Where(c => c.Id == id)
            .FirstOrDefaultAsync();

        var indetificator = itemsOnPage!.Id;

        return new IdItems<CatalogItem>() { TotalCount = totalItems, Data = itemsOnPage, Id = indetificator };
    }

    public async Task<BrandedItems<CatalogItem>> GetByBrandAsync(CatalogBrand brand)
    {
        var itemsOnPage = await _dbContext.CatalogItems
            .Include(i => i.CatalogBrand)
            .Include(i => i.CatalogType)
            .Where(c => c.CatalogBrand == brand)
            .ToListAsync();

        var totalItems = itemsOnPage
            .LongCount();

        return new BrandedItems<CatalogItem>() { TotalCount = totalItems, Data = itemsOnPage, Brand = brand };
    }

    public async Task<TypedItems<CatalogItem>> GetByTypeAsync(CatalogType type)
    {
        var itemsOnPage = await _dbContext.CatalogItems
            .Include(i => i.CatalogBrand)
            .Include(i => i.CatalogType)
            .Where(c => c.CatalogType == type)
            .ToListAsync();

        var totalItems = itemsOnPage
            .LongCount();

        return new TypedItems<CatalogItem>() { TotalCount = totalItems, Data = itemsOnPage, Type = type };
    }

    public async Task<Brands<CatalogBrand>> GetBrandsAsync(string brandsQueryMessage)
    {
        var itemsOnPage = await _dbContext.CatalogBrands
            .ToListAsync();

        var totalItems = itemsOnPage
            .LongCount();

        return new Brands<CatalogBrand>() { TotalCount = totalItems, Data = itemsOnPage };
    }

    public async Task<Types<CatalogType>> GetTypesAsync(string typeQueryMessage)
    {
        var itemsOnPage = await _dbContext.CatalogTypes
            .ToListAsync();

        var totalItems = itemsOnPage
            .LongCount();

        return new Types<CatalogType>() { TotalCount = totalItems, Data = itemsOnPage };
    }

    public async Task<int?> Add(string name, string description, decimal price, int availableStock, int catalogBrandId, int catalogTypeId, string pictureFileName)
    {
        var item1 = new CatalogItem
        {
            CatalogBrandId = catalogBrandId,
            CatalogTypeId = catalogTypeId,
            Description = description,
            Name = name,
            PictureFileName = pictureFileName,
            Price = price
        };
        var item = await _dbContext.AddAsync(item1);

        await _dbContext.SaveChangesAsync();

        return item.Entity.Id;
    }

    public async Task<int?> Update(int id, string name, string description, decimal price, int availableStock, int catalogBrandId, int catalogTypeId, string pictureFileName)
    {
        var item = await _dbContext.CatalogItems
            .FirstOrDefaultAsync(x => x.Id == id);

        if (item != null)
        {
            item.CatalogBrandId = catalogBrandId;
            item.Description = description;
            item.AvailableStock = availableStock;
            item.CatalogTypeId = catalogTypeId;
            item.PictureFileName = pictureFileName;
            item.Name = name;
            item.Price = price;
            item.CatalogBrandId = catalogBrandId;

            _dbContext.CatalogItems.Update(item);
        }

        await _dbContext.SaveChangesAsync();

        return item?.Id;
    }

    public async Task<int?> Remove(int id)
    {
        var item = await _dbContext.CatalogItems
            .FirstOrDefaultAsync(x => x.Id == id);

        if (item != null)
        {
            _dbContext.Remove(item);
        }

        await _dbContext.SaveChangesAsync();
        _logger.LogInformation("Deleted successfully");
        return id;
    }
}