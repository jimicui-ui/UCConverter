namespace UCConverter.Application.Tests.Services;

using Moq;
using UCConverter.Application.DTOs;
using UCConverter.Application.Interfaces;
using UCConverter.Application.Services;
using UCConverter.Domain.Entities;
using UCConverter.Domain.Interfaces;
using Xunit;

public class UnitConverterServiceTests
{
    private readonly Mock<IConversionService> _mockConversionService;
    private readonly Mock<IUnitRepository> _mockRepository;
    private readonly Mock<ILocalizationService> _mockLocalizationService;
    private readonly UnitConverterService _service;

    public UnitConverterServiceTests()
    {
        _mockConversionService = new Mock<IConversionService>();
        _mockRepository = new Mock<IUnitRepository>();
        _mockLocalizationService = new Mock<ILocalizationService>();
        _service = new UnitConverterService(_mockConversionService.Object, _mockRepository.Object, _mockLocalizationService.Object);
        
        // Setup default localization behavior
        _mockLocalizationService.Setup(l => l.GetCategoryDisplayName(It.IsAny<string>()))
            .Returns<string>(name => char.ToUpper(name[0]) + name.Substring(1));
        _mockLocalizationService.Setup(l => l.GetUnitDisplayName(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
            .Returns<string, string, string>((cat, sym, def) => def);
    }

    [Fact]
    public async Task ConvertAsync_WhenValid_ReturnsConvertResponseDto()
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
            FromUnit = new Unit { Symbol = "m", Name = "meter", IsBaseUnit = true, IsSIUnit = true, UnitSystem = "SI" },
            ToUnit = new Unit { Symbol = "km", Name = "kilometer", IsBaseUnit = false, IsSIUnit = true, UnitSystem = "SI" },
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
        Assert.Equal("m", result.FromUnit.Symbol);
        Assert.Equal("km", result.ToUnit.Symbol);
    }

    [Fact]
    public async Task ConvertBatchAsync_WhenValid_ReturnsMultipleResults()
    {
        // Arrange
        var request = new ConvertRequestDto
        {
            Category = "length",
            FromUnit = "m",
            ToUnit = "km",
            Value = 1000.0
        };

        var results = new List<ConversionResult>
        {
            new ConversionResult
            {
                Result = 1.0,
                FormattedResult = "1 km",
                FromUnit = new Unit { Symbol = "m" },
                ToUnit = new Unit { Symbol = "km" }
            },
            new ConversionResult
            {
                Result = 100000.0,
                FormattedResult = "100000 cm",
                FromUnit = new Unit { Symbol = "m" },
                ToUnit = new Unit { Symbol = "cm" }
            }
        };

        _mockConversionService.Setup(s => s.ConvertBatchAsync("length", "m", It.IsAny<IEnumerable<string>>(), 1000.0))
            .ReturnsAsync(results);

        // Act
        var result = await _service.ConvertBatchAsync(request, new[] { "km", "cm" });

        // Assert
        Assert.NotNull(result);
        var resultList = result.ToList();
        Assert.Equal(2, resultList.Count);
    }

    [Fact]
    public async Task GetAllCategoriesAsync_WhenValid_ReturnsCategoryDtos()
    {
        // Arrange
        var categories = new List<Category>
        {
            new Category { Name = "length", DisplayName = "Length" },
            new Category { Name = "weight", DisplayName = "Weight" }
        };

        _mockRepository.Setup(r => r.GetAllCategoriesAsync())
            .ReturnsAsync(categories);

        // Act
        var result = await _service.GetAllCategoriesAsync();

        // Assert
        Assert.NotNull(result);
        var resultList = result.ToList();
        Assert.Equal(2, resultList.Count);
        Assert.Contains(resultList, c => c.Name == "length");
        Assert.Contains(resultList, c => c.Name == "weight");
    }

    [Fact]
    public async Task GetUnitsByCategoryAsync_WhenValid_ReturnsUnitDtos()
    {
        // Arrange
        var units = new List<Unit>
        {
            new Unit { Symbol = "m", Name = "meter", DisplayName = "Meter", IsBaseUnit = true, IsSIUnit = true, UnitSystem = "SI", ConversionFactor = 1.0 },
            new Unit { Symbol = "km", Name = "kilometer", DisplayName = "Kilometer", IsBaseUnit = false, IsSIUnit = true, UnitSystem = "SI", ConversionFactor = 1000.0 }
        };

        _mockRepository.Setup(r => r.GetUnitsByCategoryAsync("length"))
            .ReturnsAsync(units);

        // Act
        var result = await _service.GetUnitsByCategoryAsync("length");

        // Assert
        Assert.NotNull(result);
        var resultList = result.ToList();
        Assert.Equal(2, resultList.Count);
        Assert.Contains(resultList, u => u.Symbol == "m");
        Assert.Contains(resultList, u => u.Symbol == "km");
    }
}

