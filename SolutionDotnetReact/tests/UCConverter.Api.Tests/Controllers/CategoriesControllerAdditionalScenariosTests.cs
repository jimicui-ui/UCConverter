namespace UCConverter.Api.Tests.Controllers;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using UCConverter.Api.Controllers;
using UCConverter.Application.DTOs;
using UCConverter.Application.Interfaces;
using Xunit;

/// <summary>
/// Additional scenario tests for CategoriesController to improve coverage
/// </summary>
public class CategoriesControllerAdditionalScenariosTests
{
    private readonly Mock<IUnitConverterService> _mockService;
    private readonly Mock<ILocalizationService> _mockLocalizationService;
    private readonly Mock<ILogger<CategoriesController>> _mockLogger;
    private readonly CategoriesController _controller;

    public CategoriesControllerAdditionalScenariosTests()
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
    public async Task GetCategories_WhenMultipleCategories_ReturnsAll()
    {
        // Arrange
        var categories = new List<CategoryDto>
        {
            new CategoryDto { Name = "length", DisplayName = "Length", Group = "Common" },
            new CategoryDto { Name = "weight", DisplayName = "Weight", Group = "Common" },
            new CategoryDto { Name = "temperature", DisplayName = "Temperature", Group = "Common" },
            new CategoryDto { Name = "acceleration", DisplayName = "Acceleration", Group = "Engineering" },
            new CategoryDto { Name = "current", DisplayName = "Current", Group = "Electricity" }
        };

        _mockService.Setup(s => s.GetAllCategoriesAsync())
            .ReturnsAsync(categories);

        // Act
        var result = await _controller.GetCategories();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var returnedCategories = Assert.IsAssignableFrom<IEnumerable<CategoryDto>>(okResult.Value);
        Assert.Equal(5, returnedCategories.Count());
    }

    [Fact]
    public async Task GetCategories_WhenLocaleIsZh_ReturnsOk()
    {
        // Arrange
        var categories = new List<CategoryDto>
        {
            new CategoryDto { Name = "length", DisplayName = "长度", Group = "Common" }
        };

        _mockService.Setup(s => s.GetAllCategoriesAsync())
            .ReturnsAsync(categories);

        // Act
        var result = await _controller.GetCategories("zh");

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        Assert.NotNull(okResult.Value);
    }

    [Fact]
    public async Task GetCategories_WhenLocaleIsFr_ReturnsOk()
    {
        // Arrange
        var categories = new List<CategoryDto>
        {
            new CategoryDto { Name = "length", DisplayName = "Longueur", Group = "Common" }
        };

        _mockService.Setup(s => s.GetAllCategoriesAsync())
            .ReturnsAsync(categories);

        // Act
        var result = await _controller.GetCategories("fr");

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        Assert.NotNull(okResult.Value);
    }

    [Fact]
    public async Task GetCategories_WhenNullReferenceException_ReturnsInternalServerError()
    {
        // Arrange
        var exception = new NullReferenceException("Null reference");
        _mockService.Setup(s => s.GetAllCategoriesAsync())
            .ThrowsAsync(exception);

        // Act
        var result = await _controller.GetCategories();

        // Assert
        var statusResult = Assert.IsType<ObjectResult>(result.Result);
        Assert.Equal(500, statusResult.StatusCode);
    }

    [Fact]
    public async Task GetCategories_WhenArgumentException_ReturnsInternalServerError()
    {
        // Arrange
        var exception = new ArgumentException("Invalid argument");
        _mockService.Setup(s => s.GetAllCategoriesAsync())
            .ThrowsAsync(exception);

        // Act
        var result = await _controller.GetCategories();

        // Assert
        var statusResult = Assert.IsType<ObjectResult>(result.Result);
        Assert.Equal(500, statusResult.StatusCode);
    }

