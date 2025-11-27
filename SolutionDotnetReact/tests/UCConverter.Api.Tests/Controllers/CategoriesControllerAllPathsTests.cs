namespace UCConverter.Api.Tests.Controllers;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using UCConverter.Api.Controllers;
using UCConverter.Application.DTOs;
using UCConverter.Application.Interfaces;
using Xunit;

/// <summary>
/// Tests to cover all code paths in CategoriesController
/// </summary>
public class CategoriesControllerAllPathsTests
{
    private readonly Mock<IUnitConverterService> _mockService;
    private readonly Mock<ILocalizationService> _mockLocalizationService;
    private readonly Mock<ILogger<CategoriesController>> _mockLogger;
    private readonly CategoriesController _controller;

    public CategoriesControllerAllPathsTests()
    {
        _mockService = new Mock<IUnitConverterService>();
        _mockLocalizationService = new Mock<ILocalizationService>();
        _mockLogger = new Mock<ILogger<CategoriesController>>();
        _controller = new CategoriesController(_mockService.Object, _mockLocalizationService.Object, _mockLogger.Object);

        _mockLocalizationService.Setup(l => l.GetErrorMessage(It.IsAny<string>(), It.IsAny<object[]>()))
            .Returns<string, object[]>((key, args) =>
            {
                return key switch
                {
                    "CategoryNotFound" => $"Category '{args[0]}' not found",
                    "InternalServerErrorCategories" => "An error occurred while retrieving categories",
                    "InternalServerErrorUnits" => "An error occurred while retrieving units",
                    _ => "An error occurred"
                };
            });
    }

    [Fact]
    public async Task GetCategories_WhenSuccess_ReturnsOkWithCategories()
    {
        // Arrange
        var categories = new List<CategoryDto>
        {
            new CategoryDto { Name = "length", DisplayName = "Length", Group = "Common" }
        };

        _mockService.Setup(s => s.GetAllCategoriesAsync())
            .ReturnsAsync(categories);

        // Act
        var result = await _controller.GetCategories();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var returnedCategories = Assert.IsAssignableFrom<IEnumerable<CategoryDto>>(okResult.Value);
        Assert.Single(returnedCategories);
        _mockService.Verify(s => s.GetAllCategoriesAsync(), Times.Once);
    }

    [Fact]
    public async Task GetCategories_WhenException_LogsError()
    {
        // Arrange
        var exception = new Exception("Test exception");
        _mockService.Setup(s => s.GetAllCategoriesAsync())
            .ThrowsAsync(exception);

        // Act
        await _controller.GetCategories();

        // Assert
        _mockLogger.Verify(
            x => x.Log(
                LogLevel.Error,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => v.ToString()!.Contains("Error retrieving categories")),
                It.Is<Exception>(e => e == exception),
                It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
            Times.Once);
    }

    [Fact]
    public async Task GetUnitsByCategory_WhenSuccess_ReturnsOkWithUnits()
    {
        // Arrange
        var units = new List<UnitDto>
        {
            new UnitDto { Symbol = "m", Name = "meter", DisplayName = "Meter", IsBaseUnit = true, IsSIUnit = true, UnitSystem = "SI", ConversionFactor = 1.0 }
        };

        _mockService.Setup(s => s.GetUnitsByCategoryAsync("length"))
            .ReturnsAsync(units);

        // Act
        var result = await _controller.GetUnitsByCategory("length");

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var returnedUnits = Assert.IsAssignableFrom<IEnumerable<UnitDto>>(okResult.Value);
        Assert.Single(returnedUnits);
        _mockService.Verify(s => s.GetUnitsByCategoryAsync("length"), Times.Once);
    }

    [Fact]
    public async Task GetUnitsByCategory_WhenNoUnits_ReturnsNotFound()
    {
        // Arrange
        _mockService.Setup(s => s.GetUnitsByCategoryAsync("nonexistent"))
            .ReturnsAsync(Enumerable.Empty<UnitDto>());

        // Act
        var result = await _controller.GetUnitsByCategory("nonexistent");

        // Assert
        var notFoundResult = Assert.IsType<NotFoundObjectResult>(result.Result);
        Assert.NotNull(notFoundResult.Value);
    }

    [Fact]
    public async Task GetUnitsByCategory_WhenException_LogsErrorWithCategoryName()
    {
        // Arrange
        var exception = new Exception("Test exception");
        _mockService.Setup(s => s.GetUnitsByCategoryAsync("length"))
            .ThrowsAsync(exception);

        // Act
        await _controller.GetUnitsByCategory("length");

        // Assert
        _mockLogger.Verify(
            x => x.Log(
                LogLevel.Error,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => v.ToString()!.Contains("Error retrieving units for category")),
                It.Is<Exception>(e => e == exception),
                It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
            Times.Once);
    }
}

