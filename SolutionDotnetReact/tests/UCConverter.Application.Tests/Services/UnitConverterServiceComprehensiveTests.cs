namespace UCConverter.Application.Tests.Services;

using Moq;
using UCConverter.Application.DTOs;
using UCConverter.Application.Interfaces;
using UCConverter.Application.Services;
using UCConverter.Domain.Entities;
using UCConverter.Domain.Interfaces;
using Xunit;

public class UnitConverterServiceComprehensiveTests
{
    private readonly Mock<IConversionService> _mockConversionService;
    private readonly Mock<IUnitRepository> _mockRepository;
    private readonly Mock<ILocalizationService> _mockLocalizationService;
    private readonly UnitConverterService _service;

    public UnitConverterServiceComprehensiveTests()
    {
        _mockConversionService = new Mock<IConversionService>();
        _mockRepository = new Mock<IUnitRepository>();
        _mockLocalizationService = new Mock<ILocalizationService>();
        _service = new UnitConverterService(
            _mockConversionService.Object,
            _mockRepository.Object,
            _mockLocalizationService.Object);
    }

    [Fact]
    public void Constructor_WhenConversionServiceIsNull_ThrowsArgumentNullException()
    {
        // Act & Assert
        Assert.Throws<ArgumentNullException>(() =>
            new UnitConverterService(null!, _mockRepository.Object, _mockLocalizationService.Object));
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
            new UnitConverterService(_mockConversionService.Object, _mockRepository.Object, null!));
    }

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
    public async Task GetAllCategoriesAsync_WhenMultipleCategories_ReturnsAllCategoryDtos()
    {
        // Arrange
        var categories = new List<Category>
        {
            new Category { Name = "length", DisplayName = "Length", Group = "Common" },
            new Category { Name = "weight", DisplayName = "Weight", Group = "Common" },
            new Category { Name = "temperature", DisplayName = "Temperature", Group = "Common" }
        };

        _mockRepository.Setup(r => r.GetAllCategoriesAsync())
            .ReturnsAsync(categories);

        _mockLocalizationService.Setup(s => s.GetCategoryDisplayName("length")).Returns("Length");
        _mockLocalizationService.Setup(s => s.GetCategoryDisplayName("weight")).Returns("Weight");
        _mockLocalizationService.Setup(s => s.GetCategoryDisplayName("temperature")).Returns("Temperature");

        // Act
        var result = await _service.GetAllCategoriesAsync();

        // Assert
        Assert.NotNull(result);
        var resultList = result.ToList();
        Assert.Equal(3, resultList.Count);
        Assert.Equal("length", resultList[0].Name);
        Assert.Equal("weight", resultList[1].Name);
        Assert.Equal("temperature", resultList[2].Name);
    }

    [Fact]
    public async Task GetUnitsByCategoryAsync_WhenMultipleUnits_ReturnsAllUnitDtos()
    {
        // Arrange
        var units = new List<Unit>
        {
            new Unit { Symbol = "m", Name = "meter", DisplayName = "Meter", IsBaseUnit = true, IsSIUnit = true, UnitSystem = "SI", ConversionFactor = 1.0 },
            new Unit { Symbol = "km", Name = "kilometer", DisplayName = "Kilometer", IsBaseUnit = false, IsSIUnit = true, UnitSystem = "SI", ConversionFactor = 1000.0 },
            new Unit { Symbol = "ft", Name = "foot", DisplayName = "Foot", IsBaseUnit = false, IsSIUnit = false, UnitSystem = "Imperial", ConversionFactor = 0.3048 }
        };

        _mockRepository.Setup(r => r.GetUnitsByCategoryAsync("length"))
            .ReturnsAsync(units);

        _mockLocalizationService.Setup(s => s.GetUnitDisplayName("length", "m", "Meter")).Returns("Meter");
        _mockLocalizationService.Setup(s => s.GetUnitDisplayName("length", "km", "Kilometer")).Returns("Kilometer");
        _mockLocalizationService.Setup(s => s.GetUnitDisplayName("length", "ft", "Foot")).Returns("Foot");

        // Act
        var result = await _service.GetUnitsByCategoryAsync("length");

        // Assert
        Assert.NotNull(result);
        var resultList = result.ToList();
        Assert.Equal(3, resultList.Count);
        Assert.Equal("m", resultList[0].Symbol);
        Assert.Equal("km", resultList[1].Symbol);
        Assert.Equal("ft", resultList[2].Symbol);
    }

    [Fact]
    public async Task ConvertBatchAsync_WhenMultipleTargetUnits_ReturnsMultipleResults()
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
}

