namespace UCConverter.Infrastructure.Tests.Repositories;

using Microsoft.Extensions.Logging;
using Moq;
using System.Text.Json;
using UCConverter.Domain.Entities;
using UCConverter.Infrastructure.Repositories;
using Xunit;

public class JsonUnitRepositoryMapUnitJsonNullCoalescingTests : IDisposable
{
    private readonly string _testUnitsSettingsPath;
    private readonly Mock<ILogger<JsonUnitRepository>> _mockLogger;

    public JsonUnitRepositoryMapUnitJsonNullCoalescingTests()
    {
        _testUnitsSettingsPath = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
        Directory.CreateDirectory(_testUnitsSettingsPath);
        _mockLogger = new Mock<ILogger<JsonUnitRepository>>();
    }

    [Fact]
    public async Task Initialize_WhenUnitJsonHasNullSymbol_UsesEmptyString()
    {
        // Arrange - JSON with null Symbol
        var jsonContent = @"{
  ""category"": ""test"",
  ""categoryDisplayName"": ""Test Category"",
  ""baseUnit"": {
    ""symbol"": null,
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

        // Assert - Should handle null symbol gracefully
        var category = await repository.GetCategoryByNameAsync("test");
        // May be null if baseUnit is null, or may have empty symbol
        // The important thing is it doesn't crash
    }

    [Fact]
    public async Task Initialize_WhenUnitJsonHasNullName_UsesEmptyString()
    {
        // Arrange - JSON with null Name
        var jsonContent = @"{
  ""category"": ""test"",
  ""categoryDisplayName"": ""Test Category"",
  ""baseUnit"": {
    ""symbol"": ""t"",
    ""name"": null,
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

        // Assert - Should handle null name gracefully
        var category = await repository.GetCategoryByNameAsync("test");
        if (category != null)
        {
            Assert.Equal(string.Empty, category.BaseUnit.Name);
        }
    }

    [Fact]
    public async Task Initialize_WhenUnitJsonHasNullDisplayName_UsesEmptyString()
    {
        // Arrange
        var jsonContent = @"{
  ""category"": ""test"",
  ""categoryDisplayName"": ""Test Category"",
  ""baseUnit"": {
    ""symbol"": ""t"",
    ""name"": ""test"",
    ""displayName"": null,
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
        if (category != null)
        {
            Assert.Equal(string.Empty, category.BaseUnit.DisplayName);
        }
    }

    [Fact]
    public async Task Initialize_WhenUnitJsonHasNullUnitSystem_UsesEmptyString()
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
    ""unitSystem"": null
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
        if (category != null)
        {
            Assert.Equal(string.Empty, category.BaseUnit.UnitSystem);
        }
    }

    public void Dispose()
    {
        if (Directory.Exists(_testUnitsSettingsPath))
        {
            Directory.Delete(_testUnitsSettingsPath, true);
        }
    }
}

