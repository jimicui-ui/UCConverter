namespace UCConverter.Domain.Tests.Entities;

using UCConverter.Domain.Entities;
using Xunit;

/// <summary>
/// Tests for Unit.IsValidUnitSymbol static method
/// </summary>
public class UnitIsValidUnitSymbolTests
{
    [Fact]
    public void IsValidUnitSymbol_WhenSymbolIsNull_ReturnsFalse()
    {
        // Act
        var result = Unit.IsValidUnitSymbol(null!);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void IsValidUnitSymbol_WhenSymbolIsEmpty_ReturnsFalse()
    {
        // Act
        var result = Unit.IsValidUnitSymbol("");

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void IsValidUnitSymbol_WhenSymbolIsWhitespaceOnly_ReturnsTrue()
    {
        // Act - Whitespace is allowed in the pattern, but IsNullOrWhiteSpace check happens first
        var result = Unit.IsValidUnitSymbol("   ");

        // Assert - The method checks IsNullOrWhiteSpace first, which returns false for whitespace
        // But the regex pattern allows spaces, so this depends on the implementation
        // Actually, IsNullOrWhiteSpace returns true for whitespace, so this should return false
        Assert.False(result);
    }

    [Fact]
    public void IsValidUnitSymbol_WhenSymbolIsValid_ReturnsTrue()
    {
        // Act
        var result = Unit.IsValidUnitSymbol("m");

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void IsValidUnitSymbol_WhenSymbolHasNumbers_ReturnsTrue()
    {
        // Act
        var result = Unit.IsValidUnitSymbol("m2");

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void IsValidUnitSymbol_WhenSymbolHasSuperscript_ReturnsTrue()
    {
        // Act
        var result = Unit.IsValidUnitSymbol("m²");

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void IsValidUnitSymbol_WhenSymbolHasCubic_ReturnsTrue()
    {
        // Act
        var result = Unit.IsValidUnitSymbol("m³");

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void IsValidUnitSymbol_WhenSymbolHasMiddleDot_ReturnsTrue()
    {
        // Act
        var result = Unit.IsValidUnitSymbol("N·m");

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void IsValidUnitSymbol_WhenSymbolHasOmega_ReturnsTrue()
    {
        // Act
        var result = Unit.IsValidUnitSymbol("Ω");

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void IsValidUnitSymbol_WhenSymbolHasMicro_ReturnsTrue()
    {
        // Act
        var result = Unit.IsValidUnitSymbol("µ");

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void IsValidUnitSymbol_WhenSymbolHasDegree_ReturnsTrue()
    {
        // Act
        var result = Unit.IsValidUnitSymbol("°C");

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void IsValidUnitSymbol_WhenSymbolHasSlash_ReturnsTrue()
    {
        // Act
        var result = Unit.IsValidUnitSymbol("m/s");

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void IsValidUnitSymbol_WhenSymbolHasParentheses_ReturnsTrue()
    {
        // Act
        var result = Unit.IsValidUnitSymbol("W/(m·K)");

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void IsValidUnitSymbol_WhenSymbolHasMultipleSuperscripts_ReturnsTrue()
    {
        // Act
        var result = Unit.IsValidUnitSymbol("m²·s⁻²");

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void IsValidUnitSymbol_WhenSymbolHasSpaces_ReturnsTrue()
    {
        // Act
        var result = Unit.IsValidUnitSymbol("kg m");

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void IsValidUnitSymbol_WhenSymbolHasMultiplicationSign_ReturnsTrue()
    {
        // Act
        var result = Unit.IsValidUnitSymbol("N×m");

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void IsValidUnitSymbol_WhenSymbolHasAsterisk_ReturnsTrue()
    {
        // Act
        var result = Unit.IsValidUnitSymbol("N*m");

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void IsValidUnitSymbol_WhenSymbolHasPlus_ReturnsTrue()
    {
        // Act
        var result = Unit.IsValidUnitSymbol("m+");

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void IsValidUnitSymbol_WhenSymbolHasMinus_ReturnsTrue()
    {
        // Act
        var result = Unit.IsValidUnitSymbol("m-");

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void IsValidUnitSymbol_WhenSymbolHasInvalidCharacter_ReturnsFalse()
    {
        // Act
        var result = Unit.IsValidUnitSymbol("m@");

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void IsValidUnitSymbol_WhenSymbolHasSpecialInvalidChar_ReturnsFalse()
    {
        // Act
        var result = Unit.IsValidUnitSymbol("m#");

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void IsValidUnitSymbol_WhenSymbolIsComplexValid_ReturnsTrue()
    {
        // Act
        var result = Unit.IsValidUnitSymbol("kg·m²/s²");

        // Assert
        Assert.True(result);
    }
}

