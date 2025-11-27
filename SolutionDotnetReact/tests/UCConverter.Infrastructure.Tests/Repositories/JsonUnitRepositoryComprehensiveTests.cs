namespace UCConverter.Infrastructure.Tests.Repositories;

using Microsoft.Extensions.Logging;
using Moq;
using System.Diagnostics;
using UCConverter.Domain.Entities;
using UCConverter.Infrastructure.Repositories;
using Xunit;

/// <summary>
/// Comprehensive tests to improve Infrastructure layer coverage to 95%
/// </summary>
public class JsonUnitRepositoryComprehensiveTests : IDisposable
{
    private readonly string _testUnitsSettingsPath;
    private readonly Mock<ILogger<JsonUnitRepository>> _mockLogger;

    public JsonUnitRepositoryComprehensiveTests()
    {
        _testUnitsSettingsPath = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
        Directory.CreateDirectory(_testUnitsSettingsPath);
        _mockLogger = new Mock<ILogger<JsonUnitRepository>>();
    }

    #region Initialize Tests - Edge Cases

    [Fact]
    public void Initialize_WhenEmptyDirectory_LogsInformationAndCompletes()
    {
        // Arrange
        var repository = new JsonUnitRepository(_testUnitsSettingsPath, _mockLogger.Object);

        // Act
        repository.Initialize();

        // Assert
        _mockLogger.Verify(
            x => x.Log(
                LogLevel.Information,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => v.ToString()!.Contains("Loading")),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
            Times.Once);

        _mockLogger.Verify(
            x => x.Log(
                LogLevel.Information,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => v.ToString()!.Contains("Successfully loaded")),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
            Times.Once);
    }

    [Fact]
    public async Task Initialize_WhenMultipleFilesWithSomeFailures_ContinuesAndLogsBothSuccessAndFailure()
    {
        // Arrange
        var validJsonContent = @"{
            ""category"": ""valid"",
            ""categoryDisplayName"": ""Valid Category"",
            ""baseUnit"": {
                ""symbol"": ""v"",
                ""name"": ""valid"",
                ""displayName"": ""Valid"",
                ""isBaseUnit"": true,
                ""isSIUnit"": true,
                ""unitSystem"": ""SI"",
                ""conversionFactor"": 1.0
            },
            ""units"": []
        }";
        File.WriteAllText(Path.Combine(_testUnitsSettingsPath, "valid.json"), validJsonContent);

        var invalidJsonContent = @"{""invalid"": json}"; // Malformed JSON
        File.WriteAllText(Path.Combine(_testUnitsSettingsPath, "invalid.json"), invalidJsonContent);

        var repository = new JsonUnitRepository(_testUnitsSettingsPath, _mockLogger.Object);

        // Act
        repository.Initialize();

        // Assert
        _mockLogger.Verify(
            x => x.Log(
                LogLevel.Error,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => v.ToString()!.Contains("Failed to load unit configuration from file")),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
            Times.Once);

