namespace UCConverter.Domain.Tests.Services;

using Moq;
using UCConverter.Domain.Entities;
using UCConverter.Domain.Exceptions;
using UCConverter.Domain.Interfaces;
using UCConverter.Domain.Services;
using Xunit;

public class ConversionServiceCategoryValidationTests
{
    private readonly Mock<IUnitRepository> _mockRepository;
    private readonly ConversionService _conversionService;

    public ConversionServiceCategoryValidationTests()
    {
        _mockRepository = new Mock<IUnitRepository>();
        _conversionService = new ConversionService(_mockRepository.Object);
    }

    [Fact]
    public async Task ConvertAsync_WhenUnitsAreFromDifferentCategories_ThrowsInvalidConversionException()
    {
        // Arrange - Units from different categories (both in the category list but with different Category property)
        var fromUnit = new Unit
        {
            Symbol = "m",
            Name = "meter",
            Category = "length",
            IsBaseUnit = true,
            ConversionFactor = 1.0
        };
        var toUnit = new Unit
        {
            Symbol = "kg",
            Name = "kilogram",
            Category = "weight", // Different category
            IsBaseUnit = true,
            ConversionFactor = 1.0
        };
        // Both units are in the category's units list, but have different Category properties
        var category = new Category
        {
            Name = "length",
            BaseUnit = fromUnit,
            Units = new List<Unit> { fromUnit, toUnit }
        };
        _mockRepository.Setup(r => r.GetCategoryByNameAsync("length"))
            .ReturnsAsync(category);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<InvalidConversionException>(() =>
            _conversionService.ConvertAsync("length", "m", "kg", 10.0));
        Assert.Equal("m", exception.FromUnit);
        Assert.Equal("kg", exception.ToUnit);
        Assert.Equal("length", exception.Category);
    }
}

