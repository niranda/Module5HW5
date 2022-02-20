namespace Catalog.Host.Services.Interfaces
{
    public interface ICatalogTypeService
    {
        Task<int?> Update(int id, string name);
        Task<int?> Remove(int id);
        Task<int?> Add(string name);
    }
}
