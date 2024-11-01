using Moq;
using Xunit;
using SeoChecker.Interfaces;
using SeoChecker.Services;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SeoChecker.Tests.Services
{
    public class CachedSearchServiceTests
    {
        private readonly CachedSearchService _cachedSearchService;
        private readonly Mock<ISearchEngine> _searchEngineMock;
        private readonly Mock<IDataRepository> _dataRepositoryMock;

        public CachedSearchServiceTests()
        {
            _searchEngineMock = new Mock<ISearchEngine>();
            _dataRepositoryMock = new Mock<IDataRepository>();
            _cachedSearchService = new CachedSearchService(_searchEngineMock.Object, _dataRepositoryMock.Object);
        }

        [Fact]
        public async Task SearchAsync_ReturnsCachedResult_WhenCacheExists()
        {
            // Arrange
            var keywords = "test keywords";
            var url = "http://testurl.com";
            var cachedPositions = new List<int> { 2, 8, 15 };
            _dataRepositoryMock.Setup(repo => repo.GetCachedResult(keywords, url))
                               .Returns(cachedPositions);

            // Act
            var result = await _cachedSearchService.SearchAsync(keywords, url);

            // Assert
            Assert.Equal(cachedPositions, result);
            _searchEngineMock.Verify(engine => engine.SearchAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>()), Times.Never);
        }

        [Fact]
        public async Task SearchAsync_FetchesFreshResults_WhenForceRefreshIsTrue()
        {
            // Arrange
            var keywords = "test keywords";
            var url = "http://testurl.com";
            var freshPositions = new List<int> { 1, 3, 6 };
            _dataRepositoryMock.Setup(repo => repo.GetCachedResult(keywords, url)).Returns((List<int>)null);
            _searchEngineMock.Setup(engine => engine.SearchAsync(keywords, url, true)).ReturnsAsync(freshPositions);

            // Act
            var result = await _cachedSearchService.SearchAsync(keywords, url, true);

            // Assert
            Assert.Equal(freshPositions, result);
            _dataRepositoryMock.Verify(repo => repo.SaveSearchResult(keywords, url, freshPositions), Times.Once);
        }
    }
}
