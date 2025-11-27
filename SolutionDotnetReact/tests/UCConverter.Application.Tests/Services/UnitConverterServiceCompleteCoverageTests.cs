namespace UCConverter.Application.Tests.Services;

using Moq;
using UCConverter.Application.DTOs;
using UCConverter.Application.Interfaces;
using UCConverter.Application.Services;
using UCConverter.Domain.Entities;
using UCConverter.Domain.Interfaces;
using Xunit;

/// <summary>
/// Comprehensive tests to achieve 100% code coverage for UnitConverterService
/// </summary>
public class UnitConverterServiceCompleteCoverageTests
{
    private readonly Mock<IConversionService> _mockConversionService;
    private readonly Mock<IUnitRepository> _mockRepository;
    private readonly Mock<ILocalizationService> _mockLocalizationService;
    private readonly UnitConverterService _service;

    public UnitConverterServiceCompleteCoverageTests()
    {
        _mockConversionService = new Mock<IConversionService>();
        _mockRepository = new Mock<IUnitRepository>();
        _mockLocalizationService = new Mock<ILocalizationService>();
        _service = new UnitConverterService(
            _mockConversionService.Object,
            _mockRepository.Object,
            _mockLocalizationService.Object);
    }

    #region Constructor Tests

    [Fact]
    public void Constructor_WhenConversionServiceIsNull_ThrowsArgumentNullException()
    {
        // Act & Assert
        var ex = Assert.Throws<ArgumentNullException>(() =>
            new UnitConverterService(null!, _mockRepository.Object, _mockLocalizationService.Object));
        Assert.Equal("conversionService", ex.ParamName);
    }

    [Fact]
    public void Constructor_WhenUnitRepositoryIsNull_ThrowsArgumentNullException()
    {
        // Act & Assert
        var ex = Assert.Throws<ArgumentNullException>(() =>
            new UnitConverterService(_mockConversionService.Object, null!, _mockLocalizationService.Object));
        Assert.Equal("unitRepository", ex.ParamName);
    }

    [Fact]
    public void Constructor_WhenLocalizationServiceIsNull_ThrowsArgumentNullException()
    {
        // Act & Assert
        var ex = Assert.Throws<ArgumentNullException>(() =>
            new UnitConverterService(_mockConversionService.Object, _mockRepository.Object, null!));
        Assert.Equal("localizationService", ex.ParamName);
    }

    [Fact]
    public void Constructor_WhenAllParametersValid_CreatesInstance()
    {
        // Act
        var service = new UnitConverterService(
            _mockConversionService.Object,
            _mockRepository.Object,
            _mockLocalizationService.Object);

        // Assert
        Assert.NotNull(service);
    }

    #endregion

    #region ConvertAsync Tests

    [Fact]
    public async Task ConvertAsync_WhenValidRequest_ReturnsConvertResponseDto()
    {
        // Arrange
        var request = new ConvertRequestDto
        {
            Category = "length",
            FromUnit = "m",
            ToUnit = "km",
            Value = 1000.0
        };

        var conversionResult = new ConversionResult
        {
            Result = 1.0,
            FormattedResult = "1 km",
            Precision = 2,
            Formula = "x / 1000",
            FromUnit = new Unit { Symbol = "m", Name = "meter" },
            ToUnit = new Unit { Symbol = "km", Name = "kilometer" }
        };

        _mockConversionService.Setup(s => s.ConvertAsync("length", "m", "km", 1000.0))
            .ReturnsAsync(conversionResult);

        // Act
        var result = await _service.ConvertAsync(request);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(1.0, result.Result);
        Assert.Equal("1 km", result.FormattedResult);
        Assert.Equal(2, result.Precision);
        Assert.Equal("x / 1000", result.Formula);
    }

