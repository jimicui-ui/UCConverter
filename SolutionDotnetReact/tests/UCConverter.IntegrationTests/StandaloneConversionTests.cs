namespace UCConverter.IntegrationTests;

using Microsoft.Extensions.Logging;
using UCConverter.Domain.Services;
using UCConverter.Infrastructure.Repositories;
using Xunit;

/// <summary>
/// Standalone integration tests for unit conversions that don't require the API to be running.
/// These tests use the repository and conversion service directly.
/// </summary>
public class StandaloneConversionTests : IDisposable
{
    private readonly JsonUnitRepository _repository;
    private readonly ConversionService _conversionService;
    private readonly string _unitsSettingsPath;

    public StandaloneConversionTests()
    {
        // Get the UnitsSettings path (same logic as Program.cs)
        var solutionRoot = Path.GetFullPath(Path.Combine(
            AppContext.BaseDirectory,
            "..", "..", "..", "..", ".."));
        _unitsSettingsPath = Path.Combine(solutionRoot, "UnitsSettings");

        // If running from bin/Debug, adjust path
        if (!Directory.Exists(_unitsSettingsPath))
        {
            _unitsSettingsPath = Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "..", "UnitsSettings");
        }

        // If still not found, use a relative path from the solution
        if (!Directory.Exists(_unitsSettingsPath))
        {
            _unitsSettingsPath = Path.Combine(Directory.GetCurrentDirectory(), "..", "..", "..", "UnitsSettings");
        }

        // Initialize repository with actual JSON files
        var loggerFactory = LoggerFactory.Create(builder => builder.AddConsole().SetMinimumLevel(LogLevel.Warning));
        var logger = loggerFactory.CreateLogger<JsonUnitRepository>();
        _repository = new JsonUnitRepository(_unitsSettingsPath, logger);
        _repository.Initialize();

