namespace UCConverter.Domain.Tests.Exceptions;

using UCConverter.Domain.Exceptions;
using Xunit;

public class InvalidConversionExceptionTests
{
    [Fact]
    public void Constructor_WhenCategoryProvided_SetsProperties()
    {
        // Act
        var exception = new InvalidConversionException("m", "kg", "length");

        // Assert
        Assert.Equal("m", exception.FromUnit);
        Assert.Equal("kg", exception.ToUnit);
        Assert.Equal("length", exception.Category);
        Assert.Contains("m", exception.Message);
        Assert.Contains("kg", exception.Message);
        Assert.Contains("length", exception.Message);
    }

    [Fact]
    public void Constructor_WhenCategoryIsNull_MessageDoesNotIncludeCategory()
    {
        // Act
        var exception = new InvalidConversionException("m", "kg", null);

        // Assert
        Assert.Equal("m", exception.FromUnit);
        Assert.Equal("kg", exception.ToUnit);
        Assert.Null(exception.Category);
        Assert.Contains("m", exception.Message);
        Assert.Contains("kg", exception.Message);
    }
}

