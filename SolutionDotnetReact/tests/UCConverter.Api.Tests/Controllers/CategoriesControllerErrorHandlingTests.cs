namespace UCConverter.Api.Tests.Controllers;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using UCConverter.Api.Controllers;
using UCConverter.Application.Interfaces;
using Xunit;

public class CategoriesControllerErrorHandlingTests
{
    private readonly Mock<IUnitConverterService> _mockService;
    private readonly Mock<ILogger<CategoriesController>> _mockLogger;
    private readonly CategoriesController _controller;

    public CategoriesControllerErrorHandlingTests()
    {
        _mockService = new Mock<IUnitConverterService>();
        _mockLogger = new Mock<ILogger<CategoriesController>>();
        _controller = new CategoriesController(_mockService.Object, _mockLogger.Object);
    }

    [Fact]
    public async Task GetCategories_WhenExceptionOccurs_ReturnsInternalServerError()
    {
        // Arrange
        _mockService.Setup(s => s.GetAllCategoriesAsync())
            .ThrowsAsync(new Exception("Database error"));

        // Act
        var result = await _controller.GetCategories();

        // Assert
        var statusCodeResult = Assert.IsType<ObjectResult>(result.Result);
        Assert.Equal(500, statusCodeResult.StatusCode);
    }

    [Fact]
    public async Task GetUnitsByCategory_WhenExceptionOccurs_ReturnsInternalServerError()
    {
        // Arrange
        _mockService.Setup(s => s.GetUnitsByCategoryAsync("test"))
            .ThrowsAsync(new Exception("Database error"));

        // Act
        var result = await _controller.GetUnitsByCategory("test");

        // Assert
        var statusCodeResult = Assert.IsType<ObjectResult>(result.Result);
        Assert.Equal(500, statusCodeResult.StatusCode);
    }
}

