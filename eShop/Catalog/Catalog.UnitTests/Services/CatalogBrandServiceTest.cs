using System.Threading;
using Catalog.Host.Data.Entities;

namespace Catalog.UnitTests.Services
{
    public class CatalogBrandServiceTest
    {
        private readonly ICatalogBrandService _catalogService;

        private readonly Mock<ICatalogBrandRepository> _catalogBrandRepository;
        private readonly Mock<IDbContextWrapper<ApplicationDbContext>> _dbContextWrapper;
        private readonly Mock<ILogger<CatalogService>> _logger;

        private readonly CatalogBrand _testBrand = new CatalogBrand()
        {
            Id = 1,
            Brand = "Name"
        };

        public CatalogBrandServiceTest()
        {
            _catalogBrandRepository = new Mock<ICatalogBrandRepository>();
            _dbContextWrapper = new Mock<IDbContextWrapper<ApplicationDbContext>>();
            _logger = new Mock<ILogger<CatalogService>>();

            var dbContextTransaction = new Mock<IDbContextTransaction>();
            _dbContextWrapper.Setup(s => s.BeginTransactionAsync(CancellationToken.None)).ReturnsAsync(dbContextTransaction.Object);

            _catalogService = new CatalogBrandService(_dbContextWrapper.Object, _logger.Object, _catalogBrandRepository.Object);
        }

        [Fact]
        public async Task Add_Success()
        {
            // arrange
            var testResult = 1;

            _catalogBrandRepository.Setup(s => s.Add(
                It.IsAny<string>())).ReturnsAsync(testResult);

            // act
            var result = await _catalogService.Add(_testBrand.Brand);

            // assert
            result.Should().Be(testResult);
        }

        [Fact]
        public async Task Add_Failed()
        {
            // arrange
            int? testResult = null;

            _catalogBrandRepository.Setup(s => s.Add(
                It.IsAny<string>())).ReturnsAsync(testResult);

            // act
            var result = await _catalogService.Add(_testBrand.Brand);

            // assert
            result.Should().Be(testResult);
        }

        [Fact]
        public async Task Update_Success()
        {
            // arrange
            var testResult = 1;

            _catalogBrandRepository.Setup(s => s.Update(
                It.IsAny<int>(),
                It.IsAny<string>())).ReturnsAsync(testResult);

            // act
            var result = await _catalogService.Update(_testBrand.Id, _testBrand.Brand);

            // assert
            result.Should().Be(testResult);
        }

        [Fact]
        public async Task Update_Failed()
        {
            // arrange
            int? testResult = null;

            _catalogBrandRepository.Setup(s => s.Update(
                It.IsAny<int>(),
                It.IsAny<string>())).ReturnsAsync(testResult);

            // act
            var result = await _catalogService.Update(_testBrand.Id, _testBrand.Brand);

            // assert
            result.Should().Be(testResult);
        }

        [Fact]
        public async Task Remove_Success()
        {
            // arrange
            var testResult = 1;

            _catalogBrandRepository.Setup(s => s.Remove(
                It.IsAny<int>())).ReturnsAsync(testResult);

            // act
            var result = await _catalogService.Remove(_testBrand.Id);

            // assert
            result.Should().Be(testResult);
        }

        [Fact]
        public async Task Remove_Failed()
        {
            // arrange
            int? testResult = null;

            _catalogBrandRepository.Setup(s => s.Remove(
                It.IsAny<int>())).ReturnsAsync(testResult);

            // act
            var result = await _catalogService.Remove(_testBrand.Id);

            // assert
            result.Should().Be(testResult);
        }
    }
}
