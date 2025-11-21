namespace UCConverter.Domain.Exceptions;

/// <summary>
/// Exception thrown when unit conversion fails
/// </summary>
public class UnitConversionException : Exception
{
    public UnitConversionException(string message) : base(message)
    {
    }

    public UnitConversionException(string message, Exception innerException) : base(message, innerException)
    {
    }
}

