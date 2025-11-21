namespace UCConverter.Infrastructure.Tests.Repositories;

using Microsoft.Extensions.Logging;
using Moq;
using UCConverter.Infrastructure.Repositories;
using Xunit;

public class JsonUnitRepositoryBaseUnitTests : IDisposable
{
    private readonly string _testUnitsSettingsPath;
    private readonly Mock<ILogger<JsonUnitRepository>> _mockLogger;

    public JsonUnitRepositoryBaseUnitTests()
    {
        _testUnitsSettingsPath = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
        Directory.CreateDirectory(_testUnitsSettingsPath);
        _mockLogger = new Mock<ILogger<JsonUnitRepository>>();
    }

    [Fact]
    public async Task Initialize_WhenBaseUnitAlreadyInUnitsList_DoesNotDuplicate()
    {
        // Arrange - Base unit already in units array
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
        var category = await repository.GetCategoryByNameAsync("test");
        Assert.NotNull(category);
        // Should not duplicate base unit
        Assert.Single(category!.Units);
    }

    [Fact]
    public async Task Initialize_WhenBaseUnitNotInUnitsList_AddsBaseUnit()
    {
        // Arrange - Base unit not in units array
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

        // Assert
        var category = await repository.GetCategoryByNameAsync("test");
        Assert.NotNull(category);
        // Base unit should be added, so we have 2 units
        Assert.Equal(2, category!.Units.Count);
        Assert.Contains(category.Units, u => u.Symbol == "t");
        Assert.Contains(category.Units, u => u.Symbol == "u");
    }

    [Fact]
    public async Task Initialize_WhenJsonFileIsNull_LogsWarning()
    {
        // Arrange
        var jsonContent = "null";
        var jsonFile = Path.Combine(_testUnitsSettingsPath, "test.json");
        File.WriteAllText(jsonFile, jsonContent);

        var repository = new JsonUnitRepository(_testUnitsSettingsPath, _mockLogger.Object);

        // Act
        repository.Initialize();

        // Assert
        // Should log warning and continue
        _mockLogger.Verify(
            x => x.Log(
                LogLevel.Warning,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => v.ToString()!.Contains("Failed to deserialize")),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
            Times.Once);
    }

    public void Dispose()
    {
        if (Directory.Exists(_testUnitsSettingsPath))
        {
            Directory.Delete(_testUnitsSettingsPath, true);
        }
    }
}

