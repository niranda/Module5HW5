namespace MVC.ViewModels;

    public record Brands
    {
        public List<CatalogBrand> Data { get; set; } = null!;
        public long TotalCount { get; init; }
    }
