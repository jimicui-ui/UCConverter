namespace UCConverter.Infrastructure.Tests.Repositories;

using Microsoft.Extensions.Logging;
using Moq;
using UCConverter.Domain.Entities;
using UCConverter.Infrastructure.Repositories;
using Xunit;

/// <summary>
/// Additional edge case tests to improve coverage to 95%
/// </summary>
public class JsonUnitRepositoryEdgeCaseCoverageTests : IDisposable
{
    private readonly string _testUnitsSettingsPath;
    private readonly Mock<ILogger<JsonUnitRepository>> _mockLogger;

    public JsonUnitRepositoryEdgeCaseCoverageTests()
    {
        _testUnitsSettingsPath = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
        Directory.CreateDirectory(_testUnitsSettingsPath);
        _mockLogger = new Mock<ILogger<JsonUnitRepository>>();
    }

    [Fact]
    public async Task LoadCategoryFromFile_WhenUnitsListIsNull_UsesEmptyList()
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
                ""unitSystem"": ""SI"",
                ""conversionFactor"": 1.0
            },
            ""units"": null
        }";
        File.WriteAllText(Path.Combine(_testUnitsSettingsPath, "test.json"), jsonContent);

        var repository = new JsonUnitRepository(_testUnitsSettingsPath, _mockLogger.Object);
        repository.Initialize();

        // Act
        var category = await repository.GetCategoryByNameAsync("test");

        // Assert
        Assert.NotNull(category);
        Assert.NotNull(category!.Units);
        Assert.Single(category.Units); // Should contain only the base unit
    }

    [Fact]
    public async Task LoadCategoryFromFile_WhenBaseUnitAlreadyInUnitsList_DoesNotAddDuplicate()
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
        var category = await repository.GetCategoryByNameAsync("test");

        // Assert
        Assert.NotNull(category);
        Assert.Single(category!.Units); // Should not have duplicate
    }

    [Fact]
    public async Task LoadCategoryFromFile_WhenGroupIsNull_DefaultsToCommon()
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
            ""units"": []
        }";
        File.WriteAllText(Path.Combine(_testUnitsSettingsPath, "test.json"), jsonContent);

        var repository = new JsonUnitRepository(_testUnitsSettingsPath, _mockLogger.Object);
        repository.Initialize();

        // Act
        var category = await repository.GetCategoryByNameAsync("test");

        // Assert
        Assert.NotNull(category);
        Assert.Equal("Common", category!.Group);
    }

    [Fact]
    public async Task LoadCategoryFromFile_WhenGroupIsProvided_UsesProvidedGroup()
    {
        // Arrange
        var jsonContent = @"{
            ""category"": ""test"",
            ""categoryDisplayName"": ""Test Category"",
            ""group"": ""Engineering"",
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
        var category = await repository.GetCategoryByNameAsync("test");

        // Assert
        Assert.NotNull(category);
        Assert.Equal("Engineering", category!.Group);
    }

    [Fact]
    public async Task LoadCategoryFromFile_WhenCategoryJsonIsNull_LogsWarningAndReturns()
    {
        // Arrange
        var invalidJsonContent = @"null";
        File.WriteAllText(Path.Combine(_testUnitsSettingsPath, "null.json"), invalidJsonContent);

        var repository = new JsonUnitRepository(_testUnitsSettingsPath, _mockLogger.Object);

        // Act
        repository.Initialize();

        // Assert
        _mockLogger.Verify(
            x => x.Log(
                LogLevel.Warning,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => v.ToString()!.Contains("Failed to deserialize JSON file")),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
            Times.Once);

        var categories = await repository.GetAllCategoriesAsync();
        Assert.Empty(categories);
    }

    [Fact]
    public async Task LoadCategoryFromFile_WhenBaseUnitIsNull_LogsWarningAndReturns()
    {
        // Arrange
        var jsonContent = @"{
            ""category"": ""test"",
            ""categoryDisplayName"": ""Test Category"",
            ""baseUnit"": null,
            ""units"": []
        }";
        File.WriteAllText(Path.Combine(_testUnitsSettingsPath, "test.json"), jsonContent);

        var repository = new JsonUnitRepository(_testUnitsSettingsPath, _mockLogger.Object);

        // Act
        repository.Initialize();

        // Assert
        _mockLogger.Verify(
            x => x.Log(
                LogLevel.Warning,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => v.ToString()!.Contains("BaseUnit is null in category file")),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
            Times.Once);

        var categories = await repository.GetAllCategoriesAsync();
        Assert.Empty(categories);
    }

    [Fact]
    public async Task GetCategoryByNameAsync_WhenCategoryNotFound_ReturnsNull()
    {
        // Arrange
        var repository = new JsonUnitRepository(_testUnitsSettingsPath, _mockLogger.Object);
        repository.Initialize();

        // Act
        var category = await repository.GetCategoryByNameAsync("nonexistent");

        // Assert
        Assert.Null(category);
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
                ""unitSystem"": ""SI"",
                ""conversionFactor"": 1.0
            },
            ""units"": []
        }";
        File.WriteAllText(Path.Combine(_testUnitsSettingsPath, "test.json"), jsonContent);

        var repository = new JsonUnitRepository(_testUnitsSettingsPath, _mockLogger.Object);
        repository.Initialize();

        // Act
        var category = await repository.GetCategoryByNameAsync("test");

        // Assert
        Assert.NotNull(category);
        Assert.Equal("test", category!.Name);
    }

    [Fact]
    public async Task GetCategoryByNameAsync_WhenCategoryNameIsCaseInsensitive_ReturnsCategory()
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
                ""unitSystem"": ""SI"",
                ""conversionFactor"": 1.0
            },
            ""units"": []
        }";
        File.WriteAllText(Path.Combine(_testUnitsSettingsPath, "test.json"), jsonContent);

        var repository = new JsonUnitRepository(_testUnitsSettingsPath, _mockLogger.Object);
        repository.Initialize();

        // Act
        var category = await repository.GetCategoryByNameAsync("TEST");

        // Assert
        Assert.NotNull(category);
        Assert.Equal("test", category!.Name);
    }

    [Fact]
    public async Task GetUnitsByCategoryAsync_WhenCategoryHasUnits_ReturnsAllUnits()
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
                ""unitSystem"": ""SI"",
                ""conversionFactor"": 1.0
            },
            ""units"": [
                {
                    ""symbol"": ""t1"",
                    ""name"": ""test1"",
                    ""displayName"": ""Test 1"",
                    ""isBaseUnit"": false,
                    ""isSIUnit"": true,
                    ""unitSystem"": ""SI"",
                    ""conversionFactor"": 2.0
                }
            ]
        }";
        File.WriteAllText(Path.Combine(_testUnitsSettingsPath, "test.json"), jsonContent);

        var repository = new JsonUnitRepository(_testUnitsSettingsPath, _mockLogger.Object);
        repository.Initialize();

        // Act
        var units = await repository.GetUnitsByCategoryAsync("test");

        // Assert
        var unitsList = units.ToList();
        Assert.Equal(2, unitsList.Count); // Base unit + 1 additional unit
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
                ""unitSystem"": ""SI"",
                ""conversionFactor"": 1.0
            },
            ""units"": []
        }";
        File.WriteAllText(Path.Combine(_testUnitsSettingsPath, "test.json"), jsonContent);

        var repository = new JsonUnitRepository(_testUnitsSettingsPath, _mockLogger.Object);
        repository.Initialize();

        // Act
        var unit = await repository.GetUnitBySymbolAsync("test", "t");

        // Assert
        Assert.NotNull(unit);
        Assert.Equal("t", unit!.Symbol);
    }

    [Fact]
    public async Task GetUnitBySymbolAsync_WhenUnitNotFound_ReturnsNull()
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
                ""unitSystem"": ""SI"",
                ""conversionFactor"": 1.0
            },
            ""units"": []
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
    public async Task GetUnitBySymbolAsync_WhenCategoryNotFound_ReturnsNull()
    {
        // Arrange
        var repository = new JsonUnitRepository(_testUnitsSettingsPath, _mockLogger.Object);
        repository.Initialize();

        // Act
        var unit = await repository.GetUnitBySymbolAsync("nonexistent", "t");

        // Assert
        Assert.Null(unit);
    }

    public void Dispose()
    {
        if (Directory.Exists(_testUnitsSettingsPath))
        {
            Directory.Delete(_testUnitsSettingsPath, true);
        }
    }
}

