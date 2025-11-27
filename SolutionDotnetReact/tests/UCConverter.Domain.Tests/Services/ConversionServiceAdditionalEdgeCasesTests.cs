namespace UCConverter.Domain.Tests.Services;

using Moq;
using UCConverter.Domain.Entities;
using UCConverter.Domain.Exceptions;
using UCConverter.Domain.Interfaces;
using UCConverter.Domain.Services;
using Xunit;

/// <summary>
/// Additional edge case tests to ensure 100% coverage
/// </summary>
public class ConversionServiceAdditionalEdgeCasesTests
{
    private readonly Mock<IUnitRepository> _mockRepository;
    private readonly ConversionService _service;

    public ConversionServiceAdditionalEdgeCasesTests()
    {
        _mockRepository = new Mock<IUnitRepository>();
        _service = new ConversionService(_mockRepository.Object);
    }

    [Fact]
    public async Task ConvertAsync_WhenCategoryNameIsWhitespace_ThrowsArgumentException()
    {
        // Act & Assert
        var ex = await Assert.ThrowsAsync<ArgumentException>(() =>
            _service.ConvertAsync("   ", "m", "km", 1000.0));
        Assert.Equal("categoryName", ex.ParamName);
    }

    [Fact]
    public async Task ConvertAsync_WhenFromUnitSymbolIsWhitespace_ThrowsArgumentException()
    {
        // Arrange
        var category = new Category
        {
            Name = "length",
            DisplayName = "Length",
            Group = "Common",
            BaseUnit = new Unit { Symbol = "m", Name = "meter", Category = "length", IsBaseUnit = true },
            Units = new List<Unit>
            {
                new Unit { Symbol = "m", Name = "meter", Category = "length", IsBaseUnit = true, ConversionFactor = 1.0 }
            }
        };

        _mockRepository.Setup(r => r.GetCategoryByNameAsync("length"))
            .ReturnsAsync(category);

        // Act & Assert
        var ex = await Assert.ThrowsAsync<ArgumentException>(() =>
            _service.ConvertAsync("length", "   ", "km", 1000.0));
        Assert.Equal("fromUnitSymbol", ex.ParamName);
    }

    [Fact]
    public async Task ConvertAsync_WhenToUnitSymbolIsWhitespace_ThrowsArgumentException()
    {
        // Arrange
        var category = new Category
        {
            Name = "length",
            DisplayName = "Length",
            Group = "Common",
            BaseUnit = new Unit { Symbol = "m", Name = "meter", Category = "length", IsBaseUnit = true },
            Units = new List<Unit>
            {
                new Unit { Symbol = "m", Name = "meter", Category = "length", IsBaseUnit = true, ConversionFactor = 1.0 }
            }
        };

        _mockRepository.Setup(r => r.GetCategoryByNameAsync("length"))
            .ReturnsAsync(category);

        // Act & Assert
        var ex = await Assert.ThrowsAsync<ArgumentException>(() =>
            _service.ConvertAsync("length", "m", "   ", 1000.0));
        Assert.Equal("toUnitSymbol", ex.ParamName);
    }

    [Fact]
    public async Task ConvertBatchAsync_WhenEmptyToUnitSymbols_ReturnsEmpty()
    {
        // Act
        var result = await _service.ConvertBatchAsync("length", "m", Array.Empty<string>(), 1000.0);

        // Assert
        Assert.NotNull(result);
        Assert.Empty(result);
    }

    [Fact]
    public async Task ConvertBatchAsync_WhenNullToUnitSymbols_ThrowsNullReferenceException()
    {
        // Act & Assert - Null collection will throw NullReferenceException when iterating
        await Assert.ThrowsAsync<NullReferenceException>(() =>
            _service.ConvertBatchAsync("length", "m", null!, 1000.0));
    }

