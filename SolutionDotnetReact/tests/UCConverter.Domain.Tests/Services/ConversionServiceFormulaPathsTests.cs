namespace UCConverter.Domain.Tests.Services;

using Moq;
using UCConverter.Domain.Entities;
using UCConverter.Domain.Exceptions;
using UCConverter.Domain.Interfaces;
using UCConverter.Domain.Services;
using Xunit;

public class ConversionServiceFormulaPathsTests
{
    private readonly Mock<IUnitRepository> _mockRepository;
    private readonly ConversionService _conversionService;

    public ConversionServiceFormulaPathsTests()
    {
        _mockRepository = new Mock<IUnitRepository>();
        _conversionService = new ConversionService(_mockRepository.Object);
    }

    [Fact]
    public async Task ConvertAsync_WhenFromUnitHasFormulaAndToUnitHasFactor_ConvertsCorrectly()
    {
        // Arrange - From unit has formula, to unit has factor
        var fromUnit = new Unit
        {
            Symbol = "°C",
            Name = "celsius",
            Category = "test",
            IsBaseUnit = false,
            ConversionFormula = "x + 273.15"
        };
        var toUnit = new Unit
        {
            Symbol = "K",
            Name = "kelvin",
            Category = "test",
            IsBaseUnit = true,
            ConversionFactor = 1.0
        };
        var category = new Category
        {
            Name = "test",
            BaseUnit = toUnit,
            Units = new List<Unit> { fromUnit, toUnit }
        };
        _mockRepository.Setup(r => r.GetCategoryByNameAsync("test"))
            .ReturnsAsync(category);

        // Act
        var result = await _conversionService.ConvertAsync("test", "°C", "K", 25.0);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(298.15, result.Result, 2);
    }

    [Fact]
    public async Task ConvertAsync_WhenFromUnitHasFactorAndToUnitHasFormula_ConvertsCorrectly()
    {
        // Arrange - From unit has factor, to unit has formula
        var fromUnit = new Unit
        {
            Symbol = "K",
            Name = "kelvin",
            Category = "test",
            IsBaseUnit = true,
            ConversionFactor = 1.0
        };
        var toUnit = new Unit
        {
            Symbol = "°C",
            Name = "celsius",
            Category = "test",
            IsBaseUnit = false,
            ConversionFormula = "x + 273.15",
            ConversionInverseFormula = "x - 273.15" // Required inverse formula
        };
        var category = new Category
        {
            Name = "test",
            BaseUnit = fromUnit,
            Units = new List<Unit> { fromUnit, toUnit }
        };
        _mockRepository.Setup(r => r.GetCategoryByNameAsync("test"))
            .ReturnsAsync(category);

        // Act
        var result = await _conversionService.ConvertAsync("test", "K", "°C", 298.15);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(25.0, result.Result, 2);
    }

    [Fact]
    public async Task ConvertAsync_WhenFormulaEvaluationFails_ThrowsUnitConversionException()
    {
        // Arrange - Invalid formula that will cause DataTable.Compute to fail
        var fromUnit = new Unit
        {
            Symbol = "invalid",
            Name = "invalid",
            Category = "test",
            IsBaseUnit = false,
            ConversionFormula = "x + invalid syntax !!!"
        };
        var toUnit = new Unit
        {
            Symbol = "K",
            Name = "kelvin",
            Category = "test",
            IsBaseUnit = true,
            ConversionFactor = 1.0
        };
        var category = new Category
        {
            Name = "test",
            BaseUnit = toUnit,
            Units = new List<Unit> { fromUnit, toUnit }
        };
        _mockRepository.Setup(r => r.GetCategoryByNameAsync("test"))
            .ReturnsAsync(category);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<UnitConversionException>(() =>
            _conversionService.ConvertAsync("test", "invalid", "K", 10.0));
        Assert.Contains("Failed to evaluate conversion formula", exception.Message);
    }

    [Fact]
    public async Task ConvertAsync_WhenInverseFormulaDoesNotMatchPattern_ThrowsUnitConversionException()
    {
        // Arrange - Formula that doesn't match any inverse pattern
        var fromUnit = new Unit
        {
            Symbol = "K",
            Name = "kelvin",
            Category = "test",
            IsBaseUnit = true,
            ConversionFactor = 1.0
        };
        var toUnit = new Unit
        {
            Symbol = "complex",
            Name = "complex",
            Category = "test",
            IsBaseUnit = false,
            ConversionFormula = "x * 2 + 5" // Doesn't match inverse patterns
        };
        var category = new Category
        {
            Name = "test",
            BaseUnit = fromUnit,
            Units = new List<Unit> { fromUnit, toUnit }
        };
        _mockRepository.Setup(r => r.GetCategoryByNameAsync("test"))
            .ReturnsAsync(category);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<UnitConversionException>(() =>
            _conversionService.ConvertAsync("test", "K", "complex", 273.15));
        Assert.Contains("Inverse formula is required for unit with formula", exception.Message);
    }
}

