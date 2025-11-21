namespace UCConverter.Domain.Tests.Services;

using Moq;
using UCConverter.Domain.Entities;
using UCConverter.Domain.Exceptions;
using UCConverter.Domain.Interfaces;
using UCConverter.Domain.Services;
using Xunit;

public class ConversionServiceEdgeCasesTests
{
    private readonly Mock<IUnitRepository> _mockRepository;
    private readonly ConversionService _conversionService;

    public ConversionServiceEdgeCasesTests()
    {
        _mockRepository = new Mock<IUnitRepository>();
        _conversionService = new ConversionService(_mockRepository.Object);
    }

    [Fact]
    public async Task ConvertAsync_WhenCategoryIsWhitespace_ThrowsArgumentException()
    {
        // Act & Assert
        await Assert.ThrowsAsync<ArgumentException>(() =>
            _conversionService.ConvertAsync("   ", "m", "km", 10.0));
    }

    [Fact]
    public async Task ConvertAsync_WhenFromUnitIsWhitespace_ThrowsArgumentException()
    {
        // Act & Assert
        await Assert.ThrowsAsync<ArgumentException>(() =>
            _conversionService.ConvertAsync("length", "   ", "km", 10.0));
    }

    [Fact]
    public async Task ConvertAsync_WhenToUnitIsWhitespace_ThrowsArgumentException()
    {
        // Act & Assert
        await Assert.ThrowsAsync<ArgumentException>(() =>
            _conversionService.ConvertAsync("length", "m", "   ", 10.0));
    }

    [Fact]
    public async Task ConvertAsync_WhenValueIsZero_ReturnsZero()
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
        var toUnit = new Unit
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
            Units = new List<Unit> { fromUnit, toUnit }
        };
        _mockRepository.Setup(r => r.GetCategoryByNameAsync("length"))
            .ReturnsAsync(category);

        // Act
        var result = await _conversionService.ConvertAsync("length", "m", "km", 0.0);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(0.0, result.Result);
    }

    [Fact]
    public async Task ConvertAsync_WhenValueIsNegative_ConvertsCorrectly()
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
        var toUnit = new Unit
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
            Units = new List<Unit> { fromUnit, toUnit }
        };
        _mockRepository.Setup(r => r.GetCategoryByNameAsync("length"))
            .ReturnsAsync(category);

        // Act
        var result = await _conversionService.ConvertAsync("length", "m", "km", -1000.0);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(-1.0, result.Result, 4);
    }

    [Fact]
    public async Task ConvertAsync_WhenValueIsVeryLarge_ConvertsCorrectly()
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
        var toUnit = new Unit
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
            Units = new List<Unit> { fromUnit, toUnit }
        };
        _mockRepository.Setup(r => r.GetCategoryByNameAsync("length"))
            .ReturnsAsync(category);

        // Act
        var result = await _conversionService.ConvertAsync("length", "m", "km", 1000000.0);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(1000.0, result.Result, 4);
    }

    [Fact]
    public async Task ConvertBatchAsync_WhenEmptyTargetUnits_ReturnsEmpty()
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

        // Act
        var results = await _conversionService.ConvertBatchAsync("length", "m", Enumerable.Empty<string>(), 10.0);

        // Assert
        Assert.NotNull(results);
        Assert.Empty(results);
    }
}

