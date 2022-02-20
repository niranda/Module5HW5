using MVC.Dtos;
using MVC.Models.Enums;
using MVC.Services.Interfaces;
using MVC.ViewModels;

namespace MVC.Services;

public class CatalogService : ICatalogService
{
    private readonly IOptions<AppSettings> _settings;
    private readonly IHttpClientService _httpClient;
    private readonly ILogger<CatalogService> _logger;

    public CatalogService(IHttpClientService httpClient, ILogger<CatalogService> logger, IOptions<AppSettings> settings)
    {
        _httpClient = httpClient;
        _settings = settings;
        _logger = logger;
    }

    public async Task<Catalog> GetCatalogItems(int page, int take, int? brand, int? type)
    {
        var filters = new Dictionary<CatalogTypeFilter, int>();

        if (brand.HasValue)
        {
            filters.Add(CatalogTypeFilter.Brand, brand.Value);
        }

        if (type.HasValue)
        {
            filters.Add(CatalogTypeFilter.Type, type.Value);
        }

        var result = await _httpClient.SendAsync<Catalog, PaginatedItemsRequest<CatalogTypeFilter>>(
            $"{_settings.Value.CatalogUrl}/items",
            HttpMethod.Post,
            new PaginatedItemsRequest<CatalogTypeFilter>()
            {
                PageIndex = page,
                PageSize = take,
                Filters = filters
            });

        return result;
    }

    public async Task<IEnumerable<SelectListItem>> GetBrands()
    {
        await Task.Delay(300);
        var result = await _httpClient.SendAsync<Brands, BrandsRequest>(
            $"{_settings.Value.CatalogUrl}/getBrands",
            HttpMethod.Post,
            new BrandsRequest()
            {
                BrandQueryMessage = "test"
            });

        var list = new List<SelectListItem>();

        for (var i = 0; i < result.Data.Count; i++)
        {
            list.Add(
                new SelectListItem()
                {
                    Value = result.Data[i].Id.ToString(),
                    Text = result.Data[i].Brand
                });
        }

        return list;
    }

    public async Task<IEnumerable<SelectListItem>> GetTypes()
    {
        await Task.Delay(300);

        var result = await _httpClient.SendAsync<Types, TypesRequest>(
            $"{_settings.Value.CatalogUrl}/getTypes",
            HttpMethod.Post,
            new TypesRequest()
            {
                TypeQueryMessage = "test"
            });

        var list = new List<SelectListItem>();

        for (var i = 0; i < result.Data.Count; i++)
        {
            list.Add(
                new SelectListItem()
                {
                    Value = result.Data[i].Id.ToString(),
                    Text = result.Data[i].Type
                });
        }

        return list;
    }
}
