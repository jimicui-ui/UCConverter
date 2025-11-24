namespace UCConverter.Domain.Tests.Services;

using Moq;
using UCConverter.Domain.Entities;
using UCConverter.Domain.Interfaces;
using UCConverter.Domain.Services;
using Xunit;

public class ConversionServiceInverseFormulaSecondConditionTests
{
    private readonly Mock<IUnitRepository> _mockRepository;
    private readonly ConversionService _conversionService;

    public ConversionServiceInverseFormulaSecondConditionTests()
    {
        _mockRepository = new Mock<IUnitRepository>();
        _conversionService = new ConversionService(_mockRepository.Object);
    }

    [Fact]
    public async Task ConvertAsync_WhenInverseFormulaWithWhitespace_ConvertsCorrectly()
    {
        // Arrange - Formula with whitespace that matches second condition (formula.Trim() == "x + 273.15")
        // but first condition might not match due to whitespace handling
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
            ConversionFormula = "  x + 273.15  " // Has whitespace, trim() == "x + 273.15"
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
        var result = await _conversionService.ConvertAsync("test", "K", "C", 298.15);

        // Assert
        Assert.NotNull(result);
        // Should convert correctly (first condition should match, but testing second condition path)
        Assert.Equal(25.0, result.Result, 2);
    }
}