    [Fact]
    public async Task ConvertBatchAsync_WhenSomeConversionsFail_ContinuesWithOthers()
    {
        // Arrange
        var category = new Category
        {
            Name = "length",
            DisplayName = "Length",
            Group = "Common",
            BaseUnit = new Unit { Symbol = "m", Name = "meter", Category = "length", IsBaseUnit = true },
            Units = new List<Unit>
            {
                new Unit { Symbol = "m", Name = "meter", Category = "length", IsBaseUnit = true, ConversionFactor = 1.0 },
                new Unit { Symbol = "km", Name = "kilometer", Category = "length", IsBaseUnit = false, ConversionFactor = 1000.0 },
                new Unit { Symbol = "ft", Name = "foot", Category = "length", IsBaseUnit = false, ConversionFactor = 0.3048 }
            }
        };

        _mockRepository.Setup(r => r.GetCategoryByNameAsync("length"))
            .ReturnsAsync(category);

        var toUnitSymbols = new[] { "km", "invalid", "ft" };

        // Act
        var result = await _service.ConvertBatchAsync("length", "m", toUnitSymbols, 1000.0);

        // Assert
        Assert.NotNull(result);
        var resultList = result.ToList();
        Assert.Equal(2, resultList.Count); // Only successful conversions
        Assert.Equal("km", resultList[0].ToUnit.Symbol);
        Assert.Equal("ft", resultList[1].ToUnit.Symbol);
    }

    [Fact]
    public async Task ConvertBatchAsync_WhenAllConversionsFail_ReturnsEmpty()
    {
        // Arrange
        var category = new Category
        {
            Name = "length",
            DisplayName = "Length",
            Group = "Common",
            BaseUnit = new Unit { Symbol = "m", Name = "meter", Category = "length", IsBaseUnit = true },
            Units = new List<Unit>
            {
                new Unit { Symbol = "m", Name = "meter", Category = "length", IsBaseUnit = true, ConversionFactor = 1.0 }
            }
        };

        _mockRepository.Setup(r => r.GetCategoryByNameAsync("length"))
            .ReturnsAsync(category);

        var toUnitSymbols = new[] { "invalid1", "invalid2" };

        // Act
        var result = await _service.ConvertBatchAsync("length", "m", toUnitSymbols, 1000.0);

        // Assert
        Assert.NotNull(result);
        Assert.Empty(result);
    }

    [Fact]
    public async Task ConvertAsync_WhenResultIsNegative_FormatsCorrectly()
    {
        // Arrange
        var category = new Category
        {
            Name = "temperature",
            DisplayName = "Temperature",
            Group = "Common",
            BaseUnit = new Unit { Symbol = "K", Name = "kelvin", Category = "temperature", IsBaseUnit = true },
            Units = new List<Unit>
            {
                new Unit { Symbol = "K", Name = "kelvin", Category = "temperature", IsBaseUnit = true, ConversionFactor = 1.0 },
                new Unit { Symbol = "°C", Name = "celsius", Category = "temperature", IsBaseUnit = false, ConversionFormula = "x + 273.15", ConversionInverseFormula = "x - 273.15" }
            }
        };

        _mockRepository.Setup(r => r.GetCategoryByNameAsync("temperature"))
            .ReturnsAsync(category);

        // Act - Convert from K to °C: K - 273.15 = °C, so 173.15K = -100°C
        var result = await _service.ConvertAsync("temperature", "K", "°C", 173.15);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(-100.0, result.Result, 4);
        Assert.Contains("-100", result.FormattedResult);
    }

    [Fact]
    public async Task ConvertAsync_WhenResultIsVeryLarge_FormatsCorrectly()
    {
        // Arrange
        var category = new Category
        {
            Name = "length",
            DisplayName = "Length",
            Group = "Common",
            BaseUnit = new Unit { Symbol = "m", Name = "meter", Category = "length", IsBaseUnit = true },
            Units = new List<Unit>
            {
                new Unit { Symbol = "m", Name = "meter", Category = "length", IsBaseUnit = true, ConversionFactor = 1.0 },
                new Unit { Symbol = "km", Name = "kilometer", Category = "length", IsBaseUnit = false, ConversionFactor = 1000.0 }
            }
        };

        _mockRepository.Setup(r => r.GetCategoryByNameAsync("length"))
            .ReturnsAsync(category);

        // Act
        var result = await _service.ConvertAsync("length", "m", "km", 1000000.0);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(1000.0, result.Result, 4);
        Assert.Contains("1000", result.FormattedResult);
    }

