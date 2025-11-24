namespace UCConverter.Domain.Tests.Services;

using Moq;
using UCConverter.Domain.Entities;
using UCConverter.Domain.Exceptions;
using UCConverter.Domain.Interfaces;
using UCConverter.Domain.Services;
using Xunit;

public class ConversionServiceErrorPathTests
{
    private readonly Mock<IUnitRepository> _mockRepository;
    private readonly ConversionService _service;

    public ConversionServiceErrorPathTests()
    {
        _mockRepository = new Mock<IUnitRepository>();
        _service = new ConversionService(_mockRepository.Object);
    }

    [Fact]
    public async Task ConvertAsync_WhenCategoryNameIsNull_ThrowsArgumentException()
    {
        // Act & Assert
        await Assert.ThrowsAsync<ArgumentException>(() =>
            _service.ConvertAsync(null!, "m", "km", 1000.0));
    }

    [Fact]
    public async Task ConvertAsync_WhenCategoryNameIsEmpty_ThrowsArgumentException()
    {
        // Act & Assert
        await Assert.ThrowsAsync<ArgumentException>(() =>
            _service.ConvertAsync("", "m", "km", 1000.0));
    }

    [Fact]
    public async Task ConvertAsync_WhenCategoryNameIsWhitespace_ThrowsArgumentException()
    {
        // Act & Assert
        await Assert.ThrowsAsync<ArgumentException>(() =>
            _service.ConvertAsync("   ", "m", "km", 1000.0));
    }

    [Fact]
    public async Task ConvertAsync_WhenFromUnitSymbolIsNull_ThrowsArgumentException()
    {
        // Act & Assert
        await Assert.ThrowsAsync<ArgumentException>(() =>
            _service.ConvertAsync("length", null!, "km", 1000.0));
    }

    [Fact]
    public async Task ConvertAsync_WhenFromUnitSymbolIsEmpty_ThrowsArgumentException()
    {
        // Act & Assert
        await Assert.ThrowsAsync<ArgumentException>(() =>
            _service.ConvertAsync("length", "", "km", 1000.0));
    }

    [Fact]
    public async Task ConvertAsync_WhenFromUnitSymbolIsWhitespace_ThrowsArgumentException()
    {
        // Act & Assert
        await Assert.ThrowsAsync<ArgumentException>(() =>
            _service.ConvertAsync("length", "   ", "km", 1000.0));
    }

    [Fact]
    public async Task ConvertAsync_WhenToUnitSymbolIsNull_ThrowsArgumentException()
    {
        // Act & Assert
        await Assert.ThrowsAsync<ArgumentException>(() =>
            _service.ConvertAsync("length", "m", null!, 1000.0));
    }

    [Fact]
    public async Task ConvertAsync_WhenToUnitSymbolIsEmpty_ThrowsArgumentException()
    {
        // Act & Assert
        await Assert.ThrowsAsync<ArgumentException>(() =>
            _service.ConvertAsync("length", "m", "", 1000.0));
    }

    [Fact]
    public async Task ConvertAsync_WhenToUnitSymbolIsWhitespace_ThrowsArgumentException()
    {
        // Act & Assert
        await Assert.ThrowsAsync<ArgumentException>(() =>
            _service.ConvertAsync("length", "m", "   ", 1000.0));
    }

    [Fact]
    public async Task ConvertAsync_WhenFormulaEvaluationFails_ThrowsUnitConversionException()
    {
        // Arrange
        var baseUnit = new Unit { Symbol = "base", Name = "base", IsBaseUnit = true, Category = "test" };
        var category = new Category
        {
            Name = "test",
            DisplayName = "Test",
            BaseUnit = baseUnit
        };

        var fromUnit = new Unit
        {
            Symbol = "from",
            Name = "from",
            Category = "test",
            ConversionFormula = "invalid formula syntax !!!"
        };

        category.Units.Add(baseUnit);
        category.Units.Add(fromUnit);

        _mockRepository.Setup(r => r.GetCategoryByNameAsync("test"))
            .ReturnsAsync(category);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<UnitConversionException>(() =>
            _service.ConvertAsync("test", "from", "base", 100.0));
        Assert.Contains("Failed to evaluate conversion formula", exception.Message);
    }

    [Fact]
    public async Task ConvertAsync_WhenComplexInverseFormula_ThrowsUnitConversionException()
    {
        // Arrange
        var category = new Category
        {
            Name = "test",
            DisplayName = "Test",
            BaseUnit = new Unit { Symbol = "base", Name = "base", IsBaseUnit = true }
        };

        var toUnit = new Unit
        {
            Symbol = "to",
            Name = "to",
            Category = "test",
            ConversionFormula = "x * 2 + 5" // Complex formula not supported
        };

        var fromUnit = new Unit
        {
            Symbol = "from",
            Name = "from",
            Category = "test",
            ConversionFactor = 1.0
        };

        category.Units.Add(fromUnit);
        category.Units.Add(toUnit);

        _mockRepository.Setup(r => r.GetCategoryByNameAsync("test"))
            .ReturnsAsync(category);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<UnitConversionException>(() =>
            _service.ConvertAsync("test", "from", "to", 100.0));
        Assert.Contains("Complex inverse formula conversion not yet supported", exception.Message);
    }
}

