namespace UCConverter.Application.Tests.Services;

using Moq;
using UCConverter.Application.DTOs;
using UCConverter.Application.Interfaces;
using UCConverter.Application.Services;
using UCConverter.Domain.Entities;
using UCConverter.Domain.Interfaces;
using Xunit;

public class UnitConverterServiceEdgeCasesTests
{
    private readonly Mock<IConversionService> _mockConversionService;
    private readonly Mock<IUnitRepository> _mockRepository;
    private readonly UnitConverterService _service;

    public UnitConverterServiceEdgeCasesTests()
    {
        _mockConversionService = new Mock<IConversionService>();
        _mockRepository = new Mock<IUnitRepository>();
        _service = new UnitConverterService(_mockConversionService.Object, _mockRepository.Object);
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

        _mockConversionService.Setup(s => s.ConvertBatchAsync("length", "m", It.IsAny<IEnumerable<string>>(), 1000.0))
            .ReturnsAsync(Enumerable.Empty<ConversionResult>());

        // Act
        var result = await _service.ConvertBatchAsync(request, Enumerable.Empty<string>());

        // Assert
        Assert.NotNull(result);
        Assert.Empty(result);
    }

    [Fact]
    public async Task GetAllCategoriesAsync_WhenEmpty_ReturnsEmpty()
    {
        // Arrange
        _mockRepository.Setup(r => r.GetAllCategoriesAsync())
            .ReturnsAsync(Enumerable.Empty<Category>());

        // Act
        var result = await _service.GetAllCategoriesAsync();

        // Assert
        Assert.NotNull(result);
        Assert.Empty(result);
    }

    [Fact]
    public async Task GetUnitsByCategoryAsync_WhenEmpty_ReturnsEmpty()
    {
        // Arrange
        _mockRepository.Setup(r => r.GetUnitsByCategoryAsync("test"))
            .ReturnsAsync(Enumerable.Empty<Unit>());

        // Act
        var result = await _service.GetUnitsByCategoryAsync("test");

        // Assert
        Assert.NotNull(result);
        Assert.Empty(result);
    }
}

