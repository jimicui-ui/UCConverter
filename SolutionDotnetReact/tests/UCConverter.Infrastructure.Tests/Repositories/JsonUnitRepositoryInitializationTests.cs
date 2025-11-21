namespace UCConverter.Infrastructure.Tests.Repositories;

using Microsoft.Extensions.Logging;
using Moq;
using UCConverter.Infrastructure.Repositories;
using Xunit;

public class JsonUnitRepositoryInitializationTests : IDisposable
{
    private readonly string _testUnitsSettingsPath;
    private readonly Mock<ILogger<JsonUnitRepository>> _mockLogger;

    public JsonUnitRepositoryInitializationTests()
    {
        _testUnitsSettingsPath = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
        Directory.CreateDirectory(_testUnitsSettingsPath);
        _mockLogger = new Mock<ILogger<JsonUnitRepository>>();
    }

    [Fact]
    public async Task Initialize_WhenCalledMultipleTimes_OnlyInitializesOnce()
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
        repository.Initialize(); // Call again

        // Assert - Should only load once
        var categories = await repository.GetAllCategoriesAsync();
        Assert.Single(categories);
    }

    [Fact]
    public async Task GetAllCategoriesAsync_WhenInitialized_ReturnsCategories()
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
        repository.Initialize();

        // Act
        var categories = await repository.GetAllCategoriesAsync();

        // Assert
        Assert.NotNull(categories);
        Assert.Single(categories);
    }

    [Fact]
    public async Task GetCategoryByNameAsync_WhenCategoryExists_ReturnsCategory()
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
        repository.Initialize();

        // Act
        var category = await repository.GetCategoryByNameAsync("test");

        // Assert
        Assert.NotNull(category);
        Assert.Equal("test", category!.Name);
    }

    [Fact]
    public async Task GetUnitsByCategoryAsync_WhenCategoryExists_ReturnsUnits()
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
      ""symbol"": ""u"",
      ""name"": ""unit"",
      ""displayName"": ""Unit"",
      ""category"": ""test"",
      ""isBaseUnit"": false,
      ""isSIUnit"": true,
      ""unitSystem"": ""SI"",
      ""conversionFactor"": 2.0,
      ""conversionFormula"": null
    }
  ]
}";
        var jsonFile = Path.Combine(_testUnitsSettingsPath, "test.json");
        File.WriteAllText(jsonFile, jsonContent);

        var repository = new JsonUnitRepository(_testUnitsSettingsPath, _mockLogger.Object);
        repository.Initialize();

        // Act
        var units = await repository.GetUnitsByCategoryAsync("test");

        // Assert
        Assert.NotNull(units);
        Assert.Equal(2, units.Count()); // Base unit + one unit
    }

    [Fact]
    public async Task GetUnitBySymbolAsync_WhenUnitExists_ReturnsUnit()
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
        repository.Initialize();

        // Act
        var unit = await repository.GetUnitBySymbolAsync("test", "t");

        // Assert
        Assert.NotNull(unit);
        Assert.Equal("t", unit!.Symbol);
    }

    [Fact]
    public async Task GetAllUnitsAsync_WhenInitialized_ReturnsAllUnits()
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
      ""symbol"": ""u1"",
      ""name"": ""unit1"",
      ""displayName"": ""Unit1"",
      ""category"": ""test"",
      ""isBaseUnit"": false,
      ""isSIUnit"": true,
      ""unitSystem"": ""SI"",
      ""conversionFactor"": 2.0,
      ""conversionFormula"": null
    },
    {
      ""symbol"": ""u2"",
      ""name"": ""unit2"",
      ""displayName"": ""Unit2"",
      ""category"": ""test"",
      ""isBaseUnit"": false,
      ""isSIUnit"": true,
      ""unitSystem"": ""SI"",
      ""conversionFactor"": 3.0,
      ""conversionFormula"": null
    }
  ]
}";
        var jsonFile = Path.Combine(_testUnitsSettingsPath, "test.json");
        File.WriteAllText(jsonFile, jsonContent);

        var repository = new JsonUnitRepository(_testUnitsSettingsPath, _mockLogger.Object);
        repository.Initialize();

        // Act
        var units = await repository.GetAllUnitsAsync();

        // Assert
        Assert.NotNull(units);
        Assert.Equal(3, units.Count()); // Base unit + 2 units
    }

    public void Dispose()
    {
        if (Directory.Exists(_testUnitsSettingsPath))
        {
            Directory.Delete(_testUnitsSettingsPath, true);
        }
    }
}