    [Fact]
    public async Task ConvertAsync_WhenRequestHasZeroValue_ReturnsResult()
    {
        // Arrange
        var request = new ConvertRequestDto
        {
            Category = "length",
            FromUnit = "m",
            ToUnit = "km",
            Value = 0.0
        };

        var conversionResult = new ConversionResult
        {
            Result = 0.0,
            FormattedResult = "0 km",
            Precision = 2,
            Formula = null,
            FromUnit = new Unit { Symbol = "m", Name = "meter" },
            ToUnit = new Unit { Symbol = "km", Name = "kilometer" }
        };

        _mockConversionService.Setup(s => s.ConvertAsync("length", "m", "km", 0.0))
            .ReturnsAsync(conversionResult);

        // Act
        var result = await _service.ConvertAsync(request);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(0.0, result.Result);
    }

    [Fact]
    public async Task ConvertAsync_WhenRequestHasNegativeValue_ReturnsResult()
    {
        // Arrange
        var request = new ConvertRequestDto
        {
            Category = "temperature",
            FromUnit = "°C",
            ToUnit = "°F",
            Value = -10.0
        };

        var conversionResult = new ConversionResult
        {
            Result = 14.0,
            FormattedResult = "14 °F",
            Precision = 2,
            Formula = "x * 9/5 + 32",
            FromUnit = new Unit { Symbol = "°C", Name = "celsius" },
            ToUnit = new Unit { Symbol = "°F", Name = "fahrenheit" }
        };

        _mockConversionService.Setup(s => s.ConvertAsync("temperature", "°C", "°F", -10.0))
            .ReturnsAsync(conversionResult);

        // Act
        var result = await _service.ConvertAsync(request);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(14.0, result.Result);
    }

    [Fact]
    public async Task ConvertAsync_WhenRequestHasLargeValue_ReturnsResult()
    {
        // Arrange
        var request = new ConvertRequestDto
        {
            Category = "length",
            FromUnit = "m",
            ToUnit = "km",
            Value = 1000000.0
        };

        var conversionResult = new ConversionResult
        {
            Result = 1000.0,
            FormattedResult = "1000 km",
            Precision = 2,
            Formula = null,
            FromUnit = new Unit { Symbol = "m", Name = "meter" },
            ToUnit = new Unit { Symbol = "km", Name = "kilometer" }
        };

        _mockConversionService.Setup(s => s.ConvertAsync("length", "m", "km", 1000000.0))
            .ReturnsAsync(conversionResult);

        // Act
        var result = await _service.ConvertAsync(request);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(1000.0, result.Result);
    }

    #endregion

    #region ConvertBatchAsync Tests

    [Fact]
    public async Task ConvertBatchAsync_WhenValidRequest_ReturnsMultipleResults()
    {
        // Arrange
        var request = new ConvertRequestDto
        {
            Category = "length",
            FromUnit = "m",
            ToUnit = "km",
            Value = 1000.0
        };
        var targetUnits = new[] { "km", "ft", "in" };

        var results = new List<ConversionResult>
        {
            new ConversionResult
            {
                Result = 1.0,
                FormattedResult = "1 km",
                Precision = 2,
                Formula = "x / 1000",
                FromUnit = new Unit { Symbol = "m", Name = "meter" },
                ToUnit = new Unit { Symbol = "km", Name = "kilometer" }
            },
            new ConversionResult
            {
                Result = 3280.84,
                FormattedResult = "3280.84 ft",
                Precision = 2,
                Formula = "x * 3.28084",
                FromUnit = new Unit { Symbol = "m", Name = "meter" },
                ToUnit = new Unit { Symbol = "ft", Name = "foot" }
            },
            new ConversionResult
            {
                Result = 39370.1,
                FormattedResult = "39370.1 in",
                Precision = 2,
                Formula = "x * 39.3701",
                FromUnit = new Unit { Symbol = "m", Name = "meter" },
                ToUnit = new Unit { Symbol = "in", Name = "inch" }
            }
        };

        _mockConversionService.Setup(s => s.ConvertBatchAsync("length", "m", targetUnits, 1000.0))
            .ReturnsAsync(results);

        // Act
        var result = await _service.ConvertBatchAsync(request, targetUnits);

        // Assert
        Assert.NotNull(result);
        var resultList = result.ToList();
        Assert.Equal(3, resultList.Count);
        Assert.All(resultList, r => Assert.NotNull(r));
    }

