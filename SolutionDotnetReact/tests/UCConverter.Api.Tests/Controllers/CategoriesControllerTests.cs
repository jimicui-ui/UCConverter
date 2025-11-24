namespace UCConverter.Api.Tests.Controllers;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using UCConverter.Api.Controllers;
using UCConverter.Application.DTOs;
using UCConverter.Application.Interfaces;
using Xunit;

public class CategoriesControllerTests
{
    private readonly Mock<IUnitConverterService> _mockService;
    private readonly Mock<ILocalizationService> _mockLocalizationService;
    private readonly Mock<ILogger<CategoriesController>> _mockLogger;
    private readonly CategoriesController _controller;

    public CategoriesControllerTests()
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
    public async Task GetCategories_WhenValid_ReturnsOkResult()
    {
        // Arrange
        var categories = new List<CategoryDto>
        {
            new CategoryDto { Name = "length", DisplayName = "Length" },
            new CategoryDto { Name = "weight", DisplayName = "Weight" }
        };

        _mockService.Setup(s => s.GetAllCategoriesAsync())
            .ReturnsAsync(categories);

        // Act
        var result = await _controller.GetCategories();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var returnedCategories = Assert.IsAssignableFrom<IEnumerable<CategoryDto>>(okResult.Value);
        Assert.Equal(2, returnedCategories.Count());
    }

    [Fact]
    public async Task GetCategories_WhenException_ReturnsInternalServerError()
    {
        // Arrange
        _mockService.Setup(s => s.GetAllCategoriesAsync())
            .ThrowsAsync(new Exception("Test exception"));

        // Act
        var result = await _controller.GetCategories();

        // Assert
        var statusResult = Assert.IsType<ObjectResult>(result.Result);
        Assert.Equal(500, statusResult.StatusCode);
    }

    [Fact]
    public async Task GetUnitsByCategory_WhenValid_ReturnsOkResult()
    {
        // Arrange
        var units = new List<UnitDto>
        {
            new UnitDto { Symbol = "m", Name = "meter", DisplayName = "Meter", IsBaseUnit = true, IsSIUnit = true, UnitSystem = "SI", ConversionFactor = 1.0 },
            new UnitDto { Symbol = "km", Name = "kilometer", DisplayName = "Kilometer", IsBaseUnit = false, IsSIUnit = true, UnitSystem = "SI", ConversionFactor = 1000.0 }
        };

        _mockService.Setup(s => s.GetUnitsByCategoryAsync("length"))
            .ReturnsAsync(units);

        // Act
        var result = await _controller.GetUnitsByCategory("length");

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var returnedUnits = Assert.IsAssignableFrom<IEnumerable<UnitDto>>(okResult.Value);
        Assert.Equal(2, returnedUnits.Count());
    }

    [Fact]
    public async Task GetUnitsByCategory_WhenNoUnits_ReturnsNotFound()
    {
        // Arrange
        _mockService.Setup(s => s.GetUnitsByCategoryAsync("nonexistent"))
            .ReturnsAsync(Enumerable.Empty<UnitDto>());
        
        _mockLocalizationService.Setup(l => l.GetErrorMessage("CategoryNotFound", It.IsAny<object[]>()))
            .Returns<string, object[]>((key, args) => $"Category '{args[0]}' not found");

        // Act
        var result = await _controller.GetUnitsByCategory("nonexistent");

        // Assert
        var notFoundResult = Assert.IsType<NotFoundObjectResult>(result.Result);
        Assert.NotNull(notFoundResult.Value);
    }

    [Fact]
    public async Task GetUnitsByCategory_WhenException_ReturnsInternalServerError()
    {
        // Arrange
        _mockService.Setup(s => s.GetUnitsByCategoryAsync("length"))
            .ThrowsAsync(new Exception("Test exception"));

        // Act
        var result = await _controller.GetUnitsByCategory("length");

        // Assert
        var statusResult = Assert.IsType<ObjectResult>(result.Result);
        Assert.Equal(500, statusResult.StatusCode);
    }
}

