namespace UCConverter.Application.Tests.Services;

using Moq;
using UCConverter.Application.DTOs;
using UCConverter.Application.Interfaces;
using UCConverter.Application.Services;
using UCConverter.Domain.Entities;
using UCConverter.Domain.Interfaces;
using Xunit;

public class UnitConverterServiceNullHandlingTests
{
    private readonly Mock<IConversionService> _mockConversionService;
    private readonly Mock<IUnitRepository> _mockRepository;
    private readonly UnitConverterService _service;

    public UnitConverterServiceNullHandlingTests()
    {
        _mockConversionService = new Mock<IConversionService>();
        _mockRepository = new Mock<IUnitRepository>();
        _service = new UnitConverterService(_mockConversionService.Object, _mockRepository.Object);
    }

    [Fact]
    public void Constructor_WhenConversionServiceIsNull_ThrowsArgumentNullException()
    {
        // Act & Assert
        Assert.Throws<ArgumentNullException>(() =>
            new UnitConverterService(null!, _mockRepository.Object));
    }

    [Fact]
    public void Constructor_WhenRepositoryIsNull_ThrowsArgumentNullException()
    {
        // Act & Assert
        Assert.Throws<ArgumentNullException>(() =>
            new UnitConverterService(_mockConversionService.Object, null!));
    }
}

