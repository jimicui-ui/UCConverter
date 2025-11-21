namespace UCConverter.Domain.Tests.Exceptions;

using UCConverter.Domain.Exceptions;
using Xunit;

public class UnitConversionExceptionTests
{
    [Fact]
    public void UnitConversionException_WithMessage_CreatesException()
    {
        // Act
        var exception = new UnitConversionException("Test message");

        // Assert
        Assert.Equal("Test message", exception.Message);
    }

    [Fact]
    public void UnitConversionException_WithMessageAndInnerException_CreatesException()
    {
        // Arrange
        var innerException = new Exception("Inner exception");

        // Act
        var exception = new UnitConversionException("Test message", innerException);

        // Assert
        Assert.Equal("Test message", exception.Message);
        Assert.Equal(innerException, exception.InnerException);
    }

    [Fact]
    public void UnitNotFoundException_CreatesExceptionWithUnitSymbol()
    {
        // Act
        var exception = new UnitNotFoundException("kg");

        // Assert
        Assert.Equal("kg", exception.UnitSymbol);
        Assert.Contains("kg", exception.Message);
    }

    [Fact]
    public void CategoryNotFoundException_CreatesExceptionWithCategoryName()
    {
        // Act
        var exception = new CategoryNotFoundException("length");

        // Assert
        Assert.Equal("length", exception.CategoryName);
        Assert.Contains("length", exception.Message);
    }

    [Fact]
    public void InvalidConversionException_CreatesExceptionWithUnits()
    {
        // Act
        var exception = new InvalidConversionException("m", "kg", "length");

        // Assert
        Assert.Equal("m", exception.FromUnit);
        Assert.Equal("kg", exception.ToUnit);
        Assert.Equal("length", exception.Category);
        Assert.Contains("m", exception.Message);
        Assert.Contains("kg", exception.Message);
    }

    [Fact]
    public void InvalidConversionException_WithoutCategory_CreatesException()
    {
        // Act
        var exception = new InvalidConversionException("m", "kg");

        // Assert
        Assert.Equal("m", exception.FromUnit);
        Assert.Equal("kg", exception.ToUnit);
        Assert.Null(exception.Category);
    }
}