    [Fact]
    public async Task ConvertBatchAsync_WhenEmptyTargetUnits_ReturnsEmpty()
    {
        // Arrange
        var request = new ConvertRequestDto
        {
            Category = "length",
            FromUnit = "m",
            ToUnit = "km",
            Value = 1000.0
        };
        var targetUnits = Array.Empty<string>();

        _mockConversionService.Setup(s => s.ConvertBatchAsync("length", "m", targetUnits, 1000.0))
            .ReturnsAsync(Enumerable.Empty<ConversionResult>());

        // Act
        var result = await _service.ConvertBatchAsync(request, targetUnits);

        // Assert
        Assert.NotNull(result);
        Assert.Empty(result);
    }

    [Fact]
    public async Task ConvertBatchAsync_WhenSingleTargetUnit_ReturnsSingleResult()
    {
        // Arrange
        var request = new ConvertRequestDto
        {
            Category = "length",
            FromUnit = "m",
            ToUnit = "km",
            Value = 1000.0
        };
        var targetUnits = new[] { "km" };

        var results = new List<ConversionResult>
        {
            new ConversionResult
            {
                Result = 1.0,
                FormattedResult = "1 km",
                Precision = 2,
                Formula = null,
                FromUnit = new Unit { Symbol = "m", Name = "meter" },
                ToUnit = new Unit { Symbol = "km", Name = "kilometer" }
            }
        };

        _mockConversionService.Setup(s => s.ConvertBatchAsync("length", "m", targetUnits, 1000.0))
            .ReturnsAsync(results);

        // Act
        var result = await _service.ConvertBatchAsync(request, targetUnits);

        // Assert
        Assert.NotNull(result);
        var resultList = result.ToList();
        Assert.Single(resultList);
    }

    #endregion

    #region GetAllCategoriesAsync Tests

    [Fact]
    public async Task GetAllCategoriesAsync_WhenEmpty_ReturnsEmpty()
    {
        // Arrange
        _mockRepository.Setup(r => r.GetAllCategoriesAsync())
            .ReturnsAsync(Enumerable.Empty<Category>());

        // Act
        var result = await _service.GetAllCategoriesAsync();

        // Assert
        Assert.NotNull(result);
        Assert.Empty(result);
    }

    [Fact]
    public async Task GetAllCategoriesAsync_WhenSingleCategory_ReturnsSingleCategoryDto()
    {
        // Arrange
        var categories = new List<Category>
        {
            new Category { Name = "length", DisplayName = "Length", Group = "Common" }
        };

        _mockRepository.Setup(r => r.GetAllCategoriesAsync())
            .ReturnsAsync(categories);

        _mockLocalizationService.Setup(s => s.GetCategoryDisplayName("length")).Returns("Length");

        // Act
        var result = await _service.GetAllCategoriesAsync();

        // Assert
        Assert.NotNull(result);
        var resultList = result.ToList();
        Assert.Single(resultList);
        Assert.Equal("length", resultList[0].Name);
    }

    [Fact]
    public async Task GetAllCategoriesAsync_WhenMultipleCategories_ReturnsAllCategoryDtos()
    {
        // Arrange
        var categories = new List<Category>
        {
            new Category { Name = "length", DisplayName = "Length", Group = "Common" },
            new Category { Name = "weight", DisplayName = "Weight", Group = "Common" },
            new Category { Name = "temperature", DisplayName = "Temperature", Group = "Common" },
            new Category { Name = "acceleration", DisplayName = "Acceleration", Group = "Engineering" },
            new Category { Name = "current", DisplayName = "Current", Group = "Electricity" }
        };

        _mockRepository.Setup(r => r.GetAllCategoriesAsync())
            .ReturnsAsync(categories);

        _mockLocalizationService.Setup(s => s.GetCategoryDisplayName(It.IsAny<string>())).Returns<string>(s => s);

        // Act
        var result = await _service.GetAllCategoriesAsync();

        // Assert
        Assert.NotNull(result);
        var resultList = result.ToList();
        Assert.Equal(5, resultList.Count);
        Assert.All(resultList, r => Assert.NotNull(r));
    }