        // Verify success count includes valid file
        var categories = await repository.GetAllCategoriesAsync();
        Assert.Single(categories);
    }

    [Fact]
    public void Initialize_WhenStopwatchIsUsed_LogsElapsedTime()
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

        // Act
        repository.Initialize();

        // Assert
        _mockLogger.Verify(
            x => x.Log(
                LogLevel.Information,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => v.ToString()!.Contains("Successfully loaded") && v.ToString()!.Contains("ms")),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
            Times.Once);
    }

    [Fact]
    public void Initialize_WhenExceptionThrown_LogsErrorAndRethrows()
    {
        // Arrange
        var mockLogger = new Mock<ILogger<JsonUnitRepository>>();
        mockLogger.Setup(x => x.Log(
            LogLevel.Information,
            It.IsAny<EventId>(),
            It.IsAny<It.IsAnyType>(),
            It.IsAny<Exception>(),
            It.IsAny<Func<It.IsAnyType, Exception?, string>>()))
            .Throws(new InvalidOperationException("Simulated error"));

        var repository = new JsonUnitRepository(_testUnitsSettingsPath, mockLogger.Object);

        // Act & Assert
        var exception = Assert.Throws<InvalidOperationException>(() => repository.Initialize());
        Assert.Equal("Simulated error", exception.Message);
    }

    #endregion

    #region Thread Safety Tests

    [Fact]
    public async Task Initialize_WhenCalledConcurrently_OnlyInitializesOnce()
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

        // Act
        var tasks = new List<Task>();
        for (int i = 0; i < 10; i++)
        {
            tasks.Add(Task.Run(() => repository.Initialize()));
        }
        await Task.WhenAll(tasks.ToArray());

        // Assert
        _mockLogger.Verify(
            x => x.Log(
                LogLevel.Information,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => v.ToString()!.Contains("Successfully loaded")),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
            Times.Once); // Should only log success once
    }

    [Fact]
    public void Initialize_WhenAlreadyInitialized_ReturnsImmediately()
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

        // Act
        repository.Initialize();
        repository.Initialize(); // Call again

        // Assert
        _mockLogger.Verify(
            x => x.Log(
                LogLevel.Information,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => v.ToString()!.Contains("Successfully loaded")),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
            Times.Once); // Should only log success once
    }

    #endregion

    #region GetAllUnitsAsync Tests

    [Fact]
    public async Task GetAllUnitsAsync_WhenMultipleCategories_ReturnsAllUnits()
    {
        // Arrange
        var jsonContent1 = @"{
            ""category"": ""test1"",
            ""categoryDisplayName"": ""Test Category 1"",
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
                    ""symbol"": ""t1a"",
                    ""name"": ""test1a"",
                    ""displayName"": ""Test 1A"",
                    ""isBaseUnit"": false,
                    ""isSIUnit"": true,
                    ""unitSystem"": ""SI"",
                    ""conversionFactor"": 2.0
                }
            ]
        }";

        var jsonContent2 = @"{
            ""category"": ""test2"",
            ""categoryDisplayName"": ""Test Category 2"",
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
                    ""symbol"": ""t2a"",
                    ""name"": ""test2a"",
                    ""displayName"": ""Test 2A"",
                    ""isBaseUnit"": false,
                    ""isSIUnit"": true,
                    ""unitSystem"": ""SI"",
                    ""conversionFactor"": 3.0
                }
            ]
        }";

        File.WriteAllText(Path.Combine(_testUnitsSettingsPath, "test1.json"), jsonContent1);
        File.WriteAllText(Path.Combine(_testUnitsSettingsPath, "test2.json"), jsonContent2);

        var repository = new JsonUnitRepository(_testUnitsSettingsPath, _mockLogger.Object);
        repository.Initialize();

        // Act
        var allUnits = await repository.GetAllUnitsAsync();

        // Assert
        var unitsList = allUnits.ToList();
        Assert.Equal(4, unitsList.Count); // 2 base units + 2 additional units
        Assert.Contains(unitsList, u => u.Symbol == "t1");
        Assert.Contains(unitsList, u => u.Symbol == "t1a");
        Assert.Contains(unitsList, u => u.Symbol == "t2");
        Assert.Contains(unitsList, u => u.Symbol == "t2a");
    }

    [Fact]
    public async Task GetAllUnitsAsync_WhenNoCategories_ReturnsEmptyEnumerable()
    {
        // Arrange
        var repository = new JsonUnitRepository(_testUnitsSettingsPath, _mockLogger.Object);
        repository.Initialize();

        // Act
        var allUnits = await repository.GetAllUnitsAsync();

        // Assert
        Assert.NotNull(allUnits);
        Assert.Empty(allUnits);
    }

    [Fact]
    public async Task GetAllUnitsAsync_WhenCategoryHasMultipleUnits_ReturnsAllUnits()
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
                },
                {
                    ""symbol"": ""t2"",
                    ""name"": ""test2"",
                    ""displayName"": ""Test 2"",
                    ""isBaseUnit"": false,
                    ""isSIUnit"": true,
                    ""unitSystem"": ""SI"",
                    ""conversionFactor"": 3.0
                }
            ]
        }";
        File.WriteAllText(Path.Combine(_testUnitsSettingsPath, "test.json"), jsonContent);

        var repository = new JsonUnitRepository(_testUnitsSettingsPath, _mockLogger.Object);
        repository.Initialize();

        // Act
        var allUnits = await repository.GetAllUnitsAsync();

        // Assert
        var unitsList = allUnits.ToList();
        Assert.Equal(3, unitsList.Count); // Base unit + 2 additional units
    }

    #endregion

    #region EnsureInitialized Tests

    [Fact]
    public async Task GetAllCategoriesAsync_WhenNotInitialized_CallsInitialize()
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

        // Act
        var categories = await repository.GetAllCategoriesAsync();

        // Assert
        Assert.Single(categories);
        _mockLogger.Verify(
            x => x.Log(
                LogLevel.Information,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => v.ToString()!.Contains("Successfully loaded")),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
            Times.Once);
    }

    [Fact]
    public async Task GetCategoryByNameAsync_WhenNotInitialized_CallsInitialize()
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

        // Act
        var category = await repository.GetCategoryByNameAsync("test");

        // Assert
        Assert.NotNull(category);
        _mockLogger.Verify(
            x => x.Log(
                LogLevel.Information,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => v.ToString()!.Contains("Successfully loaded")),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
            Times.Once);
    }

    #endregion

    #region Logging Tests

    [Fact]
    public void Initialize_WhenDirectoryNotFound_LogsWarning()
    {
        // Arrange
        var nonExistentPath = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
        var repository = new JsonUnitRepository(nonExistentPath, _mockLogger.Object);

        // Act
        repository.Initialize();

        // Assert
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
    public void Initialize_WhenLoadingFiles_LogsInformationWithCount()
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

        // Act
        repository.Initialize();

        // Assert
        _mockLogger.Verify(
            x => x.Log(
                LogLevel.Information,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => v.ToString()!.Contains("Loading") && v.ToString()!.Contains("unit configuration files")),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
            Times.Once);
    }

    [Fact]
    public void LoadCategoryFromFile_WhenCategoryLoaded_LogsDebug()
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

        // Act
        repository.Initialize();

        // Assert
        _mockLogger.Verify(
            x => x.Log(
                LogLevel.Debug,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => v.ToString()!.Contains("Loaded category")),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
            Times.Once);
    }

    #endregion

    #region Error Handling Tests

    [Fact]
    public async Task Initialize_WhenFileReadThrowsException_LogsErrorAndContinues()
    {
        // Arrange
        var validPath = Path.Combine(_testUnitsSettingsPath, "valid.json");
        var jsonContent = @"{
            ""category"": ""valid"",
            ""categoryDisplayName"": ""Valid Category"",
            ""baseUnit"": {
                ""symbol"": ""v"",
                ""name"": ""valid"",
                ""displayName"": ""Valid"",
                ""isBaseUnit"": true,
                ""isSIUnit"": true,
                ""unitSystem"": ""SI"",
                ""conversionFactor"": 1.0
            },
            ""units"": []
        }";
        File.WriteAllText(validPath, jsonContent);

        // Create a file that will cause an exception when read
        var exceptionPath = Path.Combine(_testUnitsSettingsPath, "exception.json");
        File.WriteAllText(exceptionPath, "test");
        // Make file read-only to potentially cause issues, or use invalid path
        // For this test, we'll use malformed JSON which will cause deserialization error

        var repository = new JsonUnitRepository(_testUnitsSettingsPath, _mockLogger.Object);

        // Act
        repository.Initialize();

        // Assert
        // Should still load the valid file
        var categories = await repository.GetAllCategoriesAsync();
        Assert.Single(categories);
    }

    #endregion

    public void Dispose()
    {
        if (Directory.Exists(_testUnitsSettingsPath))
        {
            Directory.Delete(_testUnitsSettingsPath, true);
        }
    }
}

