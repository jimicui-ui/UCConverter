namespace UCConverter.Api.Tests.Controllers;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using UCConverter.Api.Controllers;
using UCConverter.Application.DTOs;
using UCConverter.Application.Interfaces;
using UCConverter.Domain.Exceptions;
using Xunit;

/// <summary>
/// Additional scenario tests for ConvertController to improve coverage
/// </summary>
public class ConvertControllerAdditionalScenariosTests
{
    private readonly Mock<IUnitConverterService> _mockService;
    private readonly Mock<ILocalizationService> _mockLocalizationService;
    private readonly Mock<ILogger<ConvertController>> _mockLogger;
    private readonly ConvertController _controller;

    public ConvertControllerAdditionalScenariosTests()
    {
        _mockService = new Mock<IUnitConverterService>();
        _mockLocalizationService = new Mock<ILocalizationService>();
        _mockLogger = new Mock<ILogger<ConvertController>>();
        _controller = new ConvertController(_mockService.Object, _mockLocalizationService.Object, _mockLogger.Object);

        _mockLocalizationService.Setup(l => l.GetErrorMessage(It.IsAny<string>(), It.IsAny<object[]>()))
            .Returns<string, object[]>((key, args) =>
            {
                return key switch
                {
                    "RequestBodyRequired" => "Request body is required",
                    "CategoryRequired" => "Category is required",
                    "FromUnitRequired" => "FromUnit is required",
                    "ToUnitRequired" => "ToUnit is required",
                    "CategoryNotFound" => $"Category '{args[0]}' not found",
                    "UnitNotFound" => $"Unit '{args[0]}' not found",
                    "InvalidConversion" => "Invalid conversion",
                    "InternalServerErrorConversion" => "An error occurred while performing the conversion",
                    _ => "An error occurred"
                };
            });
    }

    [Fact]
    public async Task Convert_WhenCategoryIsNull_ReturnsBadRequest()
    {
        // Arrange
        var request = new ConvertRequestDto
        {
            Category = null!,
            FromUnit = "m",
            ToUnit = "km",
            Value = 10.0
        };

        // Act
        var result = await _controller.Convert(request);

        // Assert
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
        Assert.NotNull(badRequestResult.Value);
    }

    [Fact]
    public async Task Convert_WhenFromUnitIsNull_ReturnsBadRequest()
    {
        // Arrange
        var request = new ConvertRequestDto
        {
            Category = "length",
            FromUnit = null!,
            ToUnit = "km",
            Value = 10.0
        };

        // Act
        var result = await _controller.Convert(request);

        // Assert
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
        Assert.NotNull(badRequestResult.Value);
    }

    [Fact]
    public async Task Convert_WhenToUnitIsNull_ReturnsBadRequest()
    {
        // Arrange
        var request = new ConvertRequestDto
        {
            Category = "length",
            FromUnit = "m",
            ToUnit = null!,
            Value = 10.0
        };

        // Act
        var result = await _controller.Convert(request);

        // Assert
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
        Assert.NotNull(badRequestResult.Value);
    }

    [Fact]
    public async Task Convert_WhenValidRequestWithLocale_ReturnsOk()
    {
        // Arrange
        var request = new ConvertRequestDto
        {
            Category = "length",
            FromUnit = "m",
            ToUnit = "km",
            Value = 1000.0,
            Locale = "zh"
        };

        var response = new ConvertResponseDto
        {
            Result = 1.0,
            FormattedResult = "1 km",
            Precision = 4,
            FromUnit = new UnitInfoDto { Symbol = "m", Name = "meter" },
            ToUnit = new UnitInfoDto { Symbol = "km", Name = "kilometer" }
        };

        _mockService.Setup(s => s.ConvertAsync(request))
            .ReturnsAsync(response);

        // Act
        var result = await _controller.Convert(request);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        Assert.NotNull(okResult.Value);
    }

