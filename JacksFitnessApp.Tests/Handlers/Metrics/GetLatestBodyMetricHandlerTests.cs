using AutoMapper;
using FluentAssertions;
using JacksFitnessApp.Application.Handlers.Metrics;
using JacksFitnessApp.Application.Queries.Metrics;
using JacksFitnessApp.Domain.Entities.Metrics;
using JacksFitnessApp.Domain.Interfaces.Metrics;
using JacksFitnessApp.Tests.Helpers;
using Moq;
using NUnit.Framework;

namespace JacksFitnessApp.Tests.Handlers.Metrics;

[TestFixture]
public class GetLatestBodyMetricHandlerTests
{
    private Mock<IBodyMetricRepository> _repositoryMock;
    private IMapper _mapper;
    private GetLatestBodyMetricHandler _handler;

    [SetUp]
    public void SetUp()
    {
        _repositoryMock = new Mock<IBodyMetricRepository>();
        _mapper = MapperHelper.CreateMapper();
        _handler = new GetLatestBodyMetricHandler(_repositoryMock.Object, _mapper);
    }

    [Test]
    public async Task Handle_WhenMetricExists_ReturnsMappedDto()
    {
        var metric = new BodyMetric
        {
            Id = 1,
            UserId = "user-123",
            Date = new DateOnly(2026, 5, 16),
            WeightKg = 80.5m,
            BodyFatPercentage = 15.0m
        };

        _repositoryMock
            .Setup(r => r.GetLatestAsync("user-123"))
            .ReturnsAsync(metric);

        var result = await _handler.Handle(
            new GetLatestBodyMetricQuery("user-123"), CancellationToken.None);

        result.Should().NotBeNull();
        result!.WeightKg.Should().Be(80.5m);
        result.BodyFatPercentage.Should().Be(15.0m);
    }

    [Test]
    public async Task Handle_WhenNoMetricExists_ReturnsNull()
    {
        _repositoryMock
            .Setup(r => r.GetLatestAsync("user-123"))
            .ReturnsAsync((BodyMetric?)null);

        var result = await _handler.Handle(
            new GetLatestBodyMetricQuery("user-123"), CancellationToken.None);

        result.Should().BeNull();
    }

    [Test]
    public async Task Handle_WhenNullableFieldsAreNull_MapsCorrectly()
    {
        var metric = new BodyMetric
        {
            Id = 1,
            UserId = "user-123",
            Date = new DateOnly(2026, 5, 16),
            WeightKg = 80.5m,
            BodyFatPercentage = null,
            MuscleMassKg = null,
            ChestCm = null,
            WaistCm = null
        };

        _repositoryMock
            .Setup(r => r.GetLatestAsync("user-123"))
            .ReturnsAsync(metric);

        var result = await _handler.Handle(
            new GetLatestBodyMetricQuery("user-123"), CancellationToken.None);

        result!.BodyFatPercentage.Should().BeNull();
        result.MuscleMassKg.Should().BeNull();
        result.ChestCm.Should().BeNull();
    }
}