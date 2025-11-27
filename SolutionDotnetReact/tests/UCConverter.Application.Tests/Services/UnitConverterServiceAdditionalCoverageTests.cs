namespace UCConverter.Application.Tests.Services;

using Moq;
using UCConverter.Application.DTOs;
using UCConverter.Application.Interfaces;
using UCConverter.Application.Services;
using UCConverter.Domain.Entities;
using UCConverter.Domain.Interfaces;
using Xunit;

/// <summary>
/// Additional tests for UnitConverterService to improve coverage
/// </summary>
public class UnitConverterServiceAdditionalCoverageTests
{
    private readonly Mock<IConversionService> _mockConversionService;
    private readonly Mock<IUnitRepository> _mockUnitRepository;
    private readonly Mock<ILocalizationService> _mockLocalizationService;
    private readonly UnitConverterService _service;

    public UnitConverterServiceAdditionalCoverageTests()
    {
        _mockConversionService = new Mock<IConversionService>();
        _mockUnitRepository = new Mock<IUnitRepository>();
        _mockLocalizationService = new Mock<ILocalizationService>();
        _service = new UnitConverterService(
            _mockConversionService.Object,
            _mockUnitRepository.Object,
            _mockLocalizationService.Object);

        // Setup default localization behavior
        _mockLocalizationService.Setup(l => l.GetCategoryDisplayName(It.IsAny<string>()))
            .Returns<string>(name => name);
        _mockLocalizationService.Setup(l => l.GetUnitDisplayName(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
            .Returns<string, string, string>((cat, sym, def) => def);
    }

    [Fact]
    public async Task ConvertAsync_WhenConversionSucceeds_ReturnsConvertResponseDto()
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
            Precision = 4,
            Formula = null,
            FromUnit = new Unit { Symbol = "m", Name = "meter", DisplayName = "Meter", Category = "length", IsBaseUnit = true, IsSIUnit = true, UnitSystem = "SI", ConversionFactor = 1.0 },
            ToUnit = new Unit { Symbol = "km", Name = "kilometer", DisplayName = "Kilometer", Category = "length", IsBaseUnit = false, IsSIUnit = true, UnitSystem = "SI", ConversionFactor = 1000.0 },
            OriginalValue = 1000.0
        };

        _mockConversionService.Setup(s => s.ConvertAsync("length", "m", "km", 1000.0))
            .ReturnsAsync(conversionResult);

        // Act
        var result = await _service.ConvertAsync(request);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(1.0, result.Result);
        Assert.Equal("1 km", result.FormattedResult);
        Assert.Equal(4, result.Precision);
        Assert.Null(result.Formula);
    }

    [Fact]
    public async Task ConvertAsync_WhenConversionHasFormula_ReturnsConvertResponseDtoWithFormula()
    {
        // Arrange
        var request = new ConvertRequestDto
        {
            Category = "temperature",
            FromUnit = "°C",
            ToUnit = "°F",
            Value = 25.0
        };

        var conversionResult = new ConversionResult
        {
            Result = 77.0,
            FormattedResult = "77 °F",
            Precision = 4,
            Formula = "x * 9/5 + 32",
            FromUnit = new Unit { Symbol = "°C", Name = "celsius", DisplayName = "Celsius", Category = "temperature" },
            ToUnit = new Unit { Symbol = "°F", Name = "fahrenheit", DisplayName = "Fahrenheit", Category = "temperature" },
            OriginalValue = 25.0
        };

        _mockConversionService.Setup(s => s.ConvertAsync("temperature", "°C", "°F", 25.0))
            .ReturnsAsync(conversionResult);

        // Act
        var result = await _service.ConvertAsync(request);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(77.0, result.Result);
        Assert.Equal("x * 9/5 + 32", result.Formula);
    }

    [Fact]
    public async Task ConvertBatchAsync_WhenMultipleTargetUnits_ReturnsMultipleResults()
    {
        // Arrange
        var request = new ConvertRequestDto
        {
            Category = "length",
            FromUnit = "m",
            Value = 1000.0
        };
        var targetUnits = new[] { "km", "ft", "in" };

        var fromUnit = new Unit { Symbol = "m", Name = "meter", Category = "length" };
        var results = new[]
        {
            new ConversionResult { Result = 1.0, FormattedResult = "1 km", FromUnit = fromUnit, ToUnit = new Unit { Symbol = "km", Category = "length" } },
            new ConversionResult { Result = 3280.84, FormattedResult = "3280.84 ft", FromUnit = fromUnit, ToUnit = new Unit { Symbol = "ft", Category = "length" } },
            new ConversionResult { Result = 39370.1, FormattedResult = "39370.1 in", FromUnit = fromUnit, ToUnit = new Unit { Symbol = "in", Category = "length" } }
        };

        _mockConversionService.Setup(s => s.ConvertBatchAsync("length", "m", It.IsAny<IEnumerable<string>>(), 1000.0))
            .ReturnsAsync(results);

        // Act
        var batchResults = await _service.ConvertBatchAsync(request, targetUnits);

        // Assert
        var resultsList = batchResults.ToList();
        Assert.Equal(3, resultsList.Count);
        Assert.Equal(1.0, resultsList[0].Result);
        Assert.Equal(3280.84, resultsList[1].Result);
        Assert.Equal(39370.1, resultsList[2].Result);
    }

