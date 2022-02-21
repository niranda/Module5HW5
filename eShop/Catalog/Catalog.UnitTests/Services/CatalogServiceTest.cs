using System.Threading;
using Catalog.Host.Data.Entities;
using Catalog.Host.Models.Dtos;
using Catalog.Host.Models.Response;

namespace Catalog.UnitTests.Services;

public class CatalogServiceTest
{
    private readonly ICatalogService _catalogService;

    private readonly Mock<ICatalogItemRepository> _catalogItemRepository;
    private readonly Mock<IMapper> _mapper;
    private readonly Mock<IDbContextWrapper<ApplicationDbContext>> _dbContextWrapper;
    private readonly Mock<ILogger<CatalogService>> _logger;

    public CatalogServiceTest()
    {
        _catalogItemRepository = new Mock<ICatalogItemRepository>();
        _mapper = new Mock<IMapper>();
        _dbContextWrapper = new Mock<IDbContextWrapper<ApplicationDbContext>>();
        _logger = new Mock<ILogger<CatalogService>>();

        var dbContextTransaction = new Mock<IDbContextTransaction>();
        _dbContextWrapper.Setup(s => s.BeginTransactionAsync(CancellationToken.None)).ReturnsAsync(dbContextTransaction.Object);

        _catalogService = new CatalogService(_dbContextWrapper.Object, _logger.Object, _catalogItemRepository.Object, _mapper.Object);
    }

    [Fact]
    public async Task GetCatalogItemsAsync_Success()
    {
        // arrange
        var testPageIndex = 0;
        var testPageSize = 4;
        var testTotalCount = 12;

        var pagingPaginatedItemsSuccess = new PaginatedItems<CatalogItem>()
        {
            Data = new List<CatalogItem>()
            {
                new CatalogItem()
                {
                    Name = "TestName",
                },
            },
            TotalCount = testTotalCount,
        };

        var catalogItemSuccess = new CatalogItem()
        {
            Name = "TestName"
        };

        var catalogItemDtoSuccess = new CatalogItemDto()
        {
            Name = "TestName"
        };

        _catalogItemRepository.Setup(s => s.GetByPageAsync(
            It.Is<int>(i => i == testPageIndex),
            It.Is<int>(i => i == testPageSize),
            It.IsAny<int?>(),
            It.IsAny<int?>())).ReturnsAsync(pagingPaginatedItemsSuccess);

        _mapper.Setup(s => s.Map<CatalogItemDto>(
            It.Is<CatalogItem>(i => i.Equals(catalogItemSuccess)))).Returns(catalogItemDtoSuccess);

        // act
        var result = await _catalogService.GetCatalogItemsAsync(testPageSize, testPageIndex, null);

        // assert
        result.Should().NotBeNull();
        result?.Data.Should().NotBeNull();
        result?.Count.Should().Be(testTotalCount);
        result?.PageIndex.Should().Be(testPageIndex);
        result?.PageSize.Should().Be(testPageSize);
    }

