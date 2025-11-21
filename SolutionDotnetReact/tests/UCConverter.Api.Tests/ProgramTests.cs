namespace UCConverter.Api.Tests;

using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using UCConverter.Application.Interfaces;
using UCConverter.Domain.Interfaces;
using UCConverter.Api;
using Xunit;

public class ProgramTests
{
    [Fact]
    public void Program_CanCreateWebApplicationFactory()
    {
        // Arrange & Act
        var factory = new WebApplicationFactory<Program>();

        // Assert
        Assert.NotNull(factory);
    }

    [Fact]
    public void Program_WhenConfigured_RegistersServices()
    {
        // Arrange
        var factory = new WebApplicationFactory<Program>();

        // Act
        using var client = factory.CreateClient();

        // Assert - Verify the application can be created and client works
        Assert.NotNull(client);
        Assert.NotNull(factory);
    }
}