    [Fact]
    public async Task GetAllCategoriesAsync_WhenCategoriesExist_ReturnsLocalizedCategories()
    {
        // Arrange
        var categories = new[]
        {
            new Category { Name = "length", DisplayName = "Length", Group = "Common", BaseUnit = new Unit { Symbol = "m", Category = "length" }, Units = new List<Unit>() },
            new Category { Name = "weight", DisplayName = "Weight", Group = "Common", BaseUnit = new Unit { Symbol = "kg", Category = "weight" }, Units = new List<Unit>() }
        };

        _mockUnitRepository.Setup(r => r.GetAllCategoriesAsync())
            .ReturnsAsync(categories);

        _mockLocalizationService.Setup(l => l.GetCategoryDisplayName("length"))
            .Returns("长度");
        _mockLocalizationService.Setup(l => l.GetCategoryDisplayName("weight"))
            .Returns("重量");

        // Act
        var result = await _service.GetAllCategoriesAsync();

        // Assert
        var resultList = result.ToList();
        Assert.Equal(2, resultList.Count);
        Assert.Equal("长度", resultList[0].DisplayName);
        Assert.Equal("重量", resultList[1].DisplayName);
        _mockLocalizationService.Verify(l => l.GetCategoryDisplayName("length"), Times.Once);
        _mockLocalizationService.Verify(l => l.GetCategoryDisplayName("weight"), Times.Once);
    }

    [Fact]
    public async Task GetUnitsByCategoryAsync_WhenUnitsExist_ReturnsLocalizedUnits()
    {
        // Arrange
        var units = new[]
        {
            new Unit { Symbol = "m", Name = "meter", DisplayName = "Meter", Category = "length", IsBaseUnit = true, IsSIUnit = true, UnitSystem = "SI", ConversionFactor = 1.0 },
            new Unit { Symbol = "km", Name = "kilometer", DisplayName = "Kilometer", Category = "length", IsBaseUnit = false, IsSIUnit = true, UnitSystem = "SI", ConversionFactor = 1000.0 }
        };

        _mockUnitRepository.Setup(r => r.GetUnitsByCategoryAsync("length"))
            .ReturnsAsync(units);

        _mockLocalizationService.Setup(l => l.GetUnitDisplayName("length", "m", "Meter"))
            .Returns("米");
        _mockLocalizationService.Setup(l => l.GetUnitDisplayName("length", "km", "Kilometer"))
            .Returns("千米");

        // Act
        var result = await _service.GetUnitsByCategoryAsync("length");

        // Assert
        var resultList = result.ToList();
        Assert.Equal(2, resultList.Count);
        Assert.Equal("米", resultList[0].DisplayName);
        Assert.Equal("千米", resultList[1].DisplayName);
        _mockLocalizationService.Verify(l => l.GetUnitDisplayName("length", "m", "Meter"), Times.Once);
        _mockLocalizationService.Verify(l => l.GetUnitDisplayName("length", "km", "Kilometer"), Times.Once);
    }

    [Fact]
    public void Constructor_WhenAllParametersValid_InitializesService()
    {
        // Arrange & Act
        var service = new UnitConverterService(
            _mockConversionService.Object,
            _mockUnitRepository.Object,
            _mockLocalizationService.Object);

        // Assert
        Assert.NotNull(service);
    }

    [Fact]
    public void Constructor_WhenConversionServiceIsNull_ThrowsArgumentNullException()
    {
        // Act & Assert
        Assert.Throws<ArgumentNullException>(() =>
            new UnitConverterService(null!, _mockUnitRepository.Object, _mockLocalizationService.Object));
    }

    [Fact]
    public void Constructor_WhenUnitRepositoryIsNull_ThrowsArgumentNullException()
    {
        // Act & Assert
        Assert.Throws<ArgumentNullException>(() =>
            new UnitConverterService(_mockConversionService.Object, null!, _mockLocalizationService.Object));
    }

    [Fact]
    public void Constructor_WhenLocalizationServiceIsNull_ThrowsArgumentNullException()
    {
        // Act & Assert
        Assert.Throws<ArgumentNullException>(() =>
            new UnitConverterService(_mockConversionService.Object, _mockUnitRepository.Object, null!));
    }
}

