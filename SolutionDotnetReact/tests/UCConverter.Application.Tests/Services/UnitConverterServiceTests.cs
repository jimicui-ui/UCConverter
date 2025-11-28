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

    #endregion

    #region ConvertAsync Tests

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

    #endregion

    #region ConvertBatchAsync Tests

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
    public async Task ConvertBatchAsync_WhenNullTargetUnits_ReturnsEmpty()
    {
        // Arrange
        var request = new ConvertRequestDto
        {
            Category = "length",
            FromUnit = "m",
            ToUnit = "km",
            Value = 1000.0
        };

        _mockConversionService.Setup(s => s.ConvertBatchAsync("length", "m", null!, 1000.0))
            .ReturnsAsync(Enumerable.Empty<ConversionResult>());

        // Act
        var result = await _service.ConvertBatchAsync(request, null!);

        // Assert
        Assert.NotNull(result);
        Assert.Empty(result);
    }

    #endregion

    #region GetAllCategoriesAsync Tests

    [Fact]
    public async Task GetAllCategoriesAsync_WhenCategoriesExist_ReturnsLocalizedCategories()
    {
        // Arrange
        var categories = new[]
        {
            new Category { Name = "length", DisplayName = "Length", Group = "Common", BaseUnit = new Unit { Symbol = "m", Category = "length" }, Units = new List<Unit>() },
            new Category { Name = "weight", DisplayName = "Weight", Group = "Common", BaseUnit = new Unit { Symbol = "kg", Category = "weight" }, Units = new List<Unit>() }
        };

        _mockRepository.Setup(r => r.GetAllCategoriesAsync())
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

    #endregion

    #region GetUnitsByCategoryAsync Tests

    [Fact]
    public async Task GetUnitsByCategoryAsync_WhenUnitsExist_ReturnsLocalizedUnits()
    {
        // Arrange
        var units = new[]
        {
            new Unit { Symbol = "m", Name = "meter", DisplayName = "Meter", Category = "length", IsBaseUnit = true, IsSIUnit = true, UnitSystem = "SI", ConversionFactor = 1.0 },
            new Unit { Symbol = "km", Name = "kilometer", DisplayName = "Kilometer", Category = "length", IsBaseUnit = false, IsSIUnit = true, UnitSystem = "SI", ConversionFactor = 1000.0 }
        };

        _mockRepository.Setup(r => r.GetUnitsByCategoryAsync("length"))
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
    public async Task GetUnitsByCategoryAsync_WhenEmpty_ReturnsEmpty()
    {
        // Arrange
        _mockRepository.Setup(r => r.GetUnitsByCategoryAsync("nonexistent"))
            .ReturnsAsync(Enumerable.Empty<Unit>());

        // Act
        var result = await _service.GetUnitsByCategoryAsync("nonexistent");

        // Assert
        Assert.NotNull(result);
        Assert.Empty(result);
    }

    #endregion
}

