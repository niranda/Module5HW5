namespace MVC.ViewModels
{
    public record Types
    {
        public List<CatalogType> Data { get; set; } = null!;
        public long TotalCount { get; init; }
    }
}
