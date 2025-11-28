namespace UCConverter.Api.Tests.Controllers;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using UCConverter.Api.Controllers;
using UCConverter.Application.DTOs;
using UCConverter.Application.Interfaces;
using Xunit;

/// <summary>
/// Comprehensive tests for CategoriesController covering all scenarios
/// </summary>
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

    #region Constructor Tests

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

    #endregion

    #region GetCategories Tests

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
        _mockService.Verify(s => s.GetAllCategoriesAsync(), Times.Once);
    }

    [Fact]
    public async Task GetCategories_WhenEmpty_ReturnsOkWithEmptyList()
    {
        // Arrange
        _mockService.Setup(s => s.GetAllCategoriesAsync())
            .ReturnsAsync(Enumerable.Empty<CategoryDto>());

        // Act
        var result = await _controller.GetCategories();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var returnedCategories = Assert.IsAssignableFrom<IEnumerable<CategoryDto>>(okResult.Value);
        Assert.Empty(returnedCategories);
    }

    [Fact]
    public async Task GetCategories_WhenMultipleCategories_ReturnsAll()
    {
        // Arrange
        var categories = new List<CategoryDto>
        {
            new CategoryDto { Name = "length", DisplayName = "Length", Group = "Common" },
            new CategoryDto { Name = "weight", DisplayName = "Weight", Group = "Common" },
            new CategoryDto { Name = "temperature", DisplayName = "Temperature", Group = "Common" }
        };

        _mockService.Setup(s => s.GetAllCategoriesAsync())
            .ReturnsAsync(categories);

        // Act
        var result = await _controller.GetCategories();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var returnedCategories = Assert.IsAssignableFrom<IEnumerable<CategoryDto>>(okResult.Value);
        Assert.Equal(3, returnedCategories.Count());
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
        _mockLocalizationService.Verify(s => s.GetErrorMessage("InternalServerErrorCategories"), Times.Once);
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
    public async Task GetCategories_WhenNullReferenceException_ReturnsInternalServerError()
    {
        // Arrange
        _mockService.Setup(s => s.GetAllCategoriesAsync())
            .ThrowsAsync(new NullReferenceException("Null reference"));

        // Act
        var result = await _controller.GetCategories();

        // Assert
        var statusResult = Assert.IsType<ObjectResult>(result.Result);
        Assert.Equal(500, statusResult.StatusCode);
    }

    [Fact]
    public async Task GetCategories_WhenInvalidOperationException_ReturnsInternalServerError()
    {
        // Arrange
        _mockService.Setup(s => s.GetAllCategoriesAsync())
            .ThrowsAsync(new InvalidOperationException("Unexpected error"));

        // Act
        var result = await _controller.GetCategories();

        // Assert
        var statusCodeResult = Assert.IsType<ObjectResult>(result.Result);
        Assert.Equal(500, statusCodeResult.StatusCode);
    }

    #endregion

    #region GetUnitsByCategory Tests

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
        _mockLocalizationService.Verify(s => s.GetErrorMessage("CategoryNotFound", "nonexistent"), Times.Once);
    }

    [Fact]
    public async Task GetUnitsByCategory_WhenUnitsListIsEmpty_ReturnsNotFound()
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
    public async Task GetUnitsByCategory_WhenMultipleUnits_ReturnsAll()
    {
        // Arrange
        var units = new List<UnitDto>
        {
            new UnitDto { Symbol = "m", Name = "meter", DisplayName = "Meter", IsBaseUnit = true, IsSIUnit = true, UnitSystem = "SI", ConversionFactor = 1.0 },
            new UnitDto { Symbol = "km", Name = "kilometer", DisplayName = "Kilometer", IsBaseUnit = false, IsSIUnit = true, UnitSystem = "SI", ConversionFactor = 1000.0 },
            new UnitDto { Symbol = "ft", Name = "foot", DisplayName = "Foot", IsBaseUnit = false, IsSIUnit = false, UnitSystem = "Imperial", ConversionFactor = 0.3048 }
        };

        _mockService.Setup(s => s.GetUnitsByCategoryAsync("length"))
            .ReturnsAsync(units);

        // Act
        var result = await _controller.GetUnitsByCategory("length");

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var returnedUnits = Assert.IsAssignableFrom<IEnumerable<UnitDto>>(okResult.Value);
        Assert.Equal(3, returnedUnits.Count());
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
    public async Task GetUnitsByCategory_WhenCategoryNameIsWhitespace_ReturnsNotFound()
    {
        // Arrange
        _mockService.Setup(s => s.GetUnitsByCategoryAsync("   "))
            .ReturnsAsync(Enumerable.Empty<UnitDto>());

        // Act
        var result = await _controller.GetUnitsByCategory("   ");

        // Assert
        var notFoundResult = Assert.IsType<NotFoundObjectResult>(result.Result);
        Assert.NotNull(notFoundResult.Value);
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
        _mockLocalizationService.Verify(s => s.GetErrorMessage("InternalServerErrorUnits"), Times.Once);
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

    [Fact]
    public async Task GetUnitsByCategory_WhenNullReferenceException_ReturnsInternalServerError()
    {
        // Arrange
        _mockService.Setup(s => s.GetUnitsByCategoryAsync("length"))
            .ThrowsAsync(new NullReferenceException("Null reference"));

        // Act
        var result = await _controller.GetUnitsByCategory("length");

        // Assert
        var statusResult = Assert.IsType<ObjectResult>(result.Result);
        Assert.Equal(500, statusResult.StatusCode);
    }

    [Fact]
    public async Task GetUnitsByCategory_WhenInvalidOperationException_ReturnsInternalServerError()
    {
        // Arrange
        _mockService.Setup(s => s.GetUnitsByCategoryAsync(It.IsAny<string>()))
            .ThrowsAsync(new InvalidOperationException("Unexpected error"));

        // Act
        var result = await _controller.GetUnitsByCategory("length");

        // Assert
        var statusCodeResult = Assert.IsType<ObjectResult>(result.Result);
        Assert.Equal(500, statusCodeResult.StatusCode);
    }

    #endregion
}

