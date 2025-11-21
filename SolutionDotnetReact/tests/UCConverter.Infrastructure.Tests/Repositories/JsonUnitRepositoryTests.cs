namespace UCConverter.Infrastructure.Tests.Repositories;

using Microsoft.Extensions.Logging;
using Moq;
using System.Text.Json;
using UCConverter.Domain.Entities;
using UCConverter.Infrastructure.Repositories;
using Xunit;

public class JsonUnitRepositoryTests : IDisposable
{
    private readonly string _testUnitsSettingsPath;
    private readonly Mock<ILogger<JsonUnitRepository>> _mockLogger;

    public JsonUnitRepositoryTests()
    {
        _testUnitsSettingsPath = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
        Directory.CreateDirectory(_testUnitsSettingsPath);
        _mockLogger = new Mock<ILogger<JsonUnitRepository>>();
    }

    [Fact]
    public void Initialize_WhenDirectoryDoesNotExist_LogsWarning()
    {
        // Arrange
        var nonExistentPath = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
        var repository = new JsonUnitRepository(nonExistentPath, _mockLogger.Object);

        // Act
        repository.Initialize();

        // Assert
        // Verify warning was logged
        _mockLogger.Verify(
            x => x.Log(
                LogLevel.Warning,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => v.ToString()!.Contains("UnitsSettings directory not found")),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
            Times.Once);
    }

