namespace UCConverter.Domain.Tests.Services;

using Moq;
using UCConverter.Domain.Entities;
using UCConverter.Domain.Exceptions;
using UCConverter.Domain.Interfaces;
using UCConverter.Domain.Services;
using Xunit;

public class ConversionServiceFormulaTests
{
    private readonly Mock<IUnitRepository> _mockRepository;
    private readonly ConversionService _conversionService;

    public ConversionServiceFormulaTests()
    {
        _mockRepository = new Mock<IUnitRepository>();
        _conversionService = new ConversionService(_mockRepository.Object);
    }

    [Fact]
    public async Task ConvertAsync_WhenFromUnitHasFormula_ConvertsThroughBaseUnit()
    {
        // Arrange - Celsius to Kelvin (Celsius has formula)
        var fromUnit = new Unit
        {
            Symbol = "°C",
            Name = "celsius",
            Category = "temperature",
            IsBaseUnit = false,
            ConversionFormula = "x + 273.15"
        };
        var toUnit = new Unit
        {
            Symbol = "K",
            Name = "kelvin",
            Category = "temperature",
            IsBaseUnit = true,
            ConversionFactor = 1.0
        };
        var category = new Category
        {
            Name = "temperature",
            Units = new List<Unit> { fromUnit, toUnit }
        };
        _mockRepository.Setup(r => r.GetCategoryByNameAsync("temperature"))
            .ReturnsAsync(category);

        // Act
        var result = await _conversionService.ConvertAsync("temperature", "°C", "K", 25.0);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(298.15, result.Result, 2);
        // Formula may be null when converting to base unit
    }

    [Fact]
    public async Task ConvertAsync_WhenToUnitHasFormula_ConvertsThroughBaseUnit()
    {
        // Arrange - Kelvin to Celsius (Celsius has formula)
        var fromUnit = new Unit
        {
            Symbol = "K",
            Name = "kelvin",
            Category = "temperature",
            IsBaseUnit = true,
            ConversionFactor = 1.0
        };
        var toUnit = new Unit
        {
            Symbol = "°C",
            Name = "celsius",
            Category = "temperature",
            IsBaseUnit = false,
            ConversionFormula = "x + 273.15"
        };
        var category = new Category
        {
            Name = "temperature",
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
    public async Task ConvertAsync_WhenBothUnitsHaveFormulas_ConvertsThroughBaseUnit()
    {
        // Arrange - Celsius to Fahrenheit (both have formulas with inverse formulas)
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

        // Act - This should now work with explicit inverse formulas
        var result = await _conversionService.ConvertAsync("temperature", "°C", "°F", 25.0);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(77.0, result.Result, 1); // 25°C = 77°F
    }

    [Fact]
    public async Task ConvertAsync_WhenFromUnitHasFormulaAndToUnitHasFactor_ConvertsCorrectly()
    {
        // Arrange - Celsius (formula) to a unit with factor
        var fromUnit = new Unit
        {
            Symbol = "°C",
            Name = "celsius",
            Category = "temperature",
            IsBaseUnit = false,
            ConversionFormula = "x + 273.15"
        };
        var toUnit = new Unit
        {
            Symbol = "K",
            Name = "kelvin",
            Category = "temperature",
            IsBaseUnit = true,
            ConversionFactor = 1.0
        };
        var category = new Category
        {
            Name = "temperature",
            Units = new List<Unit> { fromUnit, toUnit }
        };
        _mockRepository.Setup(r => r.GetCategoryByNameAsync("temperature"))
            .ReturnsAsync(category);

        // Act
        var result = await _conversionService.ConvertAsync("temperature", "°C", "K", 0.0);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(273.15, result.Result, 2);
    }

    [Fact]
    public async Task ConvertAsync_WhenFromUnitHasFactorAndToUnitHasFormula_ConvertsCorrectly()
    {
        // Arrange - Base unit (factor) to Celsius (formula)
        var fromUnit = new Unit
        {
            Symbol = "K",
            Name = "kelvin",
            Category = "temperature",
            IsBaseUnit = true,
            ConversionFactor = 1.0
        };
        var toUnit = new Unit
        {
            Symbol = "°C",
            Name = "celsius",
            Category = "temperature",
            IsBaseUnit = false,
            ConversionFormula = "x + 273.15"
        };
        var category = new Category
        {
            Name = "temperature",
            Units = new List<Unit> { fromUnit, toUnit }
        };
        _mockRepository.Setup(r => r.GetCategoryByNameAsync("temperature"))
            .ReturnsAsync(category);

        // Act
        var result = await _conversionService.ConvertAsync("temperature", "K", "°C", 273.15);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(0.0, result.Result, 2);
    }
}
