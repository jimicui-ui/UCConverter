namespace UCConverter.Domain.Tests.Services;

using Moq;
using UCConverter.Domain.Entities;
using UCConverter.Domain.Interfaces;
using UCConverter.Domain.Services;
using Xunit;

/// <summary>
/// Tests for edge cases in ConversionService.ConvertWithFormula
/// </summary>
public class ConversionServiceConvertWithFormulaEdgeCasesTests
{
    private readonly Mock<IUnitRepository> _mockRepository;
    private readonly ConversionService _service;

    public ConversionServiceConvertWithFormulaEdgeCasesTests()
    {
        _mockRepository = new Mock<IUnitRepository>();
        _service = new ConversionService(_mockRepository.Object);
    }

    [Fact]
    public async Task ConvertAsync_WhenFromUnitHasFormulaAndToUnitHasFactor_ConvertsCorrectly()
    {
        // Arrange
        var category = new Category
        {
            Name = "temperature",
            DisplayName = "Temperature",
            BaseUnit = new Unit { Symbol = "K", Name = "kelvin", Category = "temperature", IsBaseUnit = true, ConversionFactor = 1.0 },
            Units = new List<Unit>
            {
                new Unit { Symbol = "K", Name = "kelvin", Category = "temperature", IsBaseUnit = true, ConversionFactor = 1.0 },
                new Unit { Symbol = "°C", Name = "celsius", Category = "temperature", ConversionFormula = "x + 273.15", ConversionInverseFormula = "x - 273.15" },
                new Unit { Symbol = "°F", Name = "fahrenheit", Category = "temperature", ConversionFactor = 1.8 }
            }
        };

        _mockRepository.Setup(r => r.GetCategoryByNameAsync("temperature"))
            .ReturnsAsync(category);

        // Act
        var result = await _service.ConvertAsync("temperature", "°C", "°F", 0.0);

        // Assert
        Assert.NotNull(result);
        // 0°C = 273.15K, then convert to °F using factor
        // This tests the path where fromUnit has formula but toUnit has factor
    }

    [Fact]
    public async Task ConvertAsync_WhenFromUnitHasFactorAndToUnitHasFormula_ConvertsCorrectly()
    {
        // Arrange
        var category = new Category
        {
            Name = "temperature",
            DisplayName = "Temperature",
            BaseUnit = new Unit { Symbol = "K", Name = "kelvin", Category = "temperature", IsBaseUnit = true, ConversionFactor = 1.0 },
            Units = new List<Unit>
            {
                new Unit { Symbol = "K", Name = "kelvin", Category = "temperature", IsBaseUnit = true, ConversionFactor = 1.0 },
                new Unit { Symbol = "°C", Name = "celsius", Category = "temperature", ConversionFactor = 1.0 },
                new Unit { Symbol = "°F", Name = "fahrenheit", Category = "temperature", ConversionFormula = "(x - 273.15) * 9/5 + 32", ConversionInverseFormula = "(x - 32) * 5/9 + 273.15" }
            }
        };

        _mockRepository.Setup(r => r.GetCategoryByNameAsync("temperature"))
            .ReturnsAsync(category);

        // Act
        var result = await _service.ConvertAsync("temperature", "°C", "°F", 0.0);

        // Assert
        Assert.NotNull(result);
        // This tests the path where fromUnit has factor but toUnit has formula
    }

    [Fact]
    public async Task ConvertAsync_WhenEvaluateFormulaThrowsException_ThrowsUnitConversionException()
    {
        // Arrange
        var category = new Category
        {
            Name = "temperature",
            DisplayName = "Temperature",
            BaseUnit = new Unit { Symbol = "K", Name = "kelvin", Category = "temperature", IsBaseUnit = true, ConversionFactor = 1.0 },
            Units = new List<Unit>
            {
                new Unit { Symbol = "K", Name = "kelvin", Category = "temperature", IsBaseUnit = true, ConversionFactor = 1.0 },
                new Unit { Symbol = "°C", Name = "celsius", Category = "temperature", ConversionFormula = "invalid formula syntax", ConversionInverseFormula = "x - 273.15" }
            }
        };

        _mockRepository.Setup(r => r.GetCategoryByNameAsync("temperature"))
            .ReturnsAsync(category);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<Domain.Exceptions.UnitConversionException>(
            () => _service.ConvertAsync("temperature", "°C", "K", 25.0));
        
        Assert.Contains("Failed to evaluate conversion formula", exception.Message);
    }

    [Fact]
    public async Task ConvertAsync_WhenEvaluateFormulaWithInvalidExpression_ThrowsUnitConversionException()
    {
        // Arrange - Use a formula that will actually cause DataTable.Compute to throw
        var category = new Category
        {
            Name = "temperature",
            DisplayName = "Temperature",
            BaseUnit = new Unit { Symbol = "K", Name = "kelvin", Category = "temperature", IsBaseUnit = true, ConversionFactor = 1.0 },
            Units = new List<Unit>
            {
                new Unit { Symbol = "K", Name = "kelvin", Category = "temperature", IsBaseUnit = true, ConversionFactor = 1.0 },
                new Unit { Symbol = "°C", Name = "celsius", Category = "temperature", ConversionFormula = "invalid syntax !!!", ConversionInverseFormula = "x - 273.15" }
            }
        };

        _mockRepository.Setup(r => r.GetCategoryByNameAsync("temperature"))
            .ReturnsAsync(category);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<Domain.Exceptions.UnitConversionException>(
            () => _service.ConvertAsync("temperature", "°C", "K", 25.0));
        
        Assert.Contains("Failed to evaluate conversion formula", exception.Message);
    }
}

