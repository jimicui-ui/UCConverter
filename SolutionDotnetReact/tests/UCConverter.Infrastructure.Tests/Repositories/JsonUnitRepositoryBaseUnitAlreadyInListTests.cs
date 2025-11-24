namespace UCConverter.Infrastructure.Tests.Repositories;

using Microsoft.Extensions.Logging;
using Moq;
using UCConverter.Infrastructure.Repositories;
using Xunit;

public class JsonUnitRepositoryBaseUnitAlreadyInListTests : IDisposable
{
    private readonly string _testUnitsSettingsPath;
    private readonly Mock<ILogger<JsonUnitRepository>> _mockLogger;

    public JsonUnitRepositoryBaseUnitAlreadyInListTests()
    {
        _testUnitsSettingsPath = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
        Directory.CreateDirectory(_testUnitsSettingsPath);
        _mockLogger = new Mock<ILogger<JsonUnitRepository>>();
    }

    [Fact]
    public async Task Initialize_WhenBaseUnitAlreadyInUnitsList_DoesNotAddDuplicate()
    {
        // Arrange - JSON where base unit is already in the units array
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
    },
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

        // Act
        repository.Initialize();

        // Assert - Base unit should not be duplicated
        var category = await repository.GetCategoryByNameAsync("test");
        Assert.NotNull(category);
        var baseUnitCount = category!.Units.Count(u => u.Symbol.Equals("t", StringComparison.OrdinalIgnoreCase) && u.IsBaseUnit);
        Assert.Equal(1, baseUnitCount); // Should only be one base unit
        Assert.Equal(2, category.Units.Count); // Should have exactly 2 units total
    }

    [Fact]
    public async Task Initialize_WhenBaseUnitNotInUnitsList_AddsBaseUnit()
    {
        // Arrange - JSON where base unit is NOT in the units array
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

        // Act
        repository.Initialize();

        // Assert - Base unit should be added to the list
        var category = await repository.GetCategoryByNameAsync("test");
        Assert.NotNull(category);
        var baseUnit = category!.Units.FirstOrDefault(u => u.Symbol.Equals("t", StringComparison.OrdinalIgnoreCase) && u.IsBaseUnit);
        Assert.NotNull(baseUnit);
        Assert.Equal(2, category.Units.Count); // Should have base unit + the other unit
    }

    public void Dispose()
    {
        if (Directory.Exists(_testUnitsSettingsPath))
        {
            Directory.Delete(_testUnitsSettingsPath, true);
        }
    }
}