        _conversionService = new ConversionService(_repository);
    }

    [Fact]
    public async Task AllCategories_ShouldLoadSuccessfully()
    {
        // Act
        var categories = await _repository.GetAllCategoriesAsync();

        // Assert
        Assert.NotNull(categories);
        var categoriesList = categories.ToList();
        Assert.NotEmpty(categoriesList);
        Assert.True(categoriesList.Count >= 43, $"Expected at least 43 categories, but found {categoriesList.Count}");
        
        Console.WriteLine($"✓ Loaded {categoriesList.Count} categories successfully");
    }

    [Fact]
    public async Task Convert_AllCategories_AllBaseToOtherUnits_ShouldSucceed()
    {
        // Arrange
        var categories = await _repository.GetAllCategoriesAsync();
        var failures = new List<string>();
        var successCount = 0;
        var totalTests = 0;

        // Act & Assert
        foreach (var category in categories)
        {
            var baseUnit = category.Units.FirstOrDefault(u => u.IsBaseUnit);
            if (baseUnit == null)
            {
                failures.Add($"{category.Name}: No base unit found");
                continue;
            }

            foreach (var unit in category.Units)
            {
                if (unit.Symbol == baseUnit.Symbol)
                    continue;

                totalTests++;
                try
                {
                    var result = await _conversionService.ConvertAsync(
                        category.Name,
                        baseUnit.Symbol,
                        unit.Symbol,
                        1.0);

                    Assert.NotNull(result);
                    Assert.True(result.Result >= 0 || category.Name == "temperature", 
                        $"{category.Name}: {baseUnit.Symbol} -> {unit.Symbol} returned negative result");
                    successCount++;
                }
                catch (Exception ex)
                {
                    failures.Add($"{category.Name}: {baseUnit.Symbol} -> {unit.Symbol} failed: {ex.Message}");
                }
            }
        }

        // Report results
        Console.WriteLine($"\n✓ Base -> Other Units Test Results:");
        Console.WriteLine($"  Total Tests: {totalTests}");
        Console.WriteLine($"  Successful: {successCount}");
        Console.WriteLine($"  Failed: {failures.Count}");

        if (failures.Any())
        {
            Console.WriteLine($"\n✗ Failures:\n{string.Join("\n", failures.Take(20))}");
            if (failures.Count > 20)
            {
                Console.WriteLine($"  ... and {failures.Count - 20} more failures");
            }
            Assert.Fail($"Conversion failures: {failures.Count} out of {totalTests}");
        }
    }

    [Fact]
    public async Task Convert_AllCategories_AllOtherToBaseUnits_ShouldSucceed()
    {
        // Arrange
        var categories = await _repository.GetAllCategoriesAsync();
        var failures = new List<string>();
        var successCount = 0;
        var totalTests = 0;

        // Act & Assert
        foreach (var category in categories)
        {
            var baseUnit = category.Units.FirstOrDefault(u => u.IsBaseUnit);
            if (baseUnit == null)
            {
                failures.Add($"{category.Name}: No base unit found");
                continue;
            }

            foreach (var unit in category.Units)
            {
                if (unit.Symbol == baseUnit.Symbol)
                    continue;

                totalTests++;
                try
                {
                    var result = await _conversionService.ConvertAsync(
                        category.Name,
                        unit.Symbol,
                        baseUnit.Symbol,
                        1.0);

                    Assert.NotNull(result);
                    Assert.True(result.Result >= 0 || category.Name == "temperature", 
                        $"{category.Name}: {unit.Symbol} -> {baseUnit.Symbol} returned negative result");
                    successCount++;
                }
                catch (Exception ex)
                {
                    failures.Add($"{category.Name}: {unit.Symbol} -> {baseUnit.Symbol} failed: {ex.Message}");
                }
            }
        }

        // Report results
        Console.WriteLine($"\n✓ Other -> Base Units Test Results:");
        Console.WriteLine($"  Total Tests: {totalTests}");
        Console.WriteLine($"  Successful: {successCount}");
        Console.WriteLine($"  Failed: {failures.Count}");

        if (failures.Any())
        {
            Console.WriteLine($"\n✗ Failures:\n{string.Join("\n", failures.Take(20))}");
            if (failures.Count > 20)
            {
                Console.WriteLine($"  ... and {failures.Count - 20} more failures");
            }
            Assert.Fail($"Conversion failures: {failures.Count} out of {totalTests}");
        }
    }

    [Fact]
    public async Task Convert_AllCategories_AllCrossConversions_ShouldSucceed()
    {
        // Arrange
        var categories = await _repository.GetAllCategoriesAsync();
        var failures = new List<string>();
        var successCount = 0;
        var totalTests = 0;

        // Act & Assert - Test ALL possible conversion pairs (N × N - 1 for each category)
        foreach (var category in categories)
        {
            var units = category.Units.ToList();
            
            // Test all pairs (excluding same-to-same)
            for (int i = 0; i < units.Count; i++)
            {
                for (int j = 0; j < units.Count; j++)
                {
                    if (i == j)
                        continue;

                    totalTests++;
                    try
                    {
                        var result = await _conversionService.ConvertAsync(
                            category.Name,
                            units[i].Symbol,
                            units[j].Symbol,
                            1.0);

                        Assert.NotNull(result);
                        Assert.True(result.Result >= 0 || category.Name == "temperature", 
                            $"{category.Name}: {units[i].Symbol} -> {units[j].Symbol} returned negative result");
                        successCount++;
                    }
                    catch (Exception ex)
                    {
                        failures.Add($"{category.Name}: {units[i].Symbol} -> {units[j].Symbol} failed: {ex.Message}");
                    }
                }
            }
        }

        // Report results
        Console.WriteLine($"\n✓ All Cross-Conversions Test Results:");
        Console.WriteLine($"  Total Tests: {totalTests}");
        Console.WriteLine($"  Successful: {successCount}");
        Console.WriteLine($"  Failed: {failures.Count}");
        Console.WriteLine($"  Success Rate: {(successCount * 100.0 / totalTests):F2}%");

        if (failures.Any())
        {
            Console.WriteLine($"\n✗ Failures:\n{string.Join("\n", failures.Take(20))}");
            if (failures.Count > 20)
            {
                Console.WriteLine($"  ... and {failures.Count - 20} more failures");
            }
            Assert.Fail($"Conversion failures: {failures.Count} out of {totalTests}");
        }
    }

    [Fact]
    public async Task Convert_AllCategories_RoundTripConversions_ShouldReturnOriginalValue()
    {
        // Arrange
        var categories = await _repository.GetAllCategoriesAsync();
        var failures = new List<string>();
        const double testValue = 100.0;
        const double tolerance = 0.0001; // Allow small rounding differences
        var successCount = 0;
        var totalTests = 0;

        // Act & Assert - Test round-trip for ALL pairs
        foreach (var category in categories)
        {
            var units = category.Units.ToList();
            
            // Test round-trip for each pair (ALL units, not limited)
            for (int i = 0; i < units.Count; i++)
            {
                for (int j = 0; j < units.Count; j++)
                {
                    if (i == j)
                        continue;

                    totalTests++;
                    try
                    {
                        // Convert A -> B
                        var forward = await _conversionService.ConvertAsync(
                            category.Name,
                            units[i].Symbol,
                            units[j].Symbol,
                            testValue);

                        // Convert B -> A
                        var backward = await _conversionService.ConvertAsync(
                            category.Name,
                            units[j].Symbol,
                            units[i].Symbol,
                            forward.Result);

                        // Should return original value (within tolerance)
                        var difference = Math.Abs(backward.Result - testValue);
                        if (difference > tolerance)
                        {
                            failures.Add($"{category.Name}: {units[i].Symbol} -> {units[j].Symbol} -> {units[i].Symbol} " +
                                       $"Round-trip error: Expected {testValue}, got {backward.Result}, difference: {difference:F6}");
                        }
                        else
                        {
                            successCount++;
                        }
                    }
                    catch (Exception ex)
                    {
                        failures.Add($"{category.Name}: {units[i].Symbol} <-> {units[j].Symbol} Round-trip failed: {ex.Message}");
                    }
                }
            }
        }

        // Report results
        Console.WriteLine($"\n✓ Round-Trip Test Results:");
        Console.WriteLine($"  Total Tests: {totalTests}");
        Console.WriteLine($"  Successful: {successCount}");
        Console.WriteLine($"  Failed: {failures.Count}");

        if (failures.Any())
        {
            Console.WriteLine($"\n✗ Failures:\n{string.Join("\n", failures.Take(20))}");
            if (failures.Count > 20)
            {
                Console.WriteLine($"  ... and {failures.Count - 20} more failures");
            }
            Assert.Fail($"Round-trip conversion failures: {failures.Count} out of {totalTests}");
        }
    }

    [Theory]
    [InlineData("length", "m", "km", 1000.0, 1.0)]
    [InlineData("length", "km", "m", 1.0, 1000.0)]
    [InlineData("weight", "kg", "g", 1.0, 1000.0)]
    [InlineData("weight", "g", "kg", 1000.0, 1.0)]
    [InlineData("volume", "m³", "L", 1.0, 1000.0)]
    [InlineData("volume", "L", "m³", 1000.0, 1.0)]
    [InlineData("temperature", "K", "°C", 273.15, 0.0)]
    [InlineData("temperature", "°C", "K", 0.0, 273.15)]
    [InlineData("temperature", "K", "°F", 273.15, 32.0)]
    [InlineData("temperature", "°F", "K", 32.0, 273.15)]
    [InlineData("pressure", "Pa", "kPa", 1000.0, 1.0)]
    [InlineData("pressure", "kPa", "Pa", 1.0, 1000.0)]
    [InlineData("energy", "J", "kJ", 1000.0, 1.0)]
    [InlineData("energy", "kJ", "J", 1.0, 1000.0)]
    [InlineData("power", "W", "kW", 1000.0, 1.0)]
    [InlineData("power", "kW", "W", 1.0, 1000.0)]
    [InlineData("current", "A", "mA", 1.0, 1000.0)]
    [InlineData("current", "mA", "A", 1000.0, 1.0)]
    public async Task Convert_SampleConversions_ShouldReturnCorrectResult(
        string category,
        string fromUnit,
        string toUnit,
        double inputValue,
        double expectedResult)
    {
        // Act
        var result = await _conversionService.ConvertAsync(category, fromUnit, toUnit, inputValue);

        // Assert
        Assert.NotNull(result);
        var precision = category == "temperature" ? 2 : 4;
        Assert.Equal(expectedResult, result.Result, precision);
        Assert.Equal(fromUnit, result.FromUnit.Symbol);
        Assert.Equal(toUnit, result.ToUnit.Symbol);
        
        Console.WriteLine($"✓ {category}: {inputValue} {fromUnit} = {result.Result} {toUnit} (expected: {expectedResult})");
    }

    public void Dispose()
    {
        // Cleanup if needed
    }
}

