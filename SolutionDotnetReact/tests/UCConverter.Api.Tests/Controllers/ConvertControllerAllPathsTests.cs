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
/// Tests to cover all code paths in ConvertController
/// </summary>
public class ConvertControllerAllPathsTests
{
    private readonly Mock<IUnitConverterService> _mockService;
    private readonly Mock<ILocalizationService> _mockLocalizationService;
    private readonly Mock<ILogger<ConvertController>> _mockLogger;
    private readonly ConvertController _controller;

    public ConvertControllerAllPathsTests()
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
    public async Task Convert_WhenAllValidationsPass_ReturnsOk()
    {
        // Arrange
        var request = new ConvertRequestDto
        {
            Category = "length",
            FromUnit = "m",
            ToUnit = "km",
            Value = 1000.0
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
        _mockService.Verify(s => s.ConvertAsync(request), Times.Once);
    }

    [Fact]
    public async Task Convert_WhenCategoryNotFound_LogsWarningWithCategoryName()
    {
        // Arrange
        var request = new ConvertRequestDto
        {
            Category = "nonexistent",
            FromUnit = "m",
            ToUnit = "km",
            Value = 10.0
        };

        var exception = new CategoryNotFoundException("nonexistent");
        _mockService.Setup(s => s.ConvertAsync(request))
            .ThrowsAsync(exception);

        // Act
        await _controller.Convert(request);

        // Assert
        _mockLogger.Verify(
            x => x.Log(
                LogLevel.Warning,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => v.ToString()!.Contains("Category not found")),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
            Times.Once);
    }

    [Fact]
    public async Task Convert_WhenUnitNotFound_LogsWarningWithUnitSymbol()
    {
        // Arrange
        var request = new ConvertRequestDto
        {
            Category = "length",
            FromUnit = "nonexistent",
            ToUnit = "km",
            Value = 10.0
        };

        var exception = new UnitNotFoundException("nonexistent");
        _mockService.Setup(s => s.ConvertAsync(request))
            .ThrowsAsync(exception);

        // Act
        await _controller.Convert(request);

        // Assert
        _mockLogger.Verify(
            x => x.Log(
                LogLevel.Warning,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => v.ToString()!.Contains("Unit not found")),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
            Times.Once);
    }

    [Fact]
    public async Task Convert_WhenInvalidConversion_LogsWarningWithFromAndToUnits()
    {
        // Arrange
        var request = new ConvertRequestDto
        {
            Category = "length",
            FromUnit = "m",
            ToUnit = "kg",
            Value = 10.0
        };

        var exception = new InvalidConversionException("m", "kg", "length");
        _mockService.Setup(s => s.ConvertAsync(request))
            .ThrowsAsync(exception);

        // Act
        await _controller.Convert(request);

        // Assert
        _mockLogger.Verify(
            x => x.Log(
                LogLevel.Warning,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => v.ToString()!.Contains("Invalid conversion")),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
            Times.Once);
    }

    [Fact]
    public async Task Convert_WhenGenericException_LogsErrorWithException()
    {
        // Arrange
        var request = new ConvertRequestDto
        {
            Category = "length",
            FromUnit = "m",
            ToUnit = "km",
            Value = 10.0
        };

        var exception = new Exception("Test exception");
        _mockService.Setup(s => s.ConvertAsync(request))
            .ThrowsAsync(exception);

        // Act
        await _controller.Convert(request);

        // Assert
        _mockLogger.Verify(
            x => x.Log(
                LogLevel.Error,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => v.ToString()!.Contains("Error performing conversion")),
                It.Is<Exception>(e => e == exception),
                It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
            Times.Once);
    }
}

