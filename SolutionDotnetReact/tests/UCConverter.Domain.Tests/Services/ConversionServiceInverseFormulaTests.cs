namespace UCConverter.Domain.Tests.Services;

using Moq;
using UCConverter.Domain.Entities;
using UCConverter.Domain.Exceptions;
using UCConverter.Domain.Interfaces;
using UCConverter.Domain.Services;
using Xunit;

public class ConversionServiceInverseFormulaTests
{
    private readonly Mock<IUnitRepository> _mockRepository;
    private readonly ConversionService _conversionService;

    public ConversionServiceInverseFormulaTests()
    {
        _mockRepository = new Mock<IUnitRepository>();
        _conversionService = new ConversionService(_mockRepository.Object);
    }

    [Fact]
    public async Task ConvertAsync_WhenUsingInverseFormula_KelvinToCelsius_ConvertsCorrectly()
    {
        // Arrange - Kelvin to Celsius using explicit inverse formula
        var fromUnit = new Unit
        {
            Symbol = "K",
            Name = "kelvin",
            Category = "temperature",
            IsBaseUnit = true,
            ConversionFactor = 1.0,
            ConversionFormula = "x",
            ConversionInverseFormula = "x"
        };
        var toUnit = new Unit
        {
            Symbol = "°C",
            Name = "celsius",
            Category = "temperature",
            IsBaseUnit = false,
            ConversionFormula = "x + 273.15",
            ConversionInverseFormula = "x - 273.15"
        };
        var category = new Category
        {
            Name = "temperature",
            BaseUnit = fromUnit,
            Units = new List<Unit> { fromUnit, toUnit }
        };
        _mockRepository.Setup(r => r.GetCategoryByNameAsync("temperature"))
            .ReturnsAsync(category);

        // Act
        var result = await _conversionService.ConvertAsync("temperature", "K", "°C", 298.15);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(25.0, result.Result, 2);
    }

    [Fact]
    public async Task ConvertAsync_WhenUsingInverseFormula_KelvinToFahrenheit_ConvertsCorrectly()
    {
        // Arrange - Kelvin to Fahrenheit using explicit inverse formula
        var fromUnit = new Unit
        {
            Symbol = "K",
            Name = "kelvin",
            Category = "temperature",
            IsBaseUnit = true,
            ConversionFactor = 1.0,
            ConversionFormula = "x",
            ConversionInverseFormula = "x"
        };
        var toUnit = new Unit
        {
            Symbol = "°F",
            Name = "fahrenheit",
            Category = "temperature",
            IsBaseUnit = false,
            ConversionFormula = "(x - 32) * 5/9 + 273.15",
            ConversionInverseFormula = "(x - 273.15) * 9/5 + 32"
        };
        var category = new Category
        {
            Name = "temperature",
            BaseUnit = fromUnit,
            Units = new List<Unit> { fromUnit, toUnit }
        };
        _mockRepository.Setup(r => r.GetCategoryByNameAsync("temperature"))
            .ReturnsAsync(category);

        // Act
        var result = await _conversionService.ConvertAsync("temperature", "K", "°F", 273.15);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(32.0, result.Result, 2); // 0°C = 32°F
    }

    [Fact]
    public async Task ConvertAsync_WhenUsingInverseFormula_CelsiusToFahrenheit_ConvertsCorrectly()
    {
        // Arrange - Celsius to Fahrenheit (both have formulas, convert through base)
        var fromUnit = new Unit
        {
            Symbol = "°C",
            Name = "celsius",
            Category = "temperature",
            IsBaseUnit = false,
            ConversionFormula = "x + 273.15",
            ConversionInverseFormula = "x - 273.15"
        };
        var toUnit = new Unit
        {
            Symbol = "°F",
            Name = "fahrenheit",
            Category = "temperature",
            IsBaseUnit = false,
            ConversionFormula = "(x - 32) * 5/9 + 273.15",
            ConversionInverseFormula = "(x - 273.15) * 9/5 + 32"
        };
        var baseUnit = new Unit
        {
            Symbol = "K",
            Name = "kelvin",
            Category = "temperature",
            IsBaseUnit = true,
            ConversionFactor = 1.0,
            ConversionFormula = "x",
            ConversionInverseFormula = "x"
        };
        var category = new Category
        {
            Name = "temperature",
            BaseUnit = baseUnit,
            Units = new List<Unit> { fromUnit, toUnit, baseUnit }
        };
        _mockRepository.Setup(r => r.GetCategoryByNameAsync("temperature"))
            .ReturnsAsync(category);

        // Act - Convert 25°C to Fahrenheit
        var result = await _conversionService.ConvertAsync("temperature", "°C", "°F", 25.0);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(77.0, result.Result, 1); // 25°C = 77°F
    }

