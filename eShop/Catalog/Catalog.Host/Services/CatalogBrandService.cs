using Catalog.Host.Data;
using Catalog.Host.Repositories.Interfaces;
using Catalog.Host.Services.Interfaces;

namespace Catalog.Host.Services
{
    public class CatalogBrandService : BaseDataService<ApplicationDbContext>, ICatalogBrandService
    {
        private readonly ICatalogBrandRepository _catalogBrandRepository;

        public CatalogBrandService (
            IDbContextWrapper<ApplicationDbContext> dbContextWrapper,
            ILogger<BaseDataService<ApplicationDbContext>> logger,
            ICatalogBrandRepository catalogBrandRepository)
            : base(dbContextWrapper, logger)
        {
            _catalogBrandRepository = catalogBrandRepository;
        }

        public Task<int?> Add(string name)
        {
            return ExecuteSafeAsync(() => _catalogBrandRepository.Add(name));
        }

        public Task<int?> Remove(int id)
        {
            return ExecuteSafeAsync(() => _catalogBrandRepository.Remove(id));
        }

        public Task<int?> Update(int id, string name)
        {
            return ExecuteSafeAsync(() => _catalogBrandRepository.Update(id, name));
        }
    }
}
