namespace UCConverter.Api.Tests.Controllers;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using UCConverter.Application.DTOs;
using UCConverter.Application.Interfaces;
using UCConverter.Api.Controllers;
using Xunit;

public class CategoriesControllerGenericExceptionTests
{
    private readonly Mock<IUnitConverterService> _mockService;
    private readonly Mock<ILocalizationService> _mockLocalizationService;
    private readonly Mock<ILogger<CategoriesController>> _mockLogger;
    private readonly CategoriesController _controller;

    public CategoriesControllerGenericExceptionTests()
    {
        _mockService = new Mock<IUnitConverterService>();
        _mockLocalizationService = new Mock<ILocalizationService>();
        _mockLogger = new Mock<ILogger<CategoriesController>>();
        _controller = new CategoriesController(_mockService.Object, _mockLocalizationService.Object, _mockLogger.Object);
        
        // Setup default localization behavior
        _mockLocalizationService.Setup(l => l.GetErrorMessage("CategoryNotFound", It.IsAny<object[]>()))
            .Returns<string, object[]>((key, args) => $"Category '{args[0]}' not found");
    }

    [Fact]
    public async Task GetCategories_WhenServiceThrowsException_Returns500StatusCode()
    {
        // Arrange
        _mockService.Setup(s => s.GetAllCategoriesAsync())
            .ThrowsAsync(new InvalidOperationException("Unexpected error"));

        // Act
        var result = await _controller.GetCategories();

        // Assert
        var statusCodeResult = Assert.IsType<ObjectResult>(result.Result);
        Assert.Equal(500, statusCodeResult.StatusCode);
        _mockLogger.Verify(
            x => x.Log(
                LogLevel.Error,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => true),
                It.IsAny<Exception>(),
                It.Is<Func<It.IsAnyType, Exception?, string>>((v, t) => true)),
            Times.Once);
    }

    [Fact]
    public async Task GetUnitsByCategory_WhenServiceThrowsException_Returns500StatusCode()
    {
        // Arrange
        _mockService.Setup(s => s.GetUnitsByCategoryAsync(It.IsAny<string>()))
            .ThrowsAsync(new InvalidOperationException("Unexpected error"));

        // Act
        var result = await _controller.GetUnitsByCategory("length");

        // Assert
        var statusCodeResult = Assert.IsType<ObjectResult>(result.Result);
        Assert.Equal(500, statusCodeResult.StatusCode);
        _mockLogger.Verify(
            x => x.Log(
                LogLevel.Error,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => true),
                It.IsAny<Exception>(),
                It.Is<Func<It.IsAnyType, Exception?, string>>((v, t) => true)),
            Times.Once);
    }
}