    [Fact]
    public async Task Convert_WhenValidRequestWithFormula_ReturnsOk()
    {
        // Arrange
        var request = new ConvertRequestDto
        {
            Category = "temperature",
            FromUnit = "°C",
            ToUnit = "°F",
            Value = 25.0
        };

        var response = new ConvertResponseDto
        {
            Result = 77.0,
            FormattedResult = "77 °F",
            Precision = 4,
            Formula = "x * 9/5 + 32",
            FromUnit = new UnitInfoDto { Symbol = "°C", Name = "celsius" },
            ToUnit = new UnitInfoDto { Symbol = "°F", Name = "fahrenheit" }
        };

        _mockService.Setup(s => s.ConvertAsync(request))
            .ReturnsAsync(response);

        // Act
        var result = await _controller.Convert(request);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var returnedResponse = Assert.IsType<ConvertResponseDto>(okResult.Value);
        Assert.Equal(77.0, returnedResponse.Result);
        Assert.Equal("x * 9/5 + 32", returnedResponse.Formula);
    }

    [Fact]
    public async Task Convert_WhenCategoryNotFound_ReturnsNotFoundWithCorrectMessage()
    {
        // Arrange
        var request = new ConvertRequestDto
        {
            Category = "nonexistent",
            FromUnit = "m",
            ToUnit = "km",
            Value = 10.0
        };

        _mockService.Setup(s => s.ConvertAsync(request))
            .ThrowsAsync(new CategoryNotFoundException("nonexistent"));

        // Act
        var result = await _controller.Convert(request);

        // Assert
        var notFoundResult = Assert.IsType<NotFoundObjectResult>(result.Result);
        Assert.NotNull(notFoundResult.Value);
    }

    [Fact]
    public async Task Convert_WhenUnitNotFound_ReturnsNotFoundWithCorrectMessage()
    {
        // Arrange
        var request = new ConvertRequestDto
        {
            Category = "length",
            FromUnit = "nonexistent",
            ToUnit = "km",
            Value = 10.0
        };

        _mockService.Setup(s => s.ConvertAsync(request))
            .ThrowsAsync(new UnitNotFoundException("nonexistent"));

        // Act
        var result = await _controller.Convert(request);

        // Assert
        var notFoundResult = Assert.IsType<NotFoundObjectResult>(result.Result);
        Assert.NotNull(notFoundResult.Value);
    }

    [Fact]
    public async Task Convert_WhenInvalidConversion_ReturnsBadRequestWithDetails()
    {
        // Arrange
        var request = new ConvertRequestDto
        {
            Category = "length",
            FromUnit = "m",
            ToUnit = "kg",
            Value = 10.0
        };

        _mockService.Setup(s => s.ConvertAsync(request))
            .ThrowsAsync(new InvalidConversionException("m", "kg", "length"));

        // Act
        var result = await _controller.Convert(request);

        // Assert
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
        Assert.NotNull(badRequestResult.Value);
    }

    [Fact]
    public async Task Convert_WhenNullReferenceException_ReturnsInternalServerError()
    {
        // Arrange
        var request = new ConvertRequestDto
        {
            Category = "length",
            FromUnit = "m",
            ToUnit = "km",
            Value = 10.0
        };

        _mockService.Setup(s => s.ConvertAsync(request))
            .ThrowsAsync(new NullReferenceException("Null reference"));

        // Act
        var result = await _controller.Convert(request);

        // Assert
        var statusResult = Assert.IsType<ObjectResult>(result.Result);
        Assert.Equal(500, statusResult.StatusCode);
    }

    [Fact]
    public async Task Convert_WhenArgumentException_ReturnsInternalServerError()
    {
        // Arrange
        var request = new ConvertRequestDto
        {
            Category = "length",
            FromUnit = "m",
            ToUnit = "km",
            Value = 10.0
        };

        _mockService.Setup(s => s.ConvertAsync(request))
            .ThrowsAsync(new ArgumentException("Invalid argument"));

        // Act
        var result = await _controller.Convert(request);

        // Assert
        var statusResult = Assert.IsType<ObjectResult>(result.Result);
        Assert.Equal(500, statusResult.StatusCode);
    }
}