    [Fact]
    public async Task ConvertAsync_WhenResultIsVerySmall_FormatsCorrectly()
    {
        // Arrange
        var category = new Category
        {
            Name = "length",
            DisplayName = "Length",
            Group = "Common",
            BaseUnit = new Unit { Symbol = "m", Name = "meter", Category = "length", IsBaseUnit = true },
            Units = new List<Unit>
            {
                new Unit { Symbol = "m", Name = "meter", Category = "length", IsBaseUnit = true, ConversionFactor = 1.0 },
                new Unit { Symbol = "mm", Name = "millimeter", Category = "length", IsBaseUnit = false, ConversionFactor = 0.001 }
            }
        };

        _mockRepository.Setup(r => r.GetCategoryByNameAsync("length"))
            .ReturnsAsync(category);

        // Act
        var result = await _service.ConvertAsync("length", "m", "mm", 0.001);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(1.0, result.Result, 4);
        Assert.Contains("1", result.FormattedResult);
    }

    [Fact]
    public async Task ConvertAsync_WhenResultIsZero_FormatsCorrectly()
    {
        // Arrange
        var category = new Category
        {
            Name = "length",
            DisplayName = "Length",
            Group = "Common",
            BaseUnit = new Unit { Symbol = "m", Name = "meter", Category = "length", IsBaseUnit = true },
            Units = new List<Unit>
            {
                new Unit { Symbol = "m", Name = "meter", Category = "length", IsBaseUnit = true, ConversionFactor = 1.0 },
                new Unit { Symbol = "km", Name = "kilometer", Category = "length", IsBaseUnit = false, ConversionFactor = 1000.0 }
            }
        };

        _mockRepository.Setup(r => r.GetCategoryByNameAsync("length"))
            .ReturnsAsync(category);

        // Act
        var result = await _service.ConvertAsync("length", "m", "km", 0.0);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(0.0, result.Result);
        Assert.Contains("0", result.FormattedResult);
    }

    [Fact]
    public async Task ConvertAsync_WhenValueIsZero_ReturnsZero()
    {
        // Arrange
        var category = new Category
        {
            Name = "length",
            DisplayName = "Length",
            Group = "Common",
            BaseUnit = new Unit { Symbol = "m", Name = "meter", Category = "length", IsBaseUnit = true },
            Units = new List<Unit>
            {
                new Unit { Symbol = "m", Name = "meter", Category = "length", IsBaseUnit = true, ConversionFactor = 1.0 },
                new Unit { Symbol = "km", Name = "kilometer", Category = "length", IsBaseUnit = false, ConversionFactor = 1000.0 }
            }
        };

        _mockRepository.Setup(r => r.GetCategoryByNameAsync("length"))
            .ReturnsAsync(category);

        // Act
        var result = await _service.ConvertAsync("length", "m", "km", 0.0);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(0.0, result.Result);
        Assert.Equal(0.0, result.OriginalValue);
    }

    [Fact]
    public async Task ConvertAsync_WhenValueIsNegative_HandlesCorrectly()
    {
        // Arrange
        var category = new Category
        {
            Name = "temperature",
            DisplayName = "Temperature",
            Group = "Common",
            BaseUnit = new Unit { Symbol = "K", Name = "kelvin", Category = "temperature", IsBaseUnit = true },
            Units = new List<Unit>
            {
                new Unit { Symbol = "K", Name = "kelvin", Category = "temperature", IsBaseUnit = true, ConversionFactor = 1.0 },
                new Unit { Symbol = "°C", Name = "celsius", Category = "temperature", IsBaseUnit = false, ConversionFormula = "x + 273.15", ConversionInverseFormula = "x - 273.15" }
            }
        };

        _mockRepository.Setup(r => r.GetCategoryByNameAsync("temperature"))
            .ReturnsAsync(category);

        // Act - Convert from K to °C: K - 273.15 = °C
        var result = await _service.ConvertAsync("temperature", "K", "°C", 173.15);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(-100.0, result.Result, 4);
        Assert.Equal(173.15, result.OriginalValue);
    }
}

