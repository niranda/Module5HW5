namespace Catalog.Host.Repositories.Interfaces
{
    public interface ICatalogTypeRepository
    {
        Task<int?> Add(string name);
        Task<int?> Update(int id, string name);
        Task<int?> Remove(int id);
    }
}
