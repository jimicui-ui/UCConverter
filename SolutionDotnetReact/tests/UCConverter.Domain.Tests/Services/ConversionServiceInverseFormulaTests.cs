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
    public async Task ConvertAsync_WhenInverseFormulaWithContainsPattern_ConvertsCorrectly()
    {
        // Arrange - Formula contains "x + 273.15" but not "x -"
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
            Symbol = "C",
            Name = "celsius",
            Category = "test",
            IsBaseUnit = false,
            ConversionFormula = "x + 273.15" // Matches first condition
        };
        var category = new Category
        {
            Name = "test",
            Units = new List<Unit> { fromUnit, toUnit }
        };
        _mockRepository.Setup(r => r.GetCategoryByNameAsync("test"))
            .ReturnsAsync(category);

        // Act
        var result = await _conversionService.ConvertAsync("test", "K", "C", 298.15);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(25.0, result.Result, 2);
    }

    [Fact]
    public async Task ConvertAsync_WhenInverseFormulaWithExactTrimMatch_ConvertsCorrectly()
    {
        // Arrange - Formula exactly "x + 273.15" (with trim)
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
            Symbol = "C",
            Name = "celsius",
            Category = "test",
            IsBaseUnit = false,
            ConversionFormula = "x + 273.15" // Exact match after trim
        };
        var category = new Category
        {
            Name = "test",
            Units = new List<Unit> { fromUnit, toUnit }
        };
        _mockRepository.Setup(r => r.GetCategoryByNameAsync("test"))
            .ReturnsAsync(category);

        // Act
        var result = await _conversionService.ConvertAsync("test", "K", "C", 273.15);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(0.0, result.Result, 2);
    }

    [Fact]
    public async Task ConvertAsync_WhenInverseFormulaWithComplexFormula_ThrowsException()
    {
        // Arrange - Complex formula that doesn't match simple patterns
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
            Symbol = "F",
            Name = "fahrenheit",
            Category = "test",
            IsBaseUnit = false,
            ConversionFormula = "(x - 32) * 5/9 + 273.15" // Complex formula
        };
        var category = new Category
        {
            Name = "test",
            Units = new List<Unit> { fromUnit, toUnit }
        };
        _mockRepository.Setup(r => r.GetCategoryByNameAsync("test"))
            .ReturnsAsync(category);

        // Act & Assert
        await Assert.ThrowsAsync<UnitConversionException>(() =>
            _conversionService.ConvertAsync("test", "K", "F", 273.15));
    }

    [Fact]
    public async Task ConvertAsync_WhenInverseFormulaContainsXMinus_ThrowsException()
    {
        // Arrange - Formula contains "x -" which doesn't match first condition
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
            Symbol = "F",
            Name = "fahrenheit",
            Category = "test",
            IsBaseUnit = false,
            ConversionFormula = "x - 273.15" // Contains "x -"
        };
        var category = new Category
        {
            Name = "test",
            Units = new List<Unit> { fromUnit, toUnit }
        };
        _mockRepository.Setup(r => r.GetCategoryByNameAsync("test"))
            .ReturnsAsync(category);

        // Act & Assert - Should throw because it doesn't match simple patterns
        await Assert.ThrowsAsync<UnitConversionException>(() =>
            _conversionService.ConvertAsync("test", "K", "F", 273.15));
    }
}