    [Fact]
    public async Task Initialize_WhenValidJsonFile_LoadsCategory()
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
  ""units"": [
    {
      ""symbol"": ""t"",
      ""name"": ""test"",
      ""displayName"": ""Test"",
      ""category"": ""test"",
      ""isBaseUnit"": true,
      ""isSIUnit"": true,
      ""unitSystem"": ""SI"",
      ""conversionFactor"": 1.0,
      ""conversionFormula"": null
    }
  ]
}";
        var jsonFile = Path.Combine(_testUnitsSettingsPath, "test.json");
        File.WriteAllText(jsonFile, jsonContent);

        var repository = new JsonUnitRepository(_testUnitsSettingsPath, _mockLogger.Object);

        // Act
        repository.Initialize();

        // Assert
        var categories = await repository.GetAllCategoriesAsync();
        Assert.Single(categories);
        var category = categories.First();
        Assert.Equal("test", category.Name);
        Assert.Equal("Test Category", category.DisplayName);
    }

    [Fact]
    public void Initialize_WhenInvalidJsonFile_LogsErrorAndContinues()
    {
        // Arrange
        var invalidJson = "{ invalid json }";
        var jsonFile = Path.Combine(_testUnitsSettingsPath, "invalid.json");
        File.WriteAllText(jsonFile, invalidJson);

        var repository = new JsonUnitRepository(_testUnitsSettingsPath, _mockLogger.Object);

        // Act
        repository.Initialize();

        // Assert
        // Should not throw, but log error
        _mockLogger.Verify(
            x => x.Log(
                LogLevel.Error,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => v.ToString()!.Contains("Failed to load")),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
            Times.Once);
    }

    [Fact]
    public async Task GetAllCategoriesAsync_WhenInitialized_ReturnsCategories()
    {
        // Arrange
        CreateTestJsonFile("test.json", "test", "Test Category");
        var repository = new JsonUnitRepository(_testUnitsSettingsPath, _mockLogger.Object);
        repository.Initialize();

        // Act
        var categories = await repository.GetAllCategoriesAsync();

        // Assert
        Assert.NotNull(categories);
        Assert.Single(categories);
    }

    [Fact]
    public async Task GetCategoryByNameAsync_WhenExists_ReturnsCategory()
    {
        // Arrange
        CreateTestJsonFile("test.json", "test", "Test Category");
        var repository = new JsonUnitRepository(_testUnitsSettingsPath, _mockLogger.Object);
        repository.Initialize();

        // Act
        var category = await repository.GetCategoryByNameAsync("test");

        // Assert
        Assert.NotNull(category);
        Assert.Equal("test", category.Name);
    }

    [Fact]
    public async Task GetCategoryByNameAsync_WhenNotExists_ReturnsNull()
    {
        // Arrange
        CreateTestJsonFile("test.json", "test", "Test Category");
        var repository = new JsonUnitRepository(_testUnitsSettingsPath, _mockLogger.Object);
        repository.Initialize();

        // Act
        var category = await repository.GetCategoryByNameAsync("nonexistent");

        // Assert
        Assert.Null(category);
    }

    [Fact]
    public async Task GetUnitsByCategoryAsync_WhenExists_ReturnsUnits()
    {
        // Arrange
        CreateTestJsonFile("test.json", "test", "Test Category");
        var repository = new JsonUnitRepository(_testUnitsSettingsPath, _mockLogger.Object);
        repository.Initialize();

        // Act
        var units = await repository.GetUnitsByCategoryAsync("test");

        // Assert
        Assert.NotNull(units);
        Assert.Single(units);
    }

    [Fact]
    public async Task GetUnitsByCategoryAsync_WhenNotExists_ReturnsEmpty()
    {
        // Arrange
        CreateTestJsonFile("test.json", "test", "Test Category");
        var repository = new JsonUnitRepository(_testUnitsSettingsPath, _mockLogger.Object);
        repository.Initialize();

        // Act
        var units = await repository.GetUnitsByCategoryAsync("nonexistent");

        // Assert
        Assert.NotNull(units);
        Assert.Empty(units);
    }

    [Fact]
    public async Task GetUnitBySymbolAsync_WhenExists_ReturnsUnit()
    {
        // Arrange
        CreateTestJsonFile("test.json", "test", "Test Category");
        var repository = new JsonUnitRepository(_testUnitsSettingsPath, _mockLogger.Object);
        repository.Initialize();

        // Act
        var unit = await repository.GetUnitBySymbolAsync("test", "t");

        // Assert
        Assert.NotNull(unit);
        Assert.Equal("t", unit.Symbol);
    }

    [Fact]
    public async Task GetUnitBySymbolAsync_WhenNotExists_ReturnsNull()
    {
        // Arrange
        CreateTestJsonFile("test.json", "test", "Test Category");
        var repository = new JsonUnitRepository(_testUnitsSettingsPath, _mockLogger.Object);
        repository.Initialize();

        // Act
        var unit = await repository.GetUnitBySymbolAsync("test", "nonexistent");

        // Assert
        Assert.Null(unit);
    }

    [Fact]
    public async Task GetAllUnitsAsync_WhenInitialized_ReturnsAllUnits()
    {
        // Arrange
        CreateTestJsonFile("test1.json", "test1", "Test Category 1");
        CreateTestJsonFile("test2.json", "test2", "Test Category 2");
        var repository = new JsonUnitRepository(_testUnitsSettingsPath, _mockLogger.Object);
        repository.Initialize();

        // Act
        var units = await repository.GetAllUnitsAsync();

        // Assert
        Assert.NotNull(units);
        Assert.Equal(2, units.Count()); // One unit per category
    }

    [Fact]
    public async Task Initialize_WhenCalledMultipleTimes_OnlyInitializesOnce()
    {
        // Arrange
        CreateTestJsonFile("test.json", "test", "Test Category");
        var repository = new JsonUnitRepository(_testUnitsSettingsPath, _mockLogger.Object);

        // Act
        repository.Initialize();
        repository.Initialize();
        repository.Initialize();

        // Assert
        var categories = await repository.GetAllCategoriesAsync();
        Assert.Single(categories); // Should only load once
    }

    [Fact]
    public void Constructor_WhenUnitsSettingsPathIsNull_ThrowsArgumentNullException()
    {
        // Act & Assert
        Assert.Throws<ArgumentNullException>(() =>
            new JsonUnitRepository(null!, _mockLogger.Object));
    }

    [Fact]
    public void Constructor_WhenLoggerIsNull_ThrowsArgumentNullException()
    {
        // Act & Assert
        Assert.Throws<ArgumentNullException>(() =>
            new JsonUnitRepository(_testUnitsSettingsPath, null!));
    }

    private void CreateTestJsonFile(string fileName, string categoryName, string displayName)
    {
        var jsonContent = $@"{{
  ""category"": ""{categoryName}"",
  ""categoryDisplayName"": ""{displayName}"",
  ""baseUnit"": {{
    ""symbol"": ""t"",
    ""name"": ""test"",
    ""displayName"": ""Test"",
    ""isBaseUnit"": true,
    ""isSIUnit"": true,
    ""unitSystem"": ""SI""
  }},
  ""units"": [
    {{
      ""symbol"": ""t"",
      ""name"": ""test"",
      ""displayName"": ""Test"",
      ""category"": ""{categoryName}"",
      ""isBaseUnit"": true,
      ""isSIUnit"": true,
      ""unitSystem"": ""SI"",
      ""conversionFactor"": 1.0,
      ""conversionFormula"": null
    }}
  ]
}}";
        var jsonFile = Path.Combine(_testUnitsSettingsPath, fileName);
        File.WriteAllText(jsonFile, jsonContent);
    }

    public void Dispose()
    {
        if (Directory.Exists(_testUnitsSettingsPath))
        {
            Directory.Delete(_testUnitsSettingsPath, true);
        }
    }
}

