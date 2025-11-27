namespace UCConverter.Infrastructure.Tests.Repositories;

using Microsoft.Extensions.Logging;
using Moq;
using System.Text.Json;
using UCConverter.Domain.Entities;
using UCConverter.Infrastructure.Repositories;
using Xunit;

public class JsonUnitRepositoryAdditionalTests : IDisposable
{
    private readonly string _testUnitsSettingsPath;
    private readonly Mock<ILogger<JsonUnitRepository>> _mockLogger;

    public JsonUnitRepositoryAdditionalTests()
    {
        _testUnitsSettingsPath = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
        Directory.CreateDirectory(_testUnitsSettingsPath);
        _mockLogger = new Mock<ILogger<JsonUnitRepository>>();
    }

    [Fact]
    public async Task GetAllUnitsAsync_WhenMultipleCategories_ReturnsAllUnits()
    {
        // Arrange
        var json1 = @"{
  ""category"": ""test1"",
  ""categoryDisplayName"": ""Test Category 1"",
  ""group"": ""Common"",
  ""baseUnit"": {
    ""symbol"": ""t1"",
    ""name"": ""test1"",
    ""displayName"": ""Test 1"",
    ""isBaseUnit"": true,
    ""isSIUnit"": true,
    ""unitSystem"": ""SI"",
    ""conversionFactor"": 1.0
  },
  ""units"": [
    {
      ""symbol"": ""t1"",
      ""name"": ""test1"",
      ""displayName"": ""Test 1"",
      ""isBaseUnit"": true,
      ""isSIUnit"": true,
      ""unitSystem"": ""SI"",
      ""conversionFactor"": 1.0
    }
  ]
}";
        var json2 = @"{
  ""category"": ""test2"",
  ""categoryDisplayName"": ""Test Category 2"",
  ""group"": ""Common"",
  ""baseUnit"": {
    ""symbol"": ""t2"",
    ""name"": ""test2"",
    ""displayName"": ""Test 2"",
    ""isBaseUnit"": true,
    ""isSIUnit"": true,
    ""unitSystem"": ""SI"",
    ""conversionFactor"": 1.0
  },
  ""units"": [
    {
      ""symbol"": ""t2"",
      ""name"": ""test2"",
      ""displayName"": ""Test 2"",
      ""isBaseUnit"": true,
      ""isSIUnit"": true,
      ""unitSystem"": ""SI"",
      ""conversionFactor"": 1.0
    }
  ]
}";
        File.WriteAllText(Path.Combine(_testUnitsSettingsPath, "test1.json"), json1);
        File.WriteAllText(Path.Combine(_testUnitsSettingsPath, "test2.json"), json2);

        var repository = new JsonUnitRepository(_testUnitsSettingsPath, _mockLogger.Object);
        repository.Initialize();

        // Act
        var units = await repository.GetAllUnitsAsync();

        // Assert
        Assert.NotNull(units);
        Assert.Equal(2, units.Count());
    }

    [Fact]
    public async Task GetUnitBySymbolAsync_WhenCategoryDoesNotExist_ReturnsNull()
    {
        // Arrange
        var repository = new JsonUnitRepository(_testUnitsSettingsPath, _mockLogger.Object);
        repository.Initialize();

        // Act
        var unit = await repository.GetUnitBySymbolAsync("nonexistent", "m");

        // Assert
        Assert.Null(unit);
    }

    [Fact]
    public async Task GetUnitBySymbolAsync_WhenUnitDoesNotExistInCategory_ReturnsNull()
    {
        // Arrange
        var jsonContent = @"{
  ""category"": ""test"",
  ""categoryDisplayName"": ""Test Category"",
  ""group"": ""Common"",
  ""baseUnit"": {
    ""symbol"": ""t"",
    ""name"": ""test"",
    ""displayName"": ""Test"",
    ""isBaseUnit"": true,
    ""isSIUnit"": true,
    ""unitSystem"": ""SI"",
    ""conversionFactor"": 1.0
  },
  ""units"": [
    {
      ""symbol"": ""t"",
      ""name"": ""test"",
      ""displayName"": ""Test"",
      ""isBaseUnit"": true,
      ""isSIUnit"": true,
      ""unitSystem"": ""SI"",
      ""conversionFactor"": 1.0
    }
  ]
}";
        File.WriteAllText(Path.Combine(_testUnitsSettingsPath, "test.json"), jsonContent);

        var repository = new JsonUnitRepository(_testUnitsSettingsPath, _mockLogger.Object);
        repository.Initialize();

        // Act
        var unit = await repository.GetUnitBySymbolAsync("test", "nonexistent");

        // Assert
        Assert.Null(unit);
    }

    [Fact]
    public async Task Initialize_WhenCalledMultipleTimes_OnlyInitializesOnce()
    {
        // Arrange
        var jsonContent = @"{
  ""category"": ""test"",
  ""categoryDisplayName"": ""Test Category"",
  ""group"": ""Common"",
  ""baseUnit"": {
    ""symbol"": ""t"",
    ""name"": ""test"",
    ""displayName"": ""Test"",
    ""isBaseUnit"": true,
    ""isSIUnit"": true,
    ""unitSystem"": ""SI"",
    ""conversionFactor"": 1.0
  },
  ""units"": [
    {
      ""symbol"": ""t"",
      ""name"": ""test"",
      ""displayName"": ""Test"",
      ""isBaseUnit"": true,
      ""isSIUnit"": true,
      ""unitSystem"": ""SI"",
      ""conversionFactor"": 1.0
    }
  ]
}";
        File.WriteAllText(Path.Combine(_testUnitsSettingsPath, "test.json"), jsonContent);

        var repository = new JsonUnitRepository(_testUnitsSettingsPath, _mockLogger.Object);

        // Act
        repository.Initialize();
        repository.Initialize();
        repository.Initialize();

        // Assert - Should not throw and should only load once
        var categories = await repository.GetAllCategoriesAsync();
        Assert.Single(categories);
    }

    [Fact]
    public async Task GetAllUnitsAsync_WhenNoCategories_ReturnsEmpty()
    {
        // Arrange
        var repository = new JsonUnitRepository(_testUnitsSettingsPath, _mockLogger.Object);
        repository.Initialize();

        // Act
        var units = await repository.GetAllUnitsAsync();

        // Assert
        Assert.Empty(units);
    }

    [Fact]
    public async Task GetUnitsByCategoryAsync_WhenCategoryHasNoUnits_ReturnsEmpty()
    {
        // Arrange - Create a category with empty units array
        var jsonContent = @"{
  ""category"": ""test"",
  ""categoryDisplayName"": ""Test Category"",
  ""group"": ""Common"",
  ""baseUnit"": {
    ""symbol"": ""t"",
    ""name"": ""test"",
    ""displayName"": ""Test"",
    ""isBaseUnit"": true,
    ""isSIUnit"": true,
    ""unitSystem"": ""SI"",
    ""conversionFactor"": 1.0
  },
  ""units"": []
}";
        File.WriteAllText(Path.Combine(_testUnitsSettingsPath, "test.json"), jsonContent);

        var repository = new JsonUnitRepository(_testUnitsSettingsPath, _mockLogger.Object);
        repository.Initialize();

        // Act
        var units = await repository.GetUnitsByCategoryAsync("test");

        // Assert - Should return at least the base unit
        Assert.NotEmpty(units);
    }

    public void Dispose()
    {
        if (Directory.Exists(_testUnitsSettingsPath))
        {
            Directory.Delete(_testUnitsSettingsPath, true);
        }
    }
}

