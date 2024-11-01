using SeoChecker.Repositories;
using System.Collections.Generic;
using Xunit;

namespace SeoChecker.Tests.Repositories
{
    public class InMemoryDataRepositoryTests
    {
        [Fact]
        public void SaveSearchResult_StoresDataCorrectly()
        {
            // Arrange
            var repository = new InMemoryDataRepository();
            var keywords = "test keywords";
            var url = "http://testurl.com";
            var positions = new List<int> { 1, 10, 25 };

            // Act
            repository.SaveSearchResult(keywords, url, positions);
            var cachedResult = repository.GetCachedResult(keywords, url);

            // Assert
            Assert.Equal(positions, cachedResult);
        }

        [Fact]
        public void GetCachedResult_ReturnsNull_WhenNoDataExists()
        {
            // Arrange
            var repository = new InMemoryDataRepository();

            // Act
            var result = repository.GetCachedResult("non-existent keywords", "http://nonexistenturl.com");

            // Assert
            Assert.Null(result);
        }
    }
}