    #endregion

    #region GetUnitsByCategoryAsync Tests

    [Fact]
    public async Task GetUnitsByCategoryAsync_WhenEmpty_ReturnsEmpty()
    {
        // Arrange
        _mockRepository.Setup(r => r.GetUnitsByCategoryAsync("test"))
            .ReturnsAsync(Enumerable.Empty<Unit>());

        // Act
        var result = await _service.GetUnitsByCategoryAsync("test");

        // Assert
        Assert.NotNull(result);
        Assert.Empty(result);
    }

    [Fact]
    public async Task GetUnitsByCategoryAsync_WhenSingleUnit_ReturnsSingleUnitDto()
    {
        // Arrange
        var units = new List<Unit>
        {
            new Unit { Symbol = "m", Name = "meter", DisplayName = "Meter", IsBaseUnit = true, IsSIUnit = true, UnitSystem = "SI", ConversionFactor = 1.0 }
        };

        _mockRepository.Setup(r => r.GetUnitsByCategoryAsync("length"))
            .ReturnsAsync(units);

        _mockLocalizationService.Setup(s => s.GetUnitDisplayName("length", "m", "Meter")).Returns("Meter");

        // Act
        var result = await _service.GetUnitsByCategoryAsync("length");

        // Assert
        Assert.NotNull(result);
        var resultList = result.ToList();
        Assert.Single(resultList);
        Assert.Equal("m", resultList[0].Symbol);
    }

    [Fact]
    public async Task GetUnitsByCategoryAsync_WhenMultipleUnits_ReturnsAllUnitDtos()
    {
        // Arrange
        var units = new List<Unit>
        {
            new Unit { Symbol = "m", Name = "meter", DisplayName = "Meter", IsBaseUnit = true, IsSIUnit = true, UnitSystem = "SI", ConversionFactor = 1.0 },
            new Unit { Symbol = "km", Name = "kilometer", DisplayName = "Kilometer", IsBaseUnit = false, IsSIUnit = true, UnitSystem = "SI", ConversionFactor = 1000.0 },
            new Unit { Symbol = "ft", Name = "foot", DisplayName = "Foot", IsBaseUnit = false, IsSIUnit = false, UnitSystem = "Imperial", ConversionFactor = 0.3048 },
            new Unit { Symbol = "in", Name = "inch", DisplayName = "Inch", IsBaseUnit = false, IsSIUnit = false, UnitSystem = "Imperial", ConversionFactor = 0.0254 }
        };

        _mockRepository.Setup(r => r.GetUnitsByCategoryAsync("length"))
            .ReturnsAsync(units);

        _mockLocalizationService.Setup(s => s.GetUnitDisplayName(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).Returns<string, string, string>((c, s, d) => d);

        // Act
        var result = await _service.GetUnitsByCategoryAsync("length");

        // Assert
        Assert.NotNull(result);
        var resultList = result.ToList();
        Assert.Equal(4, resultList.Count);
        Assert.All(resultList, r => Assert.NotNull(r));
    }

    [Fact]
    public async Task GetUnitsByCategoryAsync_WhenUnitsHaveNullConversionFactor_HandlesCorrectly()
    {
        // Arrange
        var units = new List<Unit>
        {
            new Unit { Symbol = "°C", Name = "celsius", DisplayName = "Celsius", IsBaseUnit = false, IsSIUnit = true, UnitSystem = "SI", ConversionFactor = null }
        };

        _mockRepository.Setup(r => r.GetUnitsByCategoryAsync("temperature"))
            .ReturnsAsync(units);

        _mockLocalizationService.Setup(s => s.GetUnitDisplayName("temperature", "°C", "Celsius")).Returns("Celsius");

        // Act
        var result = await _service.GetUnitsByCategoryAsync("temperature");

        // Assert
        Assert.NotNull(result);
        var resultList = result.ToList();
        Assert.Single(resultList);
        Assert.Null(resultList[0].ConversionFactor);
    }

    #endregion
}

