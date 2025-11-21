namespace UCConverter.Domain.Tests.Services;

using Moq;
using UCConverter.Domain.Entities;
using UCConverter.Domain.Exceptions;
using UCConverter.Domain.Interfaces;
using UCConverter.Domain.Services;
using Xunit;

public class ConversionServiceFormulaEvaluationTests
{
    private readonly Mock<IUnitRepository> _mockRepository;
    private readonly ConversionService _conversionService;

    public ConversionServiceFormulaEvaluationTests()
    {
        _mockRepository = new Mock<IUnitRepository>();
        _conversionService = new ConversionService(_mockRepository.Object);
    }

    [Fact]
    public async Task ConvertAsync_WhenFormulaEvaluationFails_ThrowsUnitConversionException()
    {
        // Arrange - Invalid formula that will fail evaluation
        var fromUnit = new Unit
        {
            Symbol = "invalid",
            Name = "invalid",
            Category = "test",
            IsBaseUnit = false,
            ConversionFormula = "invalid formula syntax !!!"
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
            Units = new List<Unit> { fromUnit, toUnit }
        };
        _mockRepository.Setup(r => r.GetCategoryByNameAsync("test"))
            .ReturnsAsync(category);

        // Act & Assert
        await Assert.ThrowsAsync<UnitConversionException>(() =>
            _conversionService.ConvertAsync("test", "invalid", "K", 10.0));
    }

    [Fact]
    public async Task ConvertAsync_WhenComplexInverseFormula_ThrowsUnitConversionException()
    {
        // Arrange - Complex formula that inverse is not supported
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
            _conversionService.ConvertAsync("test", "K", "complex", 273.15));
    }

    [Fact]
    public async Task ConvertAsync_WhenInverseFormulaWithExactMatch_ConvertsCorrectly()
    {
        // Arrange - Formula that matches exact pattern "x + 273.15"
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
            ConversionFormula = "x + 273.15" // Exact match for inverse
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
}

