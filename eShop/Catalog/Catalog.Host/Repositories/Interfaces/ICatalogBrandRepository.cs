using Catalog.Host.Data;
using Catalog.Host.Data.Entities;

namespace Catalog.Host.Repositories.Interfaces
{
    public interface ICatalogBrandRepository
    {
        Task<int?> Add(string name);
        Task<int?> Remove(int id);
        Task<int?> Update(int id, string name);
    }
}
