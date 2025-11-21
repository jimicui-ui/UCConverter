namespace UCConverter.Domain.Tests.Services;

using Moq;
using UCConverter.Domain.Entities;
using UCConverter.Domain.Exceptions;
using UCConverter.Domain.Interfaces;
using UCConverter.Domain.Services;
using Xunit;

public class ConversionServiceTests
{
    private readonly Mock<IUnitRepository> _mockRepository;
    private readonly ConversionService _conversionService;

    public ConversionServiceTests()
    {
        _mockRepository = new Mock<IUnitRepository>();
        _conversionService = new ConversionService(_mockRepository.Object);
    }

    [Fact]
    public async Task ConvertAsync_WhenCategoryNotFound_ThrowsCategoryNotFoundException()
    {
        // Arrange
        _mockRepository.Setup(r => r.GetCategoryByNameAsync("invalid"))
            .ReturnsAsync((Category?)null);

        // Act & Assert
        await Assert.ThrowsAsync<CategoryNotFoundException>(() =>
            _conversionService.ConvertAsync("invalid", "m", "km", 10.0));
    }

    [Fact]
    public async Task ConvertAsync_WhenFromUnitNotFound_ThrowsUnitNotFoundException()
    {
        // Arrange
        var category = new Category
        {
            Name = "length",
            Units = new List<Unit> { new Unit { Symbol = "km", Name = "kilometer" } }
        };
        _mockRepository.Setup(r => r.GetCategoryByNameAsync("length"))
            .ReturnsAsync(category);

        // Act & Assert
        await Assert.ThrowsAsync<UnitNotFoundException>(() =>
            _conversionService.ConvertAsync("length", "m", "km", 10.0));
    }

    [Fact]
    public async Task ConvertAsync_WhenToUnitNotFound_ThrowsUnitNotFoundException()
    {
        // Arrange
        var category = new Category
        {
            Name = "length",
            Units = new List<Unit> { new Unit { Symbol = "m", Name = "meter" } }
        };
        _mockRepository.Setup(r => r.GetCategoryByNameAsync("length"))
            .ReturnsAsync(category);

        // Act & Assert
        await Assert.ThrowsAsync<UnitNotFoundException>(() =>
            _conversionService.ConvertAsync("length", "m", "km", 10.0));
    }

    [Fact]
    public async Task ConvertAsync_WhenUnitsInDifferentCategories_ThrowsInvalidConversionException()
    {
        // Arrange
        var fromUnit = new Unit { Symbol = "m", Name = "meter", Category = "length" };
        var toUnit = new Unit { Symbol = "kg", Name = "kilogram", Category = "weight" };
        var category = new Category
        {
            Name = "length",
            Units = new List<Unit> { fromUnit, toUnit }
        };
        _mockRepository.Setup(r => r.GetCategoryByNameAsync("length"))
            .ReturnsAsync(category);

        // Act & Assert
        await Assert.ThrowsAsync<InvalidConversionException>(() =>
            _conversionService.ConvertAsync("length", "m", "kg", 10.0));
    }

    [Fact]
    public async Task ConvertAsync_WhenValidLinearConversion_ReturnsResult()
    {
        // Arrange
        var fromUnit = new Unit
        {
            Symbol = "m",
            Name = "meter",
            Category = "length",
            IsBaseUnit = false,
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
        var result = await _conversionService.ConvertAsync("length", "m", "km", 1000.0);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(1.0, result.Result, 4);
        Assert.Equal(fromUnit, result.FromUnit);
        Assert.Equal(toUnit, result.ToUnit);
        Assert.Equal(1000.0, result.OriginalValue);
    }

    [Fact]
    public async Task ConvertAsync_WhenFromUnitIsBaseUnit_ReturnsResult()
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
        var result = await _conversionService.ConvertAsync("length", "m", "km", 1000.0);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(1.0, result.Result, 4);
    }

    [Fact]
    public async Task ConvertAsync_WhenToUnitIsBaseUnit_ReturnsResult()
    {
        // Arrange
        var fromUnit = new Unit
        {
            Symbol = "km",
            Name = "kilometer",
            Category = "length",
            IsBaseUnit = false,
            ConversionFactor = 1000.0
        };
        var toUnit = new Unit
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
            Units = new List<Unit> { fromUnit, toUnit }
        };
        _mockRepository.Setup(r => r.GetCategoryByNameAsync("length"))
            .ReturnsAsync(category);

        // Act
        var result = await _conversionService.ConvertAsync("length", "km", "m", 1.0);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(1000.0, result.Result, 4);
    }

    [Fact]
    public async Task ConvertAsync_WhenBothUnitsAreBaseUnit_ReturnsResult()
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
            Symbol = "m",
            Name = "meter",
            Category = "length",
            IsBaseUnit = true,
            ConversionFactor = 1.0
        };
        var category = new Category
        {
            Name = "length",
            Units = new List<Unit> { fromUnit, toUnit }
        };
        _mockRepository.Setup(r => r.GetCategoryByNameAsync("length"))
            .ReturnsAsync(category);

        // Act
        var result = await _conversionService.ConvertAsync("length", "m", "m", 10.0);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(10.0, result.Result, 4);
    }

    [Fact]
    public async Task ConvertAsync_WhenCategoryIsNull_ThrowsArgumentException()
    {
        // Act & Assert
        await Assert.ThrowsAsync<ArgumentException>(() =>
            _conversionService.ConvertAsync(null!, "m", "km", 10.0));
    }

    [Fact]
    public async Task ConvertAsync_WhenFromUnitIsNull_ThrowsArgumentException()
    {
        // Act & Assert
        await Assert.ThrowsAsync<ArgumentException>(() =>
            _conversionService.ConvertAsync("length", null!, "km", 10.0));
    }

    [Fact]
    public async Task ConvertAsync_WhenToUnitIsNull_ThrowsArgumentException()
    {
        // Act & Assert
        await Assert.ThrowsAsync<ArgumentException>(() =>
            _conversionService.ConvertAsync("length", "m", null!, 10.0));
    }

    [Fact]
    public async Task ConvertBatchAsync_WhenValid_ReturnsMultipleResults()
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
        var resultsList = results.ToList();
        Assert.Equal(2, resultsList.Count);
    }

    [Fact]
    public async Task ConvertBatchAsync_WhenSomeConversionsFail_ReturnsOnlySuccessful()
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

        // Act - one valid unit, one invalid
        var results = await _conversionService.ConvertBatchAsync("length", "m", new[] { "km", "invalid" }, 1000.0);

        // Assert
        Assert.NotNull(results);
        var resultsList = results.ToList();
        Assert.Single(resultsList); // Only successful conversion
    }
}

