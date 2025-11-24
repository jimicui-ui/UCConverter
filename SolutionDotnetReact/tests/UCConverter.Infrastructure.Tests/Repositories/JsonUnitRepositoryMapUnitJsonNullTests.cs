namespace UCConverter.Infrastructure.Tests.Repositories;

using Microsoft.Extensions.Logging;
using Moq;
using System.Reflection;
using UCConverter.Infrastructure.Repositories;
using Xunit;

public class JsonUnitRepositoryMapUnitJsonNullTests : IDisposable
{
    private readonly string _testUnitsSettingsPath;
    private readonly Mock<ILogger<JsonUnitRepository>> _mockLogger;

    public JsonUnitRepositoryMapUnitJsonNullTests()
    {
        _testUnitsSettingsPath = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
        Directory.CreateDirectory(_testUnitsSettingsPath);
        _mockLogger = new Mock<ILogger<JsonUnitRepository>>();
    }

    [Fact]
    public void MapUnitJson_WhenUnitJsonIsNull_ThrowsArgumentNullException()
    {
        // Arrange
        var repository = new JsonUnitRepository(_testUnitsSettingsPath, _mockLogger.Object);
        
        // Use reflection to access the private MapUnitJson method
        var method = typeof(JsonUnitRepository).GetMethod("MapUnitJson", 
            BindingFlags.NonPublic | BindingFlags.Static);
        
        Assert.NotNull(method);

        // Act & Assert
        var exception = Assert.Throws<TargetInvocationException>(() =>
            method!.Invoke(null, new object[] { null!, "test" }));
        
        Assert.IsType<ArgumentNullException>(exception.InnerException);
        Assert.Contains("UnitJson cannot be null", exception.InnerException!.Message);
    }

    public void Dispose()
    {
        if (Directory.Exists(_testUnitsSettingsPath))
        {
            Directory.Delete(_testUnitsSettingsPath, true);
        }
    }
}