    [Fact]
    public async Task GetCatalogItemsAsync_Failed()
    {
        // arrange
        var testPageIndex = 1000;
        var testPageSize = 10000;

        _catalogItemRepository.Setup(s => s.GetByPageAsync(
            It.Is<int>(i => i == testPageIndex),
            It.Is<int>(i => i == testPageSize),
            It.IsAny<int?>(),
            It.IsAny<int?>())).Returns((Func<PaginatedItemsResponse<CatalogItemDto>>)null!);

        // act
        var result = await _catalogService.GetCatalogItemsAsync(testPageSize, testPageIndex, null);

        // assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task GetByIdAsync_Success()
    {
        // arrange
        var testId = 1;
        var testTotalCount = 1;

        var idItemsSuccess = new IdItems<CatalogItem>()
        {
            Data = new CatalogItem()
            {
                Name = "TestName",
                Id = testId
            },
            TotalCount = testTotalCount,
            Id = testId
        };

        var catalogItemSuccess = new CatalogItem()
        {
            Name = "TestName",
            Id = 1
        };

        var catalogItemDtoSuccess = new CatalogItemDto()
        {
            Name = "TestName",
            Id = 1
        };

        _catalogItemRepository.Setup(s => s.GetByIdAsync(
            It.Is<int>(i => i == testId))).ReturnsAsync(idItemsSuccess);

        _mapper.Setup(s => s.Map<CatalogItemDto>(
            It.Is<CatalogItem>(i => i.Equals(catalogItemSuccess)))).Returns(catalogItemDtoSuccess);

        // act
        var result = await _catalogService.GetByIdAsync(testId);

        // assert
        result.Should().NotBeNull();
        result?.Id.Should().Be(testId);
    }

    [Fact]
    public async Task GetByIdAsync_Failed()
    {
        // arrange
        var testId = 1000;

        _catalogItemRepository.Setup(s => s.GetByIdAsync(
            It.Is<int>(i => i == testId))).Returns((Func<IdItemsResponse<CatalogItemDto>>)null!);

        // act
        var result = await _catalogService.GetByIdAsync(testId);

        // assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task GetByBrandAsync_Success()
    {
        // arrange
        var testBrand = new CatalogBrand()
        {
            Brand = "testBrand",
        };
        var testTotalCount = 2;

        var brandItemsSuccess = new BrandedItems<CatalogItem>()
        {
            Data = new List<CatalogItem>()
            {
                new CatalogItem()
                {
                    Name = "TestName",
                },
            },
            Brand = testBrand,
            TotalCount = testTotalCount
        };

        var catalogBrandSuccess = new CatalogBrand()
        {
            Brand = "TestBrand"
        };

        var catalogBrandDtoSuccess = new CatalogBrandDto()
        {
            Brand = "TestBrand"
        };

        _catalogItemRepository.Setup(s => s.GetByBrandAsync(
            It.Is<CatalogBrand>(i => i == testBrand))).ReturnsAsync(brandItemsSuccess);

        _mapper.Setup(s => s.Map<CatalogBrandDto>(
            It.Is<CatalogBrand>(i => i.Equals(catalogBrandSuccess)))).Returns(catalogBrandDtoSuccess);

        // act
        var result = await _catalogService.GetByBrandAsync(testBrand);

        // assert
        result.Should().NotBeNull();
        result?.Data.Should().NotBeNull();
        result?.TotalCount.Should().Be(testTotalCount);
    }

    [Fact]
    public async Task GetByBrandAsync_Failed()
    {
        // arrange
        var testBrand = new CatalogBrand()
        {
            Brand = "testBrand",
        };

        _catalogItemRepository.Setup(s => s.GetByBrandAsync(
            It.Is<CatalogBrand>(i => i == testBrand))).Returns((Func<BrandedItemsResponse<CatalogItemDto>>)null!);

        // act
        var result = await _catalogService.GetByBrandAsync(testBrand);

        // assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task GetByTypeAsync_Success()
    {
        // arrange
        var testType = new CatalogType()
        {
            Type = "testType",
        };
        var testTotalCount = 2;

        var typeItemsSuccess = new TypedItems<CatalogItem>()
        {
            Data = new List<CatalogItem>()
            {
                new CatalogItem()
                {
                    Name = "TestName",
                },
            },
            Type = testType,
            TotalCount = testTotalCount
        };

        var catalogTypeSuccess = new CatalogType()
        {
            Type = "TestType"
        };

        var catalogTypeDtoSuccess = new CatalogTypeDto()
        {
            Type = "TestType"
        };

        _catalogItemRepository.Setup(s => s.GetByTypeAsync(
            It.Is<CatalogType>(i => i == testType))).ReturnsAsync(typeItemsSuccess);

        _mapper.Setup(s => s.Map<CatalogTypeDto>(
            It.Is<CatalogType>(i => i.Equals(catalogTypeSuccess)))).Returns(catalogTypeDtoSuccess);

        // act
        var result = await _catalogService.GetByTypeAsync(testType);

        // assert
        result.Should().NotBeNull();
        result?.Data.Should().NotBeNull();
        result?.TotalCount.Should().Be(testTotalCount);
    }

    [Fact]
    public async Task GetByTypeAsync_Failed()
    {
        // arrange
        var testType = new CatalogType()
        {
            Type = "testType",
        };

        _catalogItemRepository.Setup(s => s.GetByTypeAsync(
            It.Is<CatalogType>(i => i == testType))).Returns((Func<TypedItemsResponse<CatalogItemDto>>)null!);

        // act
        var result = await _catalogService.GetByTypeAsync(testType);

        // assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task GetBrandsAsync_Success()
    {
        // arrange
        var testTotalCount = 2;
        var testMessage = "test";

        var brandsSuccess = new Brands<CatalogBrand>()
        {
            Data = new List<CatalogBrand>()
            {
                new CatalogBrand()
                {
                    Brand = "TestName",
                },
            },
            TotalCount = testTotalCount
        };

        var brandSuccess = new CatalogBrand()
        {
            Brand = "TestBrand"
        };

        var brandDtoSuccess = new CatalogBrandDto()
        {
            Brand = "TestBrand"
        };

        _catalogItemRepository.Setup(s => s.GetBrandsAsync(
            It.Is<string>(i => i == testMessage))).ReturnsAsync(brandsSuccess);

        _mapper.Setup(s => s.Map<CatalogBrandDto>(
            It.Is<CatalogBrand>(i => i.Equals(brandSuccess)))).Returns(brandDtoSuccess);

        // act
        var result = await _catalogService.GetBrandsAsync();

        // assert
        result.Should().NotBeNull();
        result?.Data.Should().NotBeNull();
        result?.TotalCount.Should().Be(testTotalCount);
    }

    [Fact]
    public async Task GetBrandsAsync_Failed()
    {
        // arrange
        var testMessage = "Test msg";

        _catalogItemRepository.Setup(s => s.GetBrandsAsync(
            It.Is<string>(i => i == testMessage))).Returns((Func<BrandsResponse<CatalogItemDto>>)null!);

        // act
        var result = await _catalogService.GetBrandsAsync();

        // assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task GetTypesAsync_Success()
    {
        // arrange
        var testTotalCount = 2;
        var testMessage = "test";

        var typesSuccess = new Types<CatalogType>()
        {
            Data = new List<CatalogType>()
            {
                new CatalogType()
                {
                    Type = "TestName",
                },
            },
            TotalCount = testTotalCount
        };

        var typeSuccess = new CatalogType()
        {
            Type = "TestName"
        };

        var typeDtoSuccess = new CatalogTypeDto()
        {
            Type = "TestName"
        };

        _catalogItemRepository.Setup(s => s.GetTypesAsync(
            It.Is<string>(i => i == testMessage))).ReturnsAsync(typesSuccess);

        _mapper.Setup(s => s.Map<CatalogTypeDto>(
            It.Is<CatalogType>(i => i.Equals(typeSuccess)))).Returns(typeDtoSuccess);

        // act
        var result = await _catalogService.GetTypesAsync();

        // assert
        result.Should().NotBeNull();
        result?.Data.Should().NotBeNull();
        result?.TotalCount.Should().Be(testTotalCount);
    }

    [Fact]
    public async Task GetTypesAsync_Failed()
    {
        // arrange
        var testMessage = "Test msg";

        _catalogItemRepository.Setup(s => s.GetTypesAsync(
            It.Is<string>(i => i == testMessage))).Returns((Func<TypesResponse<CatalogItemDto>>)null!);

        // act
        var result = await _catalogService.GetTypesAsync();

        // assert
        result.Should().BeNull();
    }
}