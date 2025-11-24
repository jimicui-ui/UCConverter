namespace UCConverter.Domain.Tests.Exceptions;

using UCConverter.Domain.Exceptions;
using Xunit;

public class ExceptionConstructorTests
{
    [Fact]
    public void UnitConversionException_WithMessageOnly_CreatesException()
    {
        // Act
        var exception = new UnitConversionException("Test message");

        // Assert
        Assert.Equal("Test message", exception.Message);
        Assert.Null(exception.InnerException);
    }

    [Fact]
    public void UnitConversionException_WithMessageAndInnerException_CreatesException()
    {
        // Arrange
        var innerException = new InvalidOperationException("Inner error");

        // Act
        var exception = new UnitConversionException("Outer message", innerException);

        // Assert
        Assert.Equal("Outer message", exception.Message);
        Assert.Equal(innerException, exception.InnerException);
    }

    [Fact]
    public void CategoryNotFoundException_WithCategoryName_CreatesException()
    {
        // Act
        var exception = new CategoryNotFoundException("testCategory");

        // Assert
        Assert.Equal("testCategory", exception.CategoryName);
        Assert.Contains("testCategory", exception.Message);
    }

    [Fact]
    public void UnitNotFoundException_WithUnitSymbol_CreatesException()
    {
        // Act
        var exception = new UnitNotFoundException("testUnit");

        // Assert
        Assert.Equal("testUnit", exception.UnitSymbol);
        Assert.Contains("testUnit", exception.Message);
    }

    [Fact]
    public void InvalidConversionException_WithAllParameters_CreatesException()
    {
        // Act
        var exception = new InvalidConversionException("fromUnit", "toUnit", "category");

        // Assert
        Assert.Equal("fromUnit", exception.FromUnit);
        Assert.Equal("toUnit", exception.ToUnit);
        Assert.Equal("category", exception.Category);
        Assert.Contains("fromUnit", exception.Message);
        Assert.Contains("toUnit", exception.Message);
        Assert.Contains("category", exception.Message);
    }

    [Fact]
    public void InvalidConversionException_WithNullCategory_CreatesException()
    {
        // Act
        var exception = new InvalidConversionException("fromUnit", "toUnit", null);

        // Assert
        Assert.Equal("fromUnit", exception.FromUnit);
        Assert.Equal("toUnit", exception.ToUnit);
        Assert.Null(exception.Category);
        Assert.Contains("fromUnit", exception.Message);
        Assert.Contains("toUnit", exception.Message);
    }
}

