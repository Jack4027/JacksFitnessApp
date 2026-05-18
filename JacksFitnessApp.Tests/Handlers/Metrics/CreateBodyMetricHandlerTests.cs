using AutoMapper;
using FluentAssertions;
using JacksFitnessApp.Application.Commands.Metrics;
using JacksFitnessApp.Application.Handlers.Metrics;
using JacksFitnessApp.Domain.Entities.Metrics;
using JacksFitnessApp.Domain.Interfaces.Metrics;
using JacksFitnessApp.Tests.Helpers;
using Moq;
using NUnit.Framework;

namespace JacksFitnessApp.Tests.Handlers.Metrics;

[TestFixture]
public class CreateBodyMetricHandlerTests
{
    private Mock<IBodyMetricRepository> _repositoryMock;
    private IMapper _mapper;
    private CreateBodyMetricHandler _handler;

    [SetUp]
    public void SetUp()
    {
        _repositoryMock = new Mock<IBodyMetricRepository>();
        _mapper = MapperHelper.CreateMapper();
        _handler = new CreateBodyMetricHandler(_repositoryMock.Object, _mapper);

        _repositoryMock
            .Setup(r => r.AddAsync(It.IsAny<BodyMetric>()))
            .ReturnsAsync((BodyMetric metric) =>
            {
                metric.Id = 1;
                return metric;
            });
    }

    [Test]
    public async Task Handle_CreatesMetricWithCorrectFields()
    {
        var command = new CreateBodyMetricCommand(
            UserId: "user-123",
            Date: new DateOnly(2026, 5, 16),
            WeightKg: 80.5m,
            BodyFatPercentage: 15.0m,
            MuscleMassKg: null,
            ChestCm: null,
            WaistCm: null,
            HipsCm: null,
            BicepCm: null,
            ThighCm: null,
            Notes: "Morning weigh in"
        );

        var result = await _handler.Handle(command, CancellationToken.None);

        result.WeightKg.Should().Be(80.5m);
        result.BodyFatPercentage.Should().Be(15.0m);
        result.Notes.Should().Be("Morning weigh in");
    }

    [Test]
    public async Task Handle_WhenAllNullableFieldsAreNull_CreatesMetricSuccessfully()
    {
        var command = new CreateBodyMetricCommand(
            UserId: "user-123",
            Date: new DateOnly(2026, 5, 16),
            WeightKg: 80.5m,
            BodyFatPercentage: null,
            MuscleMassKg: null,
            ChestCm: null,
            WaistCm: null,
            HipsCm: null,
            BicepCm: null,
            ThighCm: null,
            Notes: ""
        );

        var result = await _handler.Handle(command, CancellationToken.None);

        result.Should().NotBeNull();
        result.WeightKg.Should().Be(80.5m);
        result.BodyFatPercentage.Should().BeNull();
    }

    [Test]
    public async Task Handle_CallsRepositoryAddAsyncOnce()
    {
        var command = new CreateBodyMetricCommand(
            UserId: "user-123",
            Date: new DateOnly(2026, 5, 16),
            WeightKg: 75m,
            BodyFatPercentage: null,
            MuscleMassKg: null,
            ChestCm: null,
            WaistCm: null,
            HipsCm: null,
            BicepCm: null,
            ThighCm: null,
            Notes: ""
        );

        await _handler.Handle(command, CancellationToken.None);

        _repositoryMock.Verify(r => r.AddAsync(It.IsAny<BodyMetric>()), Times.Once);
    }
}