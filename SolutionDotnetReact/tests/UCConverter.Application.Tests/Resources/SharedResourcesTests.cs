namespace UCConverter.Application.Tests.Resources;

using UCConverter.Application.Resources;
using Xunit;

/// <summary>
/// Tests for SharedResources class to ensure code coverage
/// </summary>
public class SharedResourcesTests
{
    [Fact]
    public void SharedResources_CanBeInstantiated()
    {
        // Act
        var resources = new SharedResources();

        // Assert
        Assert.NotNull(resources);
    }
}

