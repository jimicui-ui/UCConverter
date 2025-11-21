namespace UCConverter.Domain.Exceptions;

/// <summary>
/// Exception thrown when a category is not found
/// </summary>
public class CategoryNotFoundException : UnitConversionException
{
    public string CategoryName { get; }

    public CategoryNotFoundException(string categoryName) 
        : base($"Category '{categoryName}' was not found.")
    {
        CategoryName = categoryName;
    }
}