    [Fact]
    public async Task ConvertAsync_WhenUsingInverseFormula_FahrenheitToCelsius_ConvertsCorrectly()
    {
        // Arrange - Fahrenheit to Celsius (both have formulas, convert through base)
        var fromUnit = new Unit
        {
            Symbol = "°F",
            Name = "fahrenheit",
            Category = "temperature",
            IsBaseUnit = false,
            ConversionFormula = "(x - 32) * 5/9 + 273.15",
            ConversionInverseFormula = "(x - 273.15) * 9/5 + 32"
        };
        var toUnit = new Unit
        {
            Symbol = "°C",
            Name = "celsius",
            Category = "temperature",
            IsBaseUnit = false,
            ConversionFormula = "x + 273.15",
            ConversionInverseFormula = "x - 273.15"
        };
        var baseUnit = new Unit
        {
            Symbol = "K",
            Name = "kelvin",
            Category = "temperature",
            IsBaseUnit = true,
            ConversionFactor = 1.0,
            ConversionFormula = "x",
            ConversionInverseFormula = "x"
        };
        var category = new Category
        {
            Name = "temperature",
            BaseUnit = baseUnit,
            Units = new List<Unit> { fromUnit, toUnit, baseUnit }
        };
        _mockRepository.Setup(r => r.GetCategoryByNameAsync("temperature"))
            .ReturnsAsync(category);

        // Act - Convert 32°F to Celsius
        var result = await _conversionService.ConvertAsync("temperature", "°F", "°C", 32.0);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(0.0, result.Result, 2); // 32°F = 0°C
    }

    [Fact]
    public async Task ConvertAsync_WhenInverseFormulaIsMissing_ThrowsException()
    {
        // Arrange - Unit has formula but no inverse formula
        var fromUnit = new Unit
        {
            Symbol = "K",
            Name = "kelvin",
            Category = "temperature",
            IsBaseUnit = true,
            ConversionFactor = 1.0,
            ConversionFormula = "x",
            ConversionInverseFormula = "x"
        };
        var toUnit = new Unit
        {
            Symbol = "°C",
            Name = "celsius",
            Category = "temperature",
            IsBaseUnit = false,
            ConversionFormula = "x + 273.15",
            ConversionInverseFormula = null // Missing inverse formula
        };
        var category = new Category
        {
            Name = "temperature",
            BaseUnit = fromUnit,
            Units = new List<Unit> { fromUnit, toUnit }
        };
        _mockRepository.Setup(r => r.GetCategoryByNameAsync("temperature"))
            .ReturnsAsync(category);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<UnitConversionException>(() =>
            _conversionService.ConvertAsync("temperature", "K", "°C", 273.15));
        Assert.Contains("Inverse formula is required", exception.Message);
    }

    [Fact]
    public async Task ConvertAsync_WhenUsingInverseFormula_ZeroKelvinToCelsius_ConvertsCorrectly()
    {
        // Arrange
        var fromUnit = new Unit
        {
            Symbol = "K",
            Name = "kelvin",
            Category = "temperature",
            IsBaseUnit = true,
            ConversionFactor = 1.0,
            ConversionFormula = "x",
            ConversionInverseFormula = "x"
        };
        var toUnit = new Unit
        {
            Symbol = "°C",
            Name = "celsius",
            Category = "temperature",
            IsBaseUnit = false,
            ConversionFormula = "x + 273.15",
            ConversionInverseFormula = "x - 273.15"
        };
        var category = new Category
        {
            Name = "temperature",
            BaseUnit = fromUnit,
            Units = new List<Unit> { fromUnit, toUnit }
        };
        _mockRepository.Setup(r => r.GetCategoryByNameAsync("temperature"))
            .ReturnsAsync(category);

        // Act
        var result = await _conversionService.ConvertAsync("temperature", "K", "°C", 0.0);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(-273.15, result.Result, 2); // Absolute zero
    }

    [Fact]
    public async Task ConvertAsync_WhenUsingInverseFormula_BaseUnitWithFormula_ConvertsCorrectly()
    {
        // Arrange - Base unit with formula "x" should work
        var baseUnit = new Unit
        {
            Symbol = "K",
            Name = "kelvin",
            Category = "temperature",
            IsBaseUnit = true,
            ConversionFactor = 1.0,
            ConversionFormula = "x",
            ConversionInverseFormula = "x"
        };
        var toUnit = new Unit
        {
            Symbol = "°C",
            Name = "celsius",
            Category = "temperature",
            IsBaseUnit = false,
            ConversionFormula = "x + 273.15",
            ConversionInverseFormula = "x - 273.15"
        };
        var category = new Category
        {
            Name = "temperature",
            BaseUnit = baseUnit,
            Units = new List<Unit> { baseUnit, toUnit }
        };
        _mockRepository.Setup(r => r.GetCategoryByNameAsync("temperature"))
            .ReturnsAsync(category);

        // Act
        var result = await _conversionService.ConvertAsync("temperature", "K", "°C", 373.15);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(100.0, result.Result, 2); // 373.15K = 100°C (boiling point)
    }
}
