namespace UCConverter.Api.Tests.Controllers;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using UCConverter.Api.Controllers;
using UCConverter.Application.DTOs;
using UCConverter.Application.Interfaces;
using Xunit;

public class CategoriesControllerAdditionalTests
{
    private readonly Mock<IUnitConverterService> _mockService;
    private readonly Mock<ILocalizationService> _mockLocalizationService;
    private readonly Mock<ILogger<CategoriesController>> _mockLogger;
    private readonly CategoriesController _controller;

    public CategoriesControllerAdditionalTests()
    {
        _mockService = new Mock<IUnitConverterService>();
        _mockLocalizationService = new Mock<ILocalizationService>();
        _mockLogger = new Mock<ILogger<CategoriesController>>();
        _controller = new CategoriesController(_mockService.Object, _mockLocalizationService.Object, _mockLogger.Object);
    }

    [Fact]
    public void Constructor_WhenUnitConverterServiceIsNull_ThrowsArgumentNullException()
    {
        // Act & Assert
        Assert.Throws<ArgumentNullException>(() =>
            new CategoriesController(null!, _mockLocalizationService.Object, _mockLogger.Object));
    }

    [Fact]
    public void Constructor_WhenLocalizationServiceIsNull_ThrowsArgumentNullException()
    {
        // Act & Assert
        Assert.Throws<ArgumentNullException>(() =>
            new CategoriesController(_mockService.Object, null!, _mockLogger.Object));
    }

    [Fact]
    public void Constructor_WhenLoggerIsNull_ThrowsArgumentNullException()
    {
        // Act & Assert
        Assert.Throws<ArgumentNullException>(() =>
            new CategoriesController(_mockService.Object, _mockLocalizationService.Object, null!));
    }

    [Fact]
    public async Task GetUnitsByCategory_WhenUnitsListIsEmpty_ReturnsNotFound()
    {
        // Arrange
        _mockService.Setup(s => s.GetUnitsByCategoryAsync("nonexistent"))
            .ReturnsAsync(Enumerable.Empty<UnitDto>());
        _mockLocalizationService.Setup(s => s.GetErrorMessage("CategoryNotFound", "nonexistent"))
            .Returns("Category 'nonexistent' not found");

        // Act
        var result = await _controller.GetUnitsByCategory("nonexistent");

        // Assert
        var notFoundResult = Assert.IsType<NotFoundObjectResult>(result.Result);
        _mockLocalizationService.Verify(s => s.GetErrorMessage("CategoryNotFound", "nonexistent"), Times.Once);
    }

    [Fact]
    public async Task GetUnitsByCategory_WhenGenericException_ReturnsInternalServerError()
    {
        // Arrange
        _mockService.Setup(s => s.GetUnitsByCategoryAsync("test"))
            .ThrowsAsync(new Exception("Unexpected error"));
        _mockLocalizationService.Setup(s => s.GetErrorMessage("InternalServerErrorUnits"))
            .Returns("An error occurred while retrieving units");

        // Act
        var result = await _controller.GetUnitsByCategory("test");

        // Assert
        var statusCodeResult = Assert.IsType<ObjectResult>(result.Result);
        Assert.Equal(500, statusCodeResult.StatusCode);
        _mockLocalizationService.Verify(s => s.GetErrorMessage("InternalServerErrorUnits"), Times.Once);
    }

    [Fact]
    public async Task GetCategories_WhenGenericException_ReturnsInternalServerError()
    {
        // Arrange
        _mockService.Setup(s => s.GetAllCategoriesAsync())
            .ThrowsAsync(new Exception("Unexpected error"));
        _mockLocalizationService.Setup(s => s.GetErrorMessage("InternalServerErrorCategories"))
            .Returns("An error occurred while retrieving categories");

        // Act
        var result = await _controller.GetCategories();

        // Assert
        var statusCodeResult = Assert.IsType<ObjectResult>(result.Result);
        Assert.Equal(500, statusCodeResult.StatusCode);
        _mockLocalizationService.Verify(s => s.GetErrorMessage("InternalServerErrorCategories"), Times.Once);
    }
}

