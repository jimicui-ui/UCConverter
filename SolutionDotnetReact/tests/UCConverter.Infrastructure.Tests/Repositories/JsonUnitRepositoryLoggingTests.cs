namespace UCConverter.Infrastructure.Tests.Repositories;

using Microsoft.Extensions.Logging;
using Moq;
using UCConverter.Infrastructure.Repositories;
using Xunit;

/// <summary>
/// Tests to ensure all logging paths in JsonUnitRepository are covered
/// </summary>
public class JsonUnitRepositoryLoggingTests : IDisposable
{
    private readonly string _testUnitsSettingsPath;
    private readonly Mock<ILogger<JsonUnitRepository>> _mockLogger;

    public JsonUnitRepositoryLoggingTests()
    {
        _testUnitsSettingsPath = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
        Directory.CreateDirectory(_testUnitsSettingsPath);
        _mockLogger = new Mock<ILogger<JsonUnitRepository>>();
    }

    [Fact]
    public void Initialize_WhenDirectoryNotFound_LogsWarningWithPath()
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
    public void Initialize_WhenLoadingFiles_LogsInformationWithFileCount()
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
    public void Initialize_WhenSuccessfullyLoaded_LogsInformationWithStats()
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
                    v.ToString()!.Contains("categories") && 
                    v.ToString()!.Contains("ms")),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
            Times.Once);
    }

    [Fact]
    public void Initialize_WhenFileLoadFails_LogsError()
    {
        // Arrange
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

        // Act
        repository.Initialize();

        // Assert
        _mockLogger.Verify(
            x => x.Log(
                LogLevel.Debug,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => v.ToString()!.Contains("Loaded category") && 
                    v.ToString()!.Contains("test") &&
                    v.ToString()!.Contains("units")),
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

        mockLogger.Verify(
            x => x.Log(
                LogLevel.Error,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => v.ToString()!.Contains("Error initializing JsonUnitRepository")),
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

