namespace UCConverter.Infrastructure.Tests.Repositories;

using Microsoft.Extensions.Logging;
using Moq;
using UCConverter.Domain.Entities;
using UCConverter.Infrastructure.Repositories;
using Xunit;

/// <summary>
/// Complete coverage tests for all remaining code paths
/// </summary>
public class JsonUnitRepositoryCompleteCoverageTests : IDisposable
{
    private readonly string _testUnitsSettingsPath;
    private readonly Mock<ILogger<JsonUnitRepository>> _mockLogger;

    public JsonUnitRepositoryCompleteCoverageTests()
    {
        _testUnitsSettingsPath = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
        Directory.CreateDirectory(_testUnitsSettingsPath);
        _mockLogger = new Mock<ILogger<JsonUnitRepository>>();
    }

    [Fact]
    public void LoadCategoryFromFile_WhenBaseUnitNotInUnitsList_AddsBaseUnit()
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
        var category = repository.GetCategoryByNameAsync("test").Result;

        // Assert
        Assert.NotNull(category);
        Assert.Equal(2, category!.Units.Count); // Base unit + 1 additional unit
        Assert.Contains(category.Units, u => u.Symbol == "t" && u.IsBaseUnit);
    }

    [Fact]
    public void LoadCategoryFromFile_WhenBaseUnitSymbolCaseDiffers_DoesNotAddDuplicate()
    {
        // Arrange - The comparison is case-insensitive, so T and t are considered the same
        var jsonContent = @"{
            ""category"": ""test"",
            ""categoryDisplayName"": ""Test Category"",
            ""baseUnit"": {
                ""symbol"": ""T"",
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
                    ""isBaseUnit"": false,
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
        var category = repository.GetCategoryByNameAsync("test").Result;

        // Assert
        Assert.NotNull(category);
        // Case-insensitive comparison means T and t are considered the same, so only one should be present
        Assert.Single(category!.Units);
    }

    [Fact]
    public void LoadCategoryFromFile_WhenCategoryAddedToCache_CanRetrieveIt()
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
        var category1 = repository.GetCategoryByNameAsync("test").Result;
        var category2 = repository.GetCategoryByNameAsync("test").Result;

        // Assert
        Assert.NotNull(category1);
        Assert.NotNull(category2);
        Assert.Same(category1, category2); // Should be same instance from cache
    }

    [Fact]
    public async Task GetAllCategoriesAsync_WhenMultipleCategories_ReturnsAll()
    {
        // Arrange
        for (int i = 1; i <= 3; i++)
        {
            var jsonContent = $@"{{
                ""category"": ""test{i}"",
                ""categoryDisplayName"": ""Test Category {i}"",
                ""baseUnit"": {{
                    ""symbol"": ""t{i}"",
                    ""name"": ""test{i}"",
                    ""displayName"": ""Test {i}"",
                    ""isBaseUnit"": true,
                    ""isSIUnit"": true,
                    ""unitSystem"": ""SI"",
                    ""conversionFactor"": 1.0
                }},
                ""units"": []
            }}";
            File.WriteAllText(Path.Combine(_testUnitsSettingsPath, $"test{i}.json"), jsonContent);
        }

        var repository = new JsonUnitRepository(_testUnitsSettingsPath, _mockLogger.Object);
        repository.Initialize();

        // Act
        var categories = await repository.GetAllCategoriesAsync();

        // Assert
        var categoriesList = categories.ToList();
        Assert.Equal(3, categoriesList.Count);
        Assert.Contains(categoriesList, c => c.Name == "test1");
        Assert.Contains(categoriesList, c => c.Name == "test2");
        Assert.Contains(categoriesList, c => c.Name == "test3");
    }

    [Fact]
    public async Task GetAllCategoriesAsync_WhenEmpty_ReturnsEmptyEnumerable()
    {
        // Arrange
        var repository = new JsonUnitRepository(_testUnitsSettingsPath, _mockLogger.Object);
        repository.Initialize();

        // Act
        var categories = await repository.GetAllCategoriesAsync();

        // Assert
        Assert.NotNull(categories);
        Assert.Empty(categories);
    }

    [Fact]
    public async Task GetUnitsByCategoryAsync_WhenCategoryNotFound_ReturnsEmptyEnumerable()
    {
        // Arrange
        var repository = new JsonUnitRepository(_testUnitsSettingsPath, _mockLogger.Object);
        repository.Initialize();

        // Act
        var units = await repository.GetUnitsByCategoryAsync("nonexistent");

        // Assert
        Assert.NotNull(units);
        Assert.Empty(units);
    }

    [Fact]
    public async Task GetUnitsByCategoryAsync_WhenCategoryHasNoUnits_ReturnsOnlyBaseUnit()
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
        var units = await repository.GetUnitsByCategoryAsync("test");

        // Assert
        var unitsList = units.ToList();
        Assert.Single(unitsList);
        Assert.True(unitsList[0].IsBaseUnit);
    }

    [Fact]
    public async Task GetUnitBySymbolAsync_WhenCategoryAndUnitExist_ReturnsUnit()
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
        var unit1 = await repository.GetUnitBySymbolAsync("test", "t");
        var unit2 = await repository.GetUnitBySymbolAsync("test", "t1");

        // Assert
        Assert.NotNull(unit1);
        Assert.Equal("t", unit1!.Symbol);
        Assert.True(unit1.IsBaseUnit);

        Assert.NotNull(unit2);
        Assert.Equal("t1", unit2!.Symbol);
        Assert.False(unit2.IsBaseUnit);
    }

    [Fact]
    public void MapUnitJson_WhenAllPropertiesSet_MapsCorrectly()
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
                ""conversionFactor"": 1.0,
                ""conversionFormula"": ""x + 273.15"",
                ""conversionInverseFormula"": ""x - 273.15""
            },
            ""units"": []
        }";
        File.WriteAllText(Path.Combine(_testUnitsSettingsPath, "test.json"), jsonContent);

        var repository = new JsonUnitRepository(_testUnitsSettingsPath, _mockLogger.Object);
        repository.Initialize();

        // Act
        var category = repository.GetCategoryByNameAsync("test").Result;

        // Assert
        Assert.NotNull(category);
        var baseUnit = category!.BaseUnit;
        Assert.Equal("t", baseUnit.Symbol);
        Assert.Equal("test", baseUnit.Name);
        Assert.Equal("Test", baseUnit.DisplayName);
        Assert.Equal("test", baseUnit.Category);
        Assert.True(baseUnit.IsBaseUnit);
        Assert.True(baseUnit.IsSIUnit);
        Assert.Equal("SI", baseUnit.UnitSystem);
        Assert.Equal(1.0, baseUnit.ConversionFactor);
        Assert.Equal("x + 273.15", baseUnit.ConversionFormula);
        Assert.Equal("x - 273.15", baseUnit.ConversionInverseFormula);
    }

    [Fact]
    public void MapUnitJson_WhenNullCoalescingUsed_MapsToEmptyString()
    {
        // Arrange
        var jsonContent = @"{
            ""category"": ""test"",
            ""categoryDisplayName"": ""Test Category"",
            ""baseUnit"": {
                ""symbol"": null,
                ""name"": null,
                ""displayName"": null,
                ""isBaseUnit"": true,
                ""isSIUnit"": true,
                ""unitSystem"": null,
                ""conversionFactor"": 1.0
            },
            ""units"": []
        }";
        File.WriteAllText(Path.Combine(_testUnitsSettingsPath, "test.json"), jsonContent);

        var repository = new JsonUnitRepository(_testUnitsSettingsPath, _mockLogger.Object);
        repository.Initialize();

        // Act
        var category = repository.GetCategoryByNameAsync("test").Result;

        // Assert
        Assert.NotNull(category);
        var baseUnit = category!.BaseUnit;
        Assert.Equal(string.Empty, baseUnit.Symbol);
        Assert.Equal(string.Empty, baseUnit.Name);
        Assert.Equal(string.Empty, baseUnit.DisplayName);
        Assert.Equal(string.Empty, baseUnit.UnitSystem);
    }

    public void Dispose()
    {
        if (Directory.Exists(_testUnitsSettingsPath))
        {
            Directory.Delete(_testUnitsSettingsPath, true);
        }
    }
}

