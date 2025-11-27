namespace UCConverter.Infrastructure.Tests.Repositories;

using Microsoft.Extensions.Logging;
using Moq;
using System.Text.Json;
using UCConverter.Domain.Entities;
using UCConverter.Infrastructure.Repositories;
using Xunit;

public class JsonUnitRepositoryLoadCategoryTests : IDisposable
{
    private readonly string _testUnitsSettingsPath;
    private readonly Mock<ILogger<JsonUnitRepository>> _mockLogger;

    public JsonUnitRepositoryLoadCategoryTests()
    {
        _testUnitsSettingsPath = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
        Directory.CreateDirectory(_testUnitsSettingsPath);
        _mockLogger = new Mock<ILogger<JsonUnitRepository>>();
    }

    [Fact]
    public void LoadCategoryFromFile_WhenUnitsArrayIsNull_StillCreatesCategory()
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
  ""units"": null
}";
        File.WriteAllText(Path.Combine(_testUnitsSettingsPath, "test.json"), jsonContent);

        var repository = new JsonUnitRepository(_testUnitsSettingsPath, _mockLogger.Object);

        // Act
        repository.Initialize();

        // Assert
        var category = repository.GetCategoryByNameAsync("test").Result;
        Assert.NotNull(category);
        Assert.Single(category!.Units); // Should have base unit
    }

    [Fact]
    public void LoadCategoryFromFile_WhenUnitHasNullSymbol_UsesEmptyString()
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
      ""symbol"": null,
      ""name"": ""test2"",
      ""displayName"": ""Test 2"",
      ""isBaseUnit"": false,
      ""isSIUnit"": true,
      ""unitSystem"": ""SI"",
      ""conversionFactor"": 2.0
    }
  ]
}";
        File.WriteAllText(Path.Combine(_testUnitsSettingsPath, "test.json"), jsonContent);

        var repository = new JsonUnitRepository(_testUnitsSettingsPath, _mockLogger.Object);

        // Act
        repository.Initialize();

        // Assert
        var category = repository.GetCategoryByNameAsync("test").Result;
        Assert.NotNull(category);
        var unitWithNullSymbol = category!.Units.FirstOrDefault(u => u.Name == "test2");
        Assert.NotNull(unitWithNullSymbol);
        Assert.Equal(string.Empty, unitWithNullSymbol!.Symbol);
    }

    [Fact]
    public void LoadCategoryFromFile_WhenUnitHasNullName_UsesEmptyString()
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
      ""symbol"": ""t2"",
      ""name"": null,
      ""displayName"": ""Test 2"",
      ""isBaseUnit"": false,
      ""isSIUnit"": true,
      ""unitSystem"": ""SI"",
      ""conversionFactor"": 2.0
    }
  ]
}";
        File.WriteAllText(Path.Combine(_testUnitsSettingsPath, "test.json"), jsonContent);

        var repository = new JsonUnitRepository(_testUnitsSettingsPath, _mockLogger.Object);

        // Act
        repository.Initialize();

        // Assert
        var category = repository.GetCategoryByNameAsync("test").Result;
        Assert.NotNull(category);
        var unitWithNullName = category!.Units.FirstOrDefault(u => u.Symbol == "t2");
        Assert.NotNull(unitWithNullName);
        Assert.Equal(string.Empty, unitWithNullName!.Name);
    }

    [Fact]
    public void LoadCategoryFromFile_WhenUnitHasNullDisplayName_UsesEmptyString()
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
      ""symbol"": ""t2"",
      ""name"": ""test2"",
      ""displayName"": null,
      ""isBaseUnit"": false,
      ""isSIUnit"": true,
      ""unitSystem"": ""SI"",
      ""conversionFactor"": 2.0
    }
  ]
}";
        File.WriteAllText(Path.Combine(_testUnitsSettingsPath, "test.json"), jsonContent);

        var repository = new JsonUnitRepository(_testUnitsSettingsPath, _mockLogger.Object);

        // Act
        repository.Initialize();

        // Assert
        var category = repository.GetCategoryByNameAsync("test").Result;
        Assert.NotNull(category);
        var unitWithNullDisplayName = category!.Units.FirstOrDefault(u => u.Symbol == "t2");
        Assert.NotNull(unitWithNullDisplayName);
        Assert.Equal(string.Empty, unitWithNullDisplayName!.DisplayName);
    }

    [Fact]
    public void LoadCategoryFromFile_WhenUnitHasNullUnitSystem_UsesEmptyString()
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
      ""symbol"": ""t2"",
      ""name"": ""test2"",
      ""displayName"": ""Test 2"",
      ""isBaseUnit"": false,
      ""isSIUnit"": true,
      ""unitSystem"": null,
      ""conversionFactor"": 2.0
    }
  ]
}";
        File.WriteAllText(Path.Combine(_testUnitsSettingsPath, "test.json"), jsonContent);

        var repository = new JsonUnitRepository(_testUnitsSettingsPath, _mockLogger.Object);

        // Act
        repository.Initialize();

        // Assert
        var category = repository.GetCategoryByNameAsync("test").Result;
        Assert.NotNull(category);
        var unitWithNullUnitSystem = category!.Units.FirstOrDefault(u => u.Symbol == "t2");
        Assert.NotNull(unitWithNullUnitSystem);
        Assert.Equal(string.Empty, unitWithNullUnitSystem!.UnitSystem);
    }

    [Fact]
    public void Initialize_WhenSomeFilesFailToLoad_ContinuesLoadingOthers()
    {
        // Arrange
        var validJson = @"{
  ""category"": ""valid"",
  ""categoryDisplayName"": ""Valid Category"",
  ""group"": ""Common"",
  ""baseUnit"": {
    ""symbol"": ""v"",
    ""name"": ""valid"",
    ""displayName"": ""Valid"",
    ""isBaseUnit"": true,
    ""isSIUnit"": true,
    ""unitSystem"": ""SI"",
    ""conversionFactor"": 1.0
  },
  ""units"": [
    {
      ""symbol"": ""v"",
      ""name"": ""valid"",
      ""displayName"": ""Valid"",
      ""isBaseUnit"": true,
      ""isSIUnit"": true,
      ""unitSystem"": ""SI"",
      ""conversionFactor"": 1.0
    }
  ]
}";
        File.WriteAllText(Path.Combine(_testUnitsSettingsPath, "valid.json"), validJson);
        File.WriteAllText(Path.Combine(_testUnitsSettingsPath, "invalid.json"), "{ invalid json }");

        var repository = new JsonUnitRepository(_testUnitsSettingsPath, _mockLogger.Object);

        // Act
        repository.Initialize();

        // Assert
        var validCategory = repository.GetCategoryByNameAsync("valid").Result;
        Assert.NotNull(validCategory);
        _mockLogger.Verify(
            x => x.Log(
                LogLevel.Error,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => v.ToString()!.Contains("Failed to load unit configuration")),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
            Times.AtLeastOnce);
    }

    [Fact]
    public void LoadCategoryFromFile_WhenBaseUnitJsonIsNull_LogsWarningAndReturns()
    {
        // Arrange
        var jsonContent = @"{
  ""category"": ""test"",
  ""categoryDisplayName"": ""Test Category"",
  ""group"": ""Common"",
  ""baseUnit"": null,
  ""units"": []
}";
        File.WriteAllText(Path.Combine(_testUnitsSettingsPath, "test.json"), jsonContent);

        var repository = new JsonUnitRepository(_testUnitsSettingsPath, _mockLogger.Object);

        // Act
        repository.Initialize();

        // Assert
        var category = repository.GetCategoryByNameAsync("test").Result;
        Assert.Null(category);
        _mockLogger.Verify(
            x => x.Log(
                LogLevel.Warning,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => v.ToString()!.Contains("BaseUnit is null")),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
            Times.Once);
    }

    [Fact]
    public void LoadCategoryFromFile_WhenGroupIsNull_UsesCommonAsDefault()
    {
        // Arrange
        var jsonContent = @"{
  ""category"": ""test"",
  ""categoryDisplayName"": ""Test Category"",
  ""group"": null,
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

        // Assert
        var category = repository.GetCategoryByNameAsync("test").Result;
        Assert.NotNull(category);
        Assert.Equal("Common", category!.Group);
    }

    public void Dispose()
    {
        if (Directory.Exists(_testUnitsSettingsPath))
        {
            Directory.Delete(_testUnitsSettingsPath, true);
        }
    }
}

