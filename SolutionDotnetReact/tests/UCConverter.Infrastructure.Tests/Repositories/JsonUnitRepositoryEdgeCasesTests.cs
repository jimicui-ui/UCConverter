namespace UCConverter.Infrastructure.Tests.Repositories;

using Microsoft.Extensions.Logging;
using Moq;
using UCConverter.Infrastructure.Repositories;
using Xunit;

public class JsonUnitRepositoryEdgeCasesTests : IDisposable
{
    private readonly string _testUnitsSettingsPath;
    private readonly Mock<ILogger<JsonUnitRepository>> _mockLogger;

    public JsonUnitRepositoryEdgeCasesTests()
    {
        _testUnitsSettingsPath = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
        Directory.CreateDirectory(_testUnitsSettingsPath);
        _mockLogger = new Mock<ILogger<JsonUnitRepository>>();
    }

    [Fact]
    public void Initialize_WhenNoJsonFiles_DoesNotThrow()
    {
        // Arrange
        var repository = new JsonUnitRepository(_testUnitsSettingsPath, _mockLogger.Object);

        // Act & Assert - Should not throw
        repository.Initialize();
    }

    [Fact]
    public async Task GetAllCategoriesAsync_WhenNotInitialized_InitializesAndReturnsEmpty()
    {
        // Arrange
        var repository = new JsonUnitRepository(_testUnitsSettingsPath, _mockLogger.Object);

        // Act
        var categories = await repository.GetAllCategoriesAsync();

        // Assert
        Assert.NotNull(categories);
        Assert.Empty(categories);
    }

    [Fact]
    public async Task GetCategoryByNameAsync_WhenNotInitialized_InitializesAndReturnsNull()
    {
        // Arrange
        var repository = new JsonUnitRepository(_testUnitsSettingsPath, _mockLogger.Object);

        // Act
        var category = await repository.GetCategoryByNameAsync("test");

        // Assert
        Assert.Null(category);
    }

    [Fact]
    public async Task GetUnitsByCategoryAsync_WhenNotInitialized_InitializesAndReturnsEmpty()
    {
        // Arrange
        var repository = new JsonUnitRepository(_testUnitsSettingsPath, _mockLogger.Object);

        // Act
        var units = await repository.GetUnitsByCategoryAsync("test");

        // Assert
        Assert.NotNull(units);
        Assert.Empty(units);
    }

    [Fact]
    public async Task GetUnitBySymbolAsync_WhenNotInitialized_InitializesAndReturnsNull()
    {
        // Arrange
        var repository = new JsonUnitRepository(_testUnitsSettingsPath, _mockLogger.Object);

        // Act
        var unit = await repository.GetUnitBySymbolAsync("test", "t");

        // Assert
        Assert.Null(unit);
    }

    [Fact]
    public async Task GetAllUnitsAsync_WhenNotInitialized_InitializesAndReturnsEmpty()
    {
        // Arrange
        var repository = new JsonUnitRepository(_testUnitsSettingsPath, _mockLogger.Object);

        // Act
        var units = await repository.GetAllUnitsAsync();

        // Assert
        Assert.NotNull(units);
        Assert.Empty(units);
    }

    [Fact]
    public async Task Initialize_WhenJsonFileHasNullBaseUnit_HandlesGracefully()
    {
        // Arrange
        var jsonContent = @"{
  ""category"": ""test"",
  ""categoryDisplayName"": ""Test Category"",
  ""baseUnit"": null,
  ""units"": []
}";
        var jsonFile = Path.Combine(_testUnitsSettingsPath, "test.json");
        File.WriteAllText(jsonFile, jsonContent);

        var repository = new JsonUnitRepository(_testUnitsSettingsPath, _mockLogger.Object);

        // Act - Should not throw, but log error
        repository.Initialize();

        // Assert
        var categories = await repository.GetAllCategoriesAsync();
        // Should either have no categories or handle null gracefully
    }

    [Fact]
    public async Task Initialize_WhenJsonFileHasEmptyUnitsArray_HandlesGracefully()
    {
        // Arrange
        var jsonContent = @"{
  ""category"": ""test"",
  ""categoryDisplayName"": ""Test Category"",
  ""baseUnit"": {
    ""symbol"": ""t"",
    ""name"": ""test"",
    ""displayName"": ""Test"",
    ""isBaseUnit"": true,
    ""isSIUnit"": true,
    ""unitSystem"": ""SI""
  },
  ""units"": []
}";
        var jsonFile = Path.Combine(_testUnitsSettingsPath, "test.json");
        File.WriteAllText(jsonFile, jsonContent);

        var repository = new JsonUnitRepository(_testUnitsSettingsPath, _mockLogger.Object);

        // Act
        repository.Initialize();

        // Assert
        var category = await repository.GetCategoryByNameAsync("test");
        Assert.NotNull(category);
        // Base unit should be added to units list
        Assert.Single(category!.Units);
    }

    public void Dispose()
    {
        if (Directory.Exists(_testUnitsSettingsPath))
        {
            Directory.Delete(_testUnitsSettingsPath, true);
        }
    }
}

