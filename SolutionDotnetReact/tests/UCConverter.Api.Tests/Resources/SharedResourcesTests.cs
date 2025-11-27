namespace UCConverter.Api.Tests.Resources;

using UCConverter.Api.Resources;
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

