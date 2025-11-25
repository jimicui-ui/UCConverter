namespace UCConverter.IntegrationTests;

using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc.Testing;
using UCConverter.Api;
using UCConverter.Application.DTOs;
using Xunit;

public class ApiEndpointsTests : IClassFixture<WebApplicationFactory<Program>>, IDisposable
{
    private readonly WebApplicationFactory<Program> _factory;
    private readonly HttpClient _client;
    private readonly JsonSerializerOptions _jsonOptions;

    public ApiEndpointsTests(WebApplicationFactory<Program> factory)
    {
        _factory = factory;
        _client = _factory.CreateClient();
        _jsonOptions = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };
    }

    [Fact]
    public async Task GetCategories_ShouldReturnOkWithCategories()
    {
        // Act
        var response = await _client.GetAsync("/api/categories");

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var content = await response.Content.ReadAsStringAsync();
        var categories = JsonSerializer.Deserialize<List<CategoryDto>>(content, _jsonOptions);
        Assert.NotNull(categories);
        Assert.NotEmpty(categories);
    }

    [Fact]
    public async Task GetUnitsByCategory_WhenCategoryExists_ShouldReturnOkWithUnits()
    {
        // Act
        var response = await _client.GetAsync("/api/categories/length/units");

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var content = await response.Content.ReadAsStringAsync();
        var units = JsonSerializer.Deserialize<List<UnitDto>>(content, _jsonOptions);
        Assert.NotNull(units);
        Assert.NotEmpty(units);
    }

    [Fact]
    public async Task GetUnitsByCategory_WhenCategoryDoesNotExist_ShouldReturnNotFound()
    {
        // Act
        var response = await _client.GetAsync("/api/categories/nonexistent/units");

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task Convert_WhenValidRequest_ShouldReturnOkWithResult()
    {
        // Arrange
        var request = new ConvertRequestDto
        {
            Category = "length",
            FromUnit = "m",
            ToUnit = "km",
            Value = 1000.0
        };

        // Act
        var response = await _client.PostAsJsonAsync("/api/convert", request);

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var content = await response.Content.ReadAsStringAsync();
        var result = JsonSerializer.Deserialize<ConvertResponseDto>(content, _jsonOptions);
        Assert.NotNull(result);
        Assert.Equal(1.0, result.Result, 4);
        Assert.Equal("m", result.FromUnit.Symbol);
        Assert.Equal("km", result.ToUnit.Symbol);
    }

    [Fact]
    public async Task Convert_WhenRequestIsNull_ShouldReturnBadRequest()
    {
        // Act
        var response = await _client.PostAsJsonAsync<ConvertRequestDto>("/api/convert", null!);

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task Convert_WhenCategoryIsEmpty_ShouldReturnBadRequest()
    {
        // Arrange
        var request = new ConvertRequestDto
        {
            Category = "",
            FromUnit = "m",
            ToUnit = "km",
            Value = 10.0
        };

        // Act
        var response = await _client.PostAsJsonAsync("/api/convert", request);

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task Convert_WhenFromUnitIsEmpty_ShouldReturnBadRequest()
    {
        // Arrange
        var request = new ConvertRequestDto
        {
            Category = "length",
            FromUnit = "",
            ToUnit = "km",
            Value = 10.0
        };

        // Act
        var response = await _client.PostAsJsonAsync("/api/convert", request);

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task Convert_WhenToUnitIsEmpty_ShouldReturnBadRequest()
    {
        // Arrange
        var request = new ConvertRequestDto
        {
            Category = "length",
            FromUnit = "m",
            ToUnit = "",
            Value = 10.0
        };

        // Act
        var response = await _client.PostAsJsonAsync("/api/convert", request);

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task Convert_WhenCategoryNotFound_ShouldReturnNotFound()
    {
        // Arrange
        var request = new ConvertRequestDto
        {
            Category = "nonexistent",
            FromUnit = "m",
            ToUnit = "km",
            Value = 10.0
        };

        // Act
        var response = await _client.PostAsJsonAsync("/api/convert", request);

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task Convert_WhenFromUnitNotFound_ShouldReturnNotFound()
    {
        // Arrange
        var request = new ConvertRequestDto
        {
            Category = "length",
            FromUnit = "nonexistent",
            ToUnit = "km",
            Value = 10.0
        };

        // Act
        var response = await _client.PostAsJsonAsync("/api/convert", request);

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task Convert_WhenToUnitNotFound_ShouldReturnNotFound()
    {
        // Arrange
        var request = new ConvertRequestDto
        {
            Category = "length",
            FromUnit = "m",
            ToUnit = "nonexistent",
            Value = 10.0
        };

        // Act
        var response = await _client.PostAsJsonAsync("/api/convert", request);

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task Convert_WhenConvertingWeight_ShouldReturnOk()
    {
        // Arrange
        var request = new ConvertRequestDto
        {
            Category = "weight",
            FromUnit = "kg",
            ToUnit = "g",
            Value = 1.0
        };

        // Act
        var response = await _client.PostAsJsonAsync("/api/convert", request);

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var content = await response.Content.ReadAsStringAsync();
        var result = JsonSerializer.Deserialize<ConvertResponseDto>(content, _jsonOptions);
        Assert.NotNull(result);
        Assert.Equal(1000.0, result.Result, 4);
    }

    [Fact]
    public async Task Convert_WhenConvertingVolume_ShouldReturnOk()
    {
        // Arrange
        var request = new ConvertRequestDto
        {
            Category = "volume",
            FromUnit = "L",
            ToUnit = "mL",
            Value = 1.0
        };

        // Act
        var response = await _client.PostAsJsonAsync("/api/convert", request);

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var content = await response.Content.ReadAsStringAsync();
        var result = JsonSerializer.Deserialize<ConvertResponseDto>(content, _jsonOptions);
        Assert.NotNull(result);
        Assert.Equal(1000.0, result.Result, 4);
    }

    [Fact]
    public async Task Convert_WhenConvertingFromBaseUnit_ShouldReturnOk()
    {
        // Arrange
        var request = new ConvertRequestDto
        {
            Category = "length",
            FromUnit = "m",
            ToUnit = "ft",
            Value = 1.0
        };

        // Act
        var response = await _client.PostAsJsonAsync("/api/convert", request);

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var content = await response.Content.ReadAsStringAsync();
        var result = JsonSerializer.Deserialize<ConvertResponseDto>(content, _jsonOptions);
        Assert.NotNull(result);
        Assert.True(result.Result > 0);
    }

    [Fact]
    public async Task Convert_WhenConvertingToBaseUnit_ShouldReturnOk()
    {
        // Arrange
        var request = new ConvertRequestDto
        {
            Category = "length",
            FromUnit = "km",
            ToUnit = "m",
            Value = 1.0
        };

        // Act
        var response = await _client.PostAsJsonAsync("/api/convert", request);

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var content = await response.Content.ReadAsStringAsync();
        var result = JsonSerializer.Deserialize<ConvertResponseDto>(content, _jsonOptions);
        Assert.NotNull(result);
        Assert.Equal(1000.0, result.Result, 4);
    }

    [Fact]
    public async Task Convert_WhenConvertingSameUnit_ShouldReturnOk()
    {
        // Arrange
        var request = new ConvertRequestDto
        {
            Category = "length",
            FromUnit = "m",
            ToUnit = "m",
            Value = 10.0
        };

        // Act
        var response = await _client.PostAsJsonAsync("/api/convert", request);

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var content = await response.Content.ReadAsStringAsync();
        var result = JsonSerializer.Deserialize<ConvertResponseDto>(content, _jsonOptions);
        Assert.NotNull(result);
        Assert.Equal(10.0, result.Result, 4);
    }

    [Fact]
    public async Task Convert_WhenConvertingTemperature_KelvinToCelsius_ShouldReturnOk()
    {
        // Arrange
        var request = new ConvertRequestDto
        {
            Category = "temperature",
            FromUnit = "K",
            ToUnit = "°C",
            Value = 298.15
        };

        // Act
        var response = await _client.PostAsJsonAsync("/api/convert", request);

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var content = await response.Content.ReadAsStringAsync();
        var result = JsonSerializer.Deserialize<ConvertResponseDto>(content, _jsonOptions);
        Assert.NotNull(result);
        Assert.Equal(25.0, result.Result, 2); // 298.15K = 25°C
    }

    [Fact]
    public async Task Convert_WhenConvertingTemperature_KelvinToFahrenheit_ShouldReturnOk()
    {
        // Arrange
        var request = new ConvertRequestDto
        {
            Category = "temperature",
            FromUnit = "K",
            ToUnit = "°F",
            Value = 273.15
        };

        // Act
        var response = await _client.PostAsJsonAsync("/api/convert", request);

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var content = await response.Content.ReadAsStringAsync();
        var result = JsonSerializer.Deserialize<ConvertResponseDto>(content, _jsonOptions);
        Assert.NotNull(result);
        Assert.Equal(32.0, result.Result, 2); // 273.15K = 32°F (freezing point)
    }

    [Fact]
    public async Task Convert_WhenConvertingTemperature_CelsiusToFahrenheit_ShouldReturnOk()
    {
        // Arrange
        var request = new ConvertRequestDto
        {
            Category = "temperature",
            FromUnit = "°C",
            ToUnit = "°F",
            Value = 25.0
        };

        // Act
        var response = await _client.PostAsJsonAsync("/api/convert", request);

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var content = await response.Content.ReadAsStringAsync();
        var result = JsonSerializer.Deserialize<ConvertResponseDto>(content, _jsonOptions);
        Assert.NotNull(result);
        Assert.Equal(77.0, result.Result, 1); // 25°C = 77°F
    }

    [Fact]
    public async Task Convert_WhenConvertingTemperature_FahrenheitToCelsius_ShouldReturnOk()
    {
        // Arrange
        var request = new ConvertRequestDto
        {
            Category = "temperature",
            FromUnit = "°F",
            ToUnit = "°C",
            Value = 32.0
        };

        // Act
        var response = await _client.PostAsJsonAsync("/api/convert", request);

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var content = await response.Content.ReadAsStringAsync();
        var result = JsonSerializer.Deserialize<ConvertResponseDto>(content, _jsonOptions);
        Assert.NotNull(result);
        Assert.Equal(0.0, result.Result, 2); // 32°F = 0°C (freezing point)
    }

    [Fact]
    public async Task Convert_WhenConvertingTemperature_CelsiusToKelvin_ShouldReturnOk()
    {
        // Arrange
        var request = new ConvertRequestDto
        {
            Category = "temperature",
            FromUnit = "°C",
            ToUnit = "K",
            Value = 100.0
        };

        // Act
        var response = await _client.PostAsJsonAsync("/api/convert", request);

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var content = await response.Content.ReadAsStringAsync();
        var result = JsonSerializer.Deserialize<ConvertResponseDto>(content, _jsonOptions);
        Assert.NotNull(result);
        Assert.Equal(373.15, result.Result, 2); // 100°C = 373.15K (boiling point)
    }

    [Fact]
    public async Task Convert_WhenConvertingTemperature_FahrenheitToKelvin_ShouldReturnOk()
    {
        // Arrange
        var request = new ConvertRequestDto
        {
            Category = "temperature",
            FromUnit = "°F",
            ToUnit = "K",
            Value = 212.0
        };

        // Act
        var response = await _client.PostAsJsonAsync("/api/convert", request);

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var content = await response.Content.ReadAsStringAsync();
        var result = JsonSerializer.Deserialize<ConvertResponseDto>(content, _jsonOptions);
        Assert.NotNull(result);
        Assert.Equal(373.15, result.Result, 2); // 212°F = 373.15K (boiling point)
    }

    public void Dispose()
    {
        _client?.Dispose();
    }
}

