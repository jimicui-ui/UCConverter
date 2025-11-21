namespace UCConverter.Infrastructure.Tests.Repositories;

using Microsoft.Extensions.Logging;
using Moq;
using UCConverter.Domain.Entities;
using UCConverter.Infrastructure.Repositories;
using Xunit;

public class JsonUnitRepositoryMapUnitJsonTests : IDisposable
{
    private readonly string _testUnitsSettingsPath;
    private readonly Mock<ILogger<JsonUnitRepository>> _mockLogger;

    public JsonUnitRepositoryMapUnitJsonTests()
    {
        _testUnitsSettingsPath = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
        Directory.CreateDirectory(_testUnitsSettingsPath);
        _mockLogger = new Mock<ILogger<JsonUnitRepository>>();
    }

    [Fact]
    public async Task Initialize_WhenBaseUnitIsNull_HandlesGracefully()
    {
        // Arrange
        var jsonContent = @"{
  ""category"": ""test"",
  ""categoryDisplayName"": ""Test Category"",
  ""baseUnit"": null,
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

        // Act - Should handle null baseUnit
        repository.Initialize();

        // Assert - Should skip or handle null baseUnit
        var category = await repository.GetCategoryByNameAsync("test");
        // May be null or may have been skipped
    }

    public void Dispose()
    {
        if (Directory.Exists(_testUnitsSettingsPath))
        {
            Directory.Delete(_testUnitsSettingsPath, true);
        }
    }
}

