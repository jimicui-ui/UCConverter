namespace UCConverter.IntegrationTests;

using System.Net.Http.Json;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Logging;
using UCConverter.Api;
using UCConverter.Application.DTOs;
using UCConverter.Domain.Services;
using UCConverter.Infrastructure.Repositories;
using Xunit;

/// <summary>
/// Comprehensive integration tests for all unit conversions across all categories.
/// These tests load all categories from JSON files and test all possible conversion pairs.
/// </summary>
public class ComprehensiveUnitConversionTests : IClassFixture<WebApplicationFactory<Program>>, IDisposable
{
    private readonly WebApplicationFactory<Program> _factory;
    private readonly HttpClient _client;
    private readonly JsonUnitRepository _repository;
    private readonly ConversionService _conversionService;
    private readonly JsonSerializerOptions _jsonOptions;
    private readonly string _unitsSettingsPath;

    public ComprehensiveUnitConversionTests(WebApplicationFactory<Program> factory)
    {
        _factory = factory;
        _client = _factory.CreateClient();
        _jsonOptions = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };

        // Get the UnitsSettings path (same logic as Program.cs)
        var solutionRoot = Path.GetFullPath(Path.Combine(
            AppContext.BaseDirectory,
            "..", "..", "..", "..", ".."));
        _unitsSettingsPath = Path.Combine(solutionRoot, "UnitsSettings");

