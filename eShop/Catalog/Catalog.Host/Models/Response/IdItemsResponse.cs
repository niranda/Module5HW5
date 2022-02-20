namespace Catalog.Host.Models.Response
{
    public class IdItemsResponse<T>
    {
        public int Id { get; init; }

        public T Data { get; init; } = default(T) !;
    }
}
