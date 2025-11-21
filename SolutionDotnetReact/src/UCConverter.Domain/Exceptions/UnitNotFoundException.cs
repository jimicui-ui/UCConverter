namespace UCConverter.Domain.Exceptions;

/// <summary>
/// Exception thrown when a unit is not found
/// </summary>
public class UnitNotFoundException : UnitConversionException
{
    public string UnitSymbol { get; }

    public UnitNotFoundException(string unitSymbol) 
        : base($"Unit with symbol '{unitSymbol}' was not found.")
    {
        UnitSymbol = unitSymbol;
    }
}