    [Fact]
    public async Task GetUnitsByCategory_WhenMultipleUnits_ReturnsAll()
    {
        // Arrange
        var units = new List<UnitDto>
        {
            new UnitDto { Symbol = "m", Name = "meter", DisplayName = "Meter", IsBaseUnit = true, IsSIUnit = true, UnitSystem = "SI", ConversionFactor = 1.0 },
            new UnitDto { Symbol = "km", Name = "kilometer", DisplayName = "Kilometer", IsBaseUnit = false, IsSIUnit = true, UnitSystem = "SI", ConversionFactor = 1000.0 },
            new UnitDto { Symbol = "ft", Name = "foot", DisplayName = "Foot", IsBaseUnit = false, IsSIUnit = false, UnitSystem = "Imperial", ConversionFactor = 0.3048 },
            new UnitDto { Symbol = "in", Name = "inch", DisplayName = "Inch", IsBaseUnit = false, IsSIUnit = false, UnitSystem = "Imperial", ConversionFactor = 0.0254 }
        };

        _mockService.Setup(s => s.GetUnitsByCategoryAsync("length"))
            .ReturnsAsync(units);

        // Act
        var result = await _controller.GetUnitsByCategory("length");

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var returnedUnits = Assert.IsAssignableFrom<IEnumerable<UnitDto>>(okResult.Value);
        Assert.Equal(4, returnedUnits.Count());
    }

    [Fact]
    public async Task GetUnitsByCategory_WhenSingleUnit_ReturnsOk()
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
    }

    [Fact]
    public async Task GetUnitsByCategory_WhenLocaleIsZh_ReturnsOk()
    {
        // Arrange
        var units = new List<UnitDto>
        {
            new UnitDto { Symbol = "m", Name = "meter", DisplayName = "米", IsBaseUnit = true, IsSIUnit = true, UnitSystem = "SI", ConversionFactor = 1.0 }
        };

        _mockService.Setup(s => s.GetUnitsByCategoryAsync("length"))
            .ReturnsAsync(units);

        // Act
        var result = await _controller.GetUnitsByCategory("length", "zh");

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        Assert.NotNull(okResult.Value);
    }

    [Fact]
    public async Task GetUnitsByCategory_WhenLocaleIsFr_ReturnsOk()
    {
        // Arrange
        var units = new List<UnitDto>
        {
            new UnitDto { Symbol = "m", Name = "meter", DisplayName = "Mètre", IsBaseUnit = true, IsSIUnit = true, UnitSystem = "SI", ConversionFactor = 1.0 }
        };

        _mockService.Setup(s => s.GetUnitsByCategoryAsync("length"))
            .ReturnsAsync(units);

        // Act
        var result = await _controller.GetUnitsByCategory("length", "fr");

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        Assert.NotNull(okResult.Value);
    }

    [Fact]
    public async Task GetUnitsByCategory_WhenCategoryNameIsEmpty_ReturnsNotFound()
    {
        // Arrange
        _mockService.Setup(s => s.GetUnitsByCategoryAsync(""))
            .ReturnsAsync(Enumerable.Empty<UnitDto>());

        // Act
        var result = await _controller.GetUnitsByCategory("");

        // Assert
        var notFoundResult = Assert.IsType<NotFoundObjectResult>(result.Result);
        Assert.NotNull(notFoundResult.Value);
    }

    [Fact]
    public async Task GetUnitsByCategory_WhenNullReferenceException_ReturnsInternalServerError()
    {
        // Arrange
        var exception = new NullReferenceException("Null reference");
        _mockService.Setup(s => s.GetUnitsByCategoryAsync("length"))
            .ThrowsAsync(exception);

        // Act
        var result = await _controller.GetUnitsByCategory("length");

        // Assert
        var statusResult = Assert.IsType<ObjectResult>(result.Result);
        Assert.Equal(500, statusResult.StatusCode);
    }

    [Fact]
    public async Task GetUnitsByCategory_WhenArgumentException_ReturnsInternalServerError()
    {
        // Arrange
        var exception = new ArgumentException("Invalid argument");
        _mockService.Setup(s => s.GetUnitsByCategoryAsync("length"))
            .ThrowsAsync(exception);

        // Act
        var result = await _controller.GetUnitsByCategory("length");

        // Assert
        var statusResult = Assert.IsType<ObjectResult>(result.Result);
        Assert.Equal(500, statusResult.StatusCode);
    }
}

