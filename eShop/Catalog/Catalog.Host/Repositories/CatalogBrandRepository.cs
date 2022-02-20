using Catalog.Host.Data;
using Catalog.Host.Data.Entities;
using Catalog.Host.Repositories.Interfaces;
using Catalog.Host.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Catalog.Host.Repositories;

public class CatalogBrandRepository : ICatalogBrandRepository
{
    private readonly ApplicationDbContext _dbContext;
    private readonly ILogger<CatalogBrandRepository> _logger;

    public CatalogBrandRepository (
        IDbContextWrapper<ApplicationDbContext> dbContextWrapper,
        ILogger<CatalogBrandRepository> logger)
    {
        _dbContext = dbContextWrapper.DbContext;
        _logger = logger;
    }

    public async Task<int?> Add(string name)
    {
        var item = await _dbContext.AddAsync(new CatalogBrand
        {
            Brand = name
        });
        await _dbContext.SaveChangesAsync();

        return item.Entity.Id;
    }

    public async Task<int?> Update(int id, string name)
    {
        var item = await _dbContext.CatalogBrands
            .FirstOrDefaultAsync(x => x.Id == id);

        if (item != null)
        {
            item.Brand = name;

            _dbContext.CatalogBrands.Update(item);
        }

        await _dbContext.SaveChangesAsync();

        return item?.Id;
    }

    public async Task<int?> Remove(int id)
    {
        var item = await _dbContext.CatalogBrands
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