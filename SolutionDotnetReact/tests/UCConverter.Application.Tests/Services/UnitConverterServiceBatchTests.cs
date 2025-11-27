namespace UCConverter.Application.Tests.Services;

using Moq;
using UCConverter.Application.DTOs;
using UCConverter.Application.Interfaces;
using UCConverter.Application.Services;
using UCConverter.Domain.Entities;
using UCConverter.Domain.Interfaces;
using Xunit;

public class UnitConverterServiceBatchTests
{
    private readonly Mock<IConversionService> _mockConversionService;
    private readonly Mock<IUnitRepository> _mockRepository;
    private readonly Mock<ILocalizationService> _mockLocalizationService;
    private readonly UnitConverterService _service;

    public UnitConverterServiceBatchTests()
    {
        _mockConversionService = new Mock<IConversionService>();
        _mockRepository = new Mock<IUnitRepository>();
        _mockLocalizationService = new Mock<ILocalizationService>();
        _service = new UnitConverterService(
            _mockConversionService.Object,
            _mockRepository.Object,
            _mockLocalizationService.Object);
    }

    [Fact]
    public async Task ConvertBatchAsync_WhenValidRequest_ReturnsMultipleResults()
    {
        // Arrange
        var request = new ConvertRequestDto
        {
            Category = "length",
            FromUnit = "m",
            ToUnit = "km",
            Value = 1000.0
        };
        var targetUnits = new[] { "km", "ft", "in" };

        var results = new List<ConversionResult>
        {
            new ConversionResult
            {
                Result = 1.0,
                FormattedResult = "1 km",
                Precision = 2,
                Formula = "x / 1000",
                FromUnit = new Unit { Symbol = "m", Name = "meter" },
                ToUnit = new Unit { Symbol = "km", Name = "kilometer" }
            },
            new ConversionResult
            {
                Result = 3280.84,
                FormattedResult = "3280.84 ft",
                Precision = 2,
                Formula = "x * 3.28084",
                FromUnit = new Unit { Symbol = "m", Name = "meter" },
                ToUnit = new Unit { Symbol = "ft", Name = "foot" }
            },
            new ConversionResult
            {
                Result = 39370.1,
                FormattedResult = "39370.1 in",
                Precision = 2,
                Formula = "x * 39.3701",
                FromUnit = new Unit { Symbol = "m", Name = "meter" },
                ToUnit = new Unit { Symbol = "in", Name = "inch" }
            }
        };

        _mockConversionService.Setup(s => s.ConvertBatchAsync("length", "m", targetUnits, 1000.0))
            .ReturnsAsync(results);

        // Act
        var result = await _service.ConvertBatchAsync(request, targetUnits);

        // Assert
        Assert.NotNull(result);
        var resultList = result.ToList();
        Assert.Equal(3, resultList.Count);
        Assert.Equal(1.0, resultList[0].Result);
        Assert.Equal(3280.84, resultList[1].Result);
        Assert.Equal(39370.1, resultList[2].Result);
    }

    [Fact]
    public async Task ConvertBatchAsync_WhenEmptyTargetUnits_ReturnsEmpty()
    {
        // Arrange
        var request = new ConvertRequestDto
        {
            Category = "length",
            FromUnit = "m",
            ToUnit = "km",
            Value = 1000.0
        };
        var targetUnits = Array.Empty<string>();

        _mockConversionService.Setup(s => s.ConvertBatchAsync("length", "m", targetUnits, 1000.0))
            .ReturnsAsync(Enumerable.Empty<ConversionResult>());

        // Act
        var result = await _service.ConvertBatchAsync(request, targetUnits);

        // Assert
        Assert.NotNull(result);
        Assert.Empty(result);
    }

    [Fact]
    public async Task ConvertBatchAsync_WhenNullTargetUnits_ReturnsEmpty()
    {
        // Arrange
        var request = new ConvertRequestDto
        {
            Category = "length",
            FromUnit = "m",
            ToUnit = "km",
            Value = 1000.0
        };

        _mockConversionService.Setup(s => s.ConvertBatchAsync("length", "m", null!, 1000.0))
            .ReturnsAsync(Enumerable.Empty<ConversionResult>());

        // Act
        var result = await _service.ConvertBatchAsync(request, null!);

        // Assert
        Assert.NotNull(result);
        Assert.Empty(result);
    }
}

