namespace UCConverter.Domain.Tests.Services;

using Moq;
using UCConverter.Domain.Entities;
using UCConverter.Domain.Exceptions;
using UCConverter.Domain.Interfaces;
using UCConverter.Domain.Services;
using Xunit;

public class ConversionServiceBatchTests
{
    private readonly Mock<IUnitRepository> _mockRepository;
    private readonly ConversionService _conversionService;

    public ConversionServiceBatchTests()
    {
        _mockRepository = new Mock<IUnitRepository>();
        _conversionService = new ConversionService(_mockRepository.Object);
    }

    [Fact]
    public async Task ConvertBatchAsync_WhenSomeConversionsFail_ContinuesWithOthers()
    {
        // Arrange
        var fromUnit = new Unit
        {
            Symbol = "m",
            Name = "meter",
            Category = "length",
            IsBaseUnit = true,
            ConversionFactor = 1.0
        };
        var toUnit1 = new Unit
        {
            Symbol = "km",
            Name = "kilometer",
            Category = "length",
            IsBaseUnit = false,
            ConversionFactor = 1000.0
        };
        var category = new Category
        {
            Name = "length",
            Units = new List<Unit> { fromUnit, toUnit1 }
        };
        _mockRepository.Setup(r => r.GetCategoryByNameAsync("length"))
            .ReturnsAsync(category);

        // Act - One valid unit, one invalid unit
        var results = await _conversionService.ConvertBatchAsync("length", "m", new[] { "km", "invalid" }, 1000.0);

        // Assert - Should return only successful conversions
        Assert.NotNull(results);
        Assert.Single(results);
        Assert.Equal("km", results.First().ToUnit.Symbol);
    }

    [Fact]
    public async Task ConvertBatchAsync_WhenAllConversionsFail_ReturnsEmpty()
    {
        // Arrange
        var fromUnit = new Unit
        {
            Symbol = "m",
            Name = "meter",
            Category = "length",
            IsBaseUnit = true,
            ConversionFactor = 1.0
        };
        var category = new Category
        {
            Name = "length",
            Units = new List<Unit> { fromUnit }
        };
        _mockRepository.Setup(r => r.GetCategoryByNameAsync("length"))
            .ReturnsAsync(category);

        // Act - All invalid units
        var results = await _conversionService.ConvertBatchAsync("length", "m", new[] { "invalid1", "invalid2" }, 1000.0);

        // Assert - Should return empty list
        Assert.NotNull(results);
        Assert.Empty(results);
    }

    [Fact]
    public async Task ConvertBatchAsync_WhenAllConversionsSucceed_ReturnsAllResults()
    {
        // Arrange
        var fromUnit = new Unit
        {
            Symbol = "m",
            Name = "meter",
            Category = "length",
            IsBaseUnit = true,
            ConversionFactor = 1.0
        };
        var toUnit1 = new Unit
        {
            Symbol = "km",
            Name = "kilometer",
            Category = "length",
            IsBaseUnit = false,
            ConversionFactor = 1000.0
        };
        var toUnit2 = new Unit
        {
            Symbol = "cm",
            Name = "centimeter",
            Category = "length",
            IsBaseUnit = false,
            ConversionFactor = 0.01
        };
        var category = new Category
        {
            Name = "length",
            Units = new List<Unit> { fromUnit, toUnit1, toUnit2 }
        };
        _mockRepository.Setup(r => r.GetCategoryByNameAsync("length"))
            .ReturnsAsync(category);

        // Act
        var results = await _conversionService.ConvertBatchAsync("length", "m", new[] { "km", "cm" }, 1000.0);

        // Assert
        Assert.NotNull(results);
        Assert.Equal(2, results.Count());
        Assert.Contains(results, r => r.ToUnit.Symbol == "km");
        Assert.Contains(results, r => r.ToUnit.Symbol == "cm");
    }
}