        // If running from bin/Debug, adjust path
        if (!Directory.Exists(_unitsSettingsPath))
        {
            // Try alternative path
            _unitsSettingsPath = Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "..", "UnitsSettings");
        }

        // If still not found, use a relative path from the solution
        if (!Directory.Exists(_unitsSettingsPath))
        {
            _unitsSettingsPath = Path.Combine(Directory.GetCurrentDirectory(), "..", "..", "..", "UnitsSettings");
        }

        // Initialize repository with actual JSON files
        var loggerFactory = LoggerFactory.Create(builder => builder.AddConsole());
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
    }

    [Theory]
    [InlineData("length", "m", "km", 1000.0, 1.0)]
    [InlineData("length", "km", "m", 1.0, 1000.0)]
    [InlineData("length", "m", "cm", 1.0, 100.0)]
    [InlineData("length", "cm", "m", 100.0, 1.0)]
    [InlineData("length", "m", "ft", 1.0, 3.2808)]
    [InlineData("length", "ft", "m", 1.0, 0.3048)]
    [InlineData("length", "m", "in", 1.0, 39.3701)]
    [InlineData("length", "in", "m", 1.0, 0.0254)]
    [InlineData("weight", "kg", "g", 1.0, 1000.0)]
    [InlineData("weight", "g", "kg", 1000.0, 1.0)]
    [InlineData("weight", "kg", "lb", 1.0, 2.2046)]
    [InlineData("weight", "lb", "kg", 1.0, 0.4536)]
    [InlineData("volume", "m³", "L", 1.0, 1000.0)]
    [InlineData("volume", "L", "m³", 1000.0, 1.0)]
    [InlineData("volume", "L", "mL", 1.0, 1000.0)]
    [InlineData("volume", "mL", "L", 1000.0, 1.0)]
    [InlineData("area", "m²", "km²", 1000000.0, 1.0)]
    [InlineData("area", "km²", "m²", 1.0, 1000000.0)]
    [InlineData("area", "m²", "cm²", 1.0, 10000.0)]
    [InlineData("area", "cm²", "m²", 10000.0, 1.0)]
    [InlineData("time", "s", "ms", 1.0, 1000.0)]
    [InlineData("time", "ms", "s", 1000.0, 1.0)]
    [InlineData("time", "s", "min", 60.0, 1.0)]
    [InlineData("time", "min", "s", 1.0, 60.0)]
    [InlineData("time", "h", "min", 1.0, 60.0)]
    [InlineData("time", "min", "h", 60.0, 1.0)]
    [InlineData("speed", "m/s", "km/h", 1.0, 3.6)]
    [InlineData("speed", "km/h", "m/s", 1.0, 0.2778)]
    [InlineData("speed", "m/s", "mph", 1.0, 2.2369)]
    [InlineData("speed", "mph", "m/s", 1.0, 0.4470)]
    [InlineData("pressure", "Pa", "kPa", 1000.0, 1.0)]
    [InlineData("pressure", "kPa", "Pa", 1.0, 1000.0)]
    [InlineData("pressure", "Pa", "bar", 100000.0, 1.0)]
    [InlineData("pressure", "bar", "Pa", 1.0, 100000.0)]
    [InlineData("pressure", "Pa", "psi", 6894.76, 1.0)]
    [InlineData("pressure", "psi", "Pa", 1.0, 6894.76)]
    [InlineData("energy", "J", "kJ", 1000.0, 1.0)]
    [InlineData("energy", "kJ", "J", 1.0, 1000.0)]
    [InlineData("energy", "J", "cal", 4.184, 1.0)]
    [InlineData("energy", "cal", "J", 1.0, 4.184)]
    [InlineData("energy", "J", "kWh", 3600000.0, 1.0)]
    [InlineData("energy", "kWh", "J", 1.0, 3600000.0)]
    [InlineData("power", "W", "kW", 1000.0, 1.0)]
    [InlineData("power", "kW", "W", 1.0, 1000.0)]
    [InlineData("power", "W", "hp", 745.7, 1.0)]
    [InlineData("power", "hp", "W", 1.0, 745.7)]
    [InlineData("current", "A", "mA", 1.0, 1000.0)]
    [InlineData("current", "mA", "A", 1000.0, 1.0)]
    [InlineData("current", "A", "kA", 1000.0, 1.0)]
    [InlineData("current", "kA", "A", 1.0, 1000.0)]
    [InlineData("electricPotential", "V", "mV", 1.0, 1000.0)]
    [InlineData("electricPotential", "mV", "V", 1000.0, 1.0)]
    [InlineData("electricPotential", "V", "kV", 1000.0, 1.0)]
    [InlineData("electricPotential", "kV", "V", 1.0, 1000.0)]
    [InlineData("electricResistance", "Ω", "mΩ", 1.0, 1000.0)]
    [InlineData("electricResistance", "mΩ", "Ω", 1000.0, 1.0)]
    [InlineData("electricResistance", "Ω", "kΩ", 1000.0, 1.0)]
    [InlineData("electricResistance", "kΩ", "Ω", 1.0, 1000.0)]
    [InlineData("capacitance", "F", "mF", 1.0, 1000.0)]
    [InlineData("capacitance", "mF", "F", 1000.0, 1.0)]
    [InlineData("capacitance", "F", "µF", 1.0, 1000000.0)]
    [InlineData("capacitance", "µF", "F", 1000000.0, 1.0)]
    [InlineData("inductance", "H", "mH", 1.0, 1000.0)]
    [InlineData("inductance", "mH", "H", 1000.0, 1.0)]
    [InlineData("inductance", "H", "µH", 1.0, 1000000.0)]
    [InlineData("inductance", "µH", "H", 1000000.0, 1.0)]
    [InlineData("acceleration", "m/s²", "ft/s²", 1.0, 3.2808)]
    [InlineData("acceleration", "ft/s²", "m/s²", 1.0, 0.3048)]
    [InlineData("acceleration", "m/s²", "g", 9.80665, 1.0)]
    [InlineData("acceleration", "g", "m/s²", 1.0, 9.80665)]
    [InlineData("density", "kg/m³", "g/cm³", 1000.0, 1.0)]
    [InlineData("density", "g/cm³", "kg/m³", 1.0, 1000.0)]
    [InlineData("density", "kg/m³", "lb/ft³", 16.0185, 1.0)]
    [InlineData("density", "lb/ft³", "kg/m³", 1.0, 16.0185)]
    [InlineData("torque", "N·m", "kN·m", 1000.0, 1.0)]
    [InlineData("torque", "kN·m", "N·m", 1.0, 1000.0)]
    [InlineData("torque", "N·m", "lbf·ft", 1.35582, 1.0)]
    [InlineData("torque", "lbf·ft", "N·m", 1.0, 1.35582)]
    public async Task Convert_LinearConversions_ShouldReturnCorrectResult(
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
        Assert.Equal(expectedResult, result.Result, 4); // 4 decimal places precision
        Assert.Equal(fromUnit, result.FromUnit.Symbol);
        Assert.Equal(toUnit, result.ToUnit.Symbol);
    }

    [Theory]
    [InlineData("temperature", "K", "°C", 273.15, 0.0)]
    [InlineData("temperature", "K", "°C", 373.15, 100.0)]
    [InlineData("temperature", "°C", "K", 0.0, 273.15)]
    [InlineData("temperature", "°C", "K", 100.0, 373.15)]
    [InlineData("temperature", "K", "°F", 273.15, 32.0)]
    [InlineData("temperature", "K", "°F", 373.15, 212.0)]
    [InlineData("temperature", "°F", "K", 32.0, 273.15)]
    [InlineData("temperature", "°F", "K", 212.0, 373.15)]
    [InlineData("temperature", "°C", "°F", 0.0, 32.0)]
    [InlineData("temperature", "°C", "°F", 100.0, 212.0)]
    [InlineData("temperature", "°C", "°F", -40.0, -40.0)]
    [InlineData("temperature", "°F", "°C", 32.0, 0.0)]
    [InlineData("temperature", "°F", "°C", 212.0, 100.0)]
    [InlineData("temperature", "°F", "°C", -40.0, -40.0)]
    public async Task Convert_TemperatureConversions_ShouldReturnCorrectResult(
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
        Assert.Equal(expectedResult, result.Result, 2); // 2 decimal places for temperature
        Assert.Equal(fromUnit, result.FromUnit.Symbol);
        Assert.Equal(toUnit, result.ToUnit.Symbol);
    }

    [Fact]
    public async Task Convert_AllCategories_AllBaseToOtherUnits_ShouldSucceed()
    {
        // Arrange
        var categories = await _repository.GetAllCategoriesAsync();
        var failures = new List<string>();

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
                }
                catch (Exception ex)
                {
                    failures.Add($"{category.Name}: {baseUnit.Symbol} -> {unit.Symbol} failed: {ex.Message}");
                }
            }
        }

        // Report failures
        if (failures.Any())
        {
            Assert.Fail($"Conversion failures:\n{string.Join("\n", failures)}");
        }
    }

    [Fact]
    public async Task Convert_AllCategories_AllOtherToBaseUnits_ShouldSucceed()
    {
        // Arrange
        var categories = await _repository.GetAllCategoriesAsync();
        var failures = new List<string>();

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
                }
                catch (Exception ex)
                {
                    failures.Add($"{category.Name}: {unit.Symbol} -> {baseUnit.Symbol} failed: {ex.Message}");
                }
            }
        }

        // Report failures
        if (failures.Any())
        {
            Assert.Fail($"Conversion failures:\n{string.Join("\n", failures)}");
        }
    }

    [Fact]
    public async Task Convert_AllCategories_AllCrossConversions_ShouldSucceed()
    {
        // Arrange
        var categories = await _repository.GetAllCategoriesAsync();
        var failures = new List<string>();
        var testCount = 0;
        var successCount = 0;

        // Act & Assert
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

                    testCount++;
                    try
                    {
                        var result = await _conversionService.ConvertAsync(
                            category.Name,
                            units[i].Symbol,
                            units[j].Symbol,
                            1.0);

                        Assert.NotNull(result);
                        successCount++;
                    }
                    catch (Exception ex)
                    {
                        failures.Add($"{category.Name}: {units[i].Symbol} -> {units[j].Symbol} failed: {ex.Message}");
                    }
                }
            }
        }

        // Report summary
        var failureCount = failures.Count;
        var successRate = testCount > 0 ? (successCount * 100.0 / testCount) : 0;

        if (failures.Any())
        {
            var failureReport = $"Conversion Test Summary:\n" +
                              $"Total Tests: {testCount}\n" +
                              $"Success: {successCount} ({successRate:F2}%)\n" +
                              $"Failures: {failureCount}\n\n" +
                              $"Failures:\n{string.Join("\n", failures.Take(50))}";
            
            if (failures.Count > 50)
            {
                failureReport += $"\n... and {failures.Count - 50} more failures";
            }

            Assert.Fail(failureReport);
        }

        // Log success
        Assert.True(successCount > 0, "No successful conversions");
    }

    [Fact]
    public async Task Convert_AllCategories_RoundTripConversions_ShouldReturnOriginalValue()
    {
        // Arrange
        var categories = await _repository.GetAllCategoriesAsync();
        var failures = new List<string>();
        const double testValue = 100.0;
        const double tolerance = 0.0001; // Allow small rounding differences

        // Act & Assert
        foreach (var category in categories)
        {
            var units = category.Units.ToList();
            
            // Test round-trip for each pair
            for (int i = 0; i < units.Count; i++)
            {
                for (int j = 0; j < units.Count; j++)
                {
                    if (i == j)
                        continue;

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
                                       $"Round-trip error: Expected {testValue}, got {backward.Result}, difference: {difference}");
                        }
                    }
                    catch (Exception ex)
                    {
                        failures.Add($"{category.Name}: {units[i].Symbol} <-> {units[j].Symbol} Round-trip failed: {ex.Message}");
                    }
                }
            }
        }

        // Report failures
        if (failures.Any())
        {
            var failureReport = $"Round-trip conversion failures ({failures.Count}):\n" +
                              $"{string.Join("\n", failures.Take(50))}";
            
            if (failures.Count > 50)
            {
                failureReport += $"\n... and {failures.Count - 50} more failures";
            }

            Assert.Fail(failureReport);
        }
    }

    [Fact]
    public async Task Convert_AllCategories_EdgeCaseValues_ShouldHandleCorrectly()
    {
        // Arrange
        var categories = await _repository.GetAllCategoriesAsync();
        var testValues = new[] { 0.0, 1.0, 0.001, 1000.0, 1000000.0 };
        var failures = new List<string>();

        // Act & Assert
        foreach (var category in categories)
        {
            var units = category.Units.ToList();
            if (units.Count < 2)
                continue;

            var fromUnit = units[0];
            var toUnit = units[1];

            foreach (var testValue in testValues)
            {
                // Skip negative values for non-temperature categories
                if (testValue < 0 && category.Name != "temperature")
                    continue;

                try
                {
                    var result = await _conversionService.ConvertAsync(
                        category.Name,
                        fromUnit.Symbol,
                        toUnit.Symbol,
                        testValue);

                    Assert.NotNull(result);
                    
                    // Zero should remain zero (except for temperature conversions which use formulas)
                    if (testValue == 0.0 && category.Name != "temperature")
                    {
                        Assert.Equal(0.0, result.Result, 4);
                    }
                    // For temperature, 0K = -273.15°C, so we skip the zero check
                }
                catch (Exception ex)
                {
                    failures.Add($"{category.Name}: {fromUnit.Symbol} -> {toUnit.Symbol} with value {testValue} failed: {ex.Message}");
                }
            }
        }

        // Report failures
        if (failures.Any())
        {
            Assert.Fail($"Edge case conversion failures:\n{string.Join("\n", failures)}");
        }
    }

    [Fact]
    public async Task Convert_AllCategories_ViaApi_ShouldReturnCorrectResults()
    {
        // Arrange
        var categories = await _repository.GetAllCategoriesAsync();
        var failures = new List<string>();
        var testCount = 0;
        var successCount = 0;

        // Act & Assert - Test a sample of conversions via API
        foreach (var category in categories)
        {
            var units = category.Units.Take(3).ToList(); // Test first 3 units to keep test time reasonable
            if (units.Count < 2)
                continue;

            for (int i = 0; i < units.Count; i++)
            {
                for (int j = 0; j < units.Count; j++)
                {
                    if (i == j)
                        continue;

                    testCount++;
                    try
                    {
                        var request = new ConvertRequestDto
                        {
                            Category = category.Name,
                            FromUnit = units[i].Symbol,
                            ToUnit = units[j].Symbol,
                            Value = 1.0
                        };

                        var response = await _client.PostAsJsonAsync("/api/convert", request);
                        
                        if (response.IsSuccessStatusCode)
                        {
                            var content = await response.Content.ReadAsStringAsync();
                            var result = JsonSerializer.Deserialize<ConvertResponseDto>(content, _jsonOptions);
                            
                            Assert.NotNull(result);
                            Assert.Equal(units[i].Symbol, result.FromUnit.Symbol);
                            Assert.Equal(units[j].Symbol, result.ToUnit.Symbol);
                            successCount++;
                        }
                        else
                        {
                            failures.Add($"{category.Name}: {units[i].Symbol} -> {units[j].Symbol} " +
                                       $"API returned {response.StatusCode}");
                        }
                    }
                    catch (Exception ex)
                    {
                        failures.Add($"{category.Name}: {units[i].Symbol} -> {units[j].Symbol} " +
                                   $"API call failed: {ex.Message}");
                    }
                }
            }
        }

        // Report summary
        if (failures.Any())
        {
            var failureReport = $"API Conversion Test Summary:\n" +
                              $"Total Tests: {testCount}\n" +
                              $"Success: {successCount}\n" +
                              $"Failures: {failures.Count}\n\n" +
                              $"Failures:\n{string.Join("\n", failures.Take(50))}";
            
            if (failures.Count > 50)
            {
                failureReport += $"\n... and {failures.Count - 50} more failures";
            }

            Assert.Fail(failureReport);
        }

        Assert.True(successCount > 0, "No successful API conversions");
    }

    [Fact]
    public async Task Convert_AllCategories_VerifyConversionFactors_ShouldBeCorrect()
    {
        // Arrange
        var categories = await _repository.GetAllCategoriesAsync();
        var failures = new List<string>();

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

                // Skip formula-based units (temperature)
                if (!string.IsNullOrEmpty(unit.ConversionFormula))
                    continue;

                if (!unit.ConversionFactor.HasValue)
                {
                    failures.Add($"{category.Name}: Unit {unit.Symbol} has no conversion factor");
                    continue;
                }

                try
                {
                    // Convert 1 unit to base
                    var toBase = await _conversionService.ConvertAsync(
                        category.Name,
                        unit.Symbol,
                        baseUnit.Symbol,
                        1.0);

                    // Convert 1 base to unit
                    var fromBase = await _conversionService.ConvertAsync(
                        category.Name,
                        baseUnit.Symbol,
                        unit.Symbol,
                        1.0);

                    // Verify: toBase * fromBase should equal 1 (within tolerance)
                    var product = toBase.Result * fromBase.Result;
                    if (Math.Abs(product - 1.0) > 0.0001)
                    {
                        failures.Add($"{category.Name}: {unit.Symbol} conversion factor inconsistency. " +
                                   $"toBase={toBase.Result}, fromBase={fromBase.Result}, product={product}");
                    }
                }
                catch (Exception ex)
                {
                    failures.Add($"{category.Name}: {unit.Symbol} verification failed: {ex.Message}");
                }
            }
        }

        // Report failures
        if (failures.Any())
        {
            Assert.Fail($"Conversion factor verification failures:\n{string.Join("\n", failures)}");
        }
    }

    public void Dispose()
    {
        _client?.Dispose();
    }
}

