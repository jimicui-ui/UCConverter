namespace UCConverter.Infrastructure.Tests.Repositories;

using Microsoft.Extensions.Logging;
using Moq;
using System.Threading.Tasks;
using UCConverter.Infrastructure.Repositories;
using Xunit;

/// <summary>
/// Tests to cover all initialization paths and thread safety
/// </summary>
public class JsonUnitRepositoryInitializationPathsTests : IDisposable
{
    private readonly string _testUnitsSettingsPath;
    private readonly Mock<ILogger<JsonUnitRepository>> _mockLogger;

    public JsonUnitRepositoryInitializationPathsTests()
    {
        _testUnitsSettingsPath = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
        Directory.CreateDirectory(_testUnitsSettingsPath);
        _mockLogger = new Mock<ILogger<JsonUnitRepository>>();
    }

    [Fact]
    public void Initialize_WhenAlreadyInitialized_ReturnsEarlyWithoutLock()
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
        repository.Initialize(); // Second call should return early

        // Assert
        // Should only log success once
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
    public async Task Initialize_WhenLockedAndAlreadyInitialized_ReturnsEarly()
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

        // Act - Initialize first time
        repository.Initialize();

        // Simulate concurrent access - second thread checking after lock
        var task1 = Task.Run(() => repository.Initialize());
        var task2 = Task.Run(() => repository.Initialize());
        await Task.WhenAll(task1, task2);

        // Assert
        // Should only log success once total
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
    public async Task Initialize_WhenMultipleFiles_ProcessesAllFiles()
    {
        // Arrange
        for (int i = 1; i <= 5; i++)
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

        // Act
        repository.Initialize();

        // Assert
        var categories = await repository.GetAllCategoriesAsync();
        Assert.Equal(5, categories.Count());
    }

    [Fact]
    public async Task Initialize_WhenFileReadThrowsException_ContinuesWithOtherFiles()
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

        // Create a file with invalid JSON that will cause deserialization error
        var invalidJsonContent = @"{""invalid"": json syntax error}";
        File.WriteAllText(Path.Combine(_testUnitsSettingsPath, "invalid.json"), invalidJsonContent);

        var repository = new JsonUnitRepository(_testUnitsSettingsPath, _mockLogger.Object);

        // Act
        repository.Initialize();

        // Assert
        var categories = await repository.GetAllCategoriesAsync();
        Assert.Single(categories); // Should still load valid file

        _mockLogger.Verify(
            x => x.Log(
                LogLevel.Error,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => v.ToString()!.Contains("Failed to load unit configuration from file")),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
            Times.Once);
    }

    [Fact]
    public async Task EnsureInitialized_WhenCalledFromGetAllCategoriesAsync_InitializesRepository()
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

        // Act - Don't call Initialize() explicitly
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
    public async Task EnsureInitialized_WhenCalledFromGetCategoryByNameAsync_InitializesRepository()
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

        // Act - Don't call Initialize() explicitly
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

    [Fact]
    public void Initialize_WhenSuccessAndFailureCounts_LogsCorrectStats()
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

        var invalidJsonContent = @"{""invalid"": json}";
        File.WriteAllText(Path.Combine(_testUnitsSettingsPath, "invalid.json"), invalidJsonContent);

        var repository = new JsonUnitRepository(_testUnitsSettingsPath, _mockLogger.Object);

        // Act
        repository.Initialize();

        // Assert
        _mockLogger.Verify(
            x => x.Log(
                LogLevel.Information,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => v.ToString()!.Contains("Successfully loaded") && 
                    v.ToString()!.Contains("Success:") && 
                    v.ToString()!.Contains("Failed:")),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
            Times.Once);
    }

    [Fact]
    public void Initialize_WhenStopwatchElapsed_LogsElapsedMilliseconds()
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
                It.Is<It.IsAnyType>((v, t) => v.ToString()!.Contains("Successfully loaded") && 
                    v.ToString()!.Contains("ms")),
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

