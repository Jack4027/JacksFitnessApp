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
public class UpdateBodyMetricHandlerTests
{
    private Mock<IBodyMetricRepository> _repositoryMock;
    private IMapper _mapper;
    private UpdateBodyMetricHandler _handler;

    [SetUp]
    public void SetUp()
    {
        _repositoryMock = new Mock<IBodyMetricRepository>();
        _mapper = MapperHelper.CreateMapper();
        _handler = new UpdateBodyMetricHandler(_repositoryMock.Object, _mapper);
    }

    [Test]
    public async Task Handle_WhenMetricExistsAndUserOwnsIt_UpdatesMetric()
    {
        var metric = new BodyMetric
        {
            Id = 1,
            UserId = "user-123",
            Date = new DateOnly(2026, 5, 16),
            WeightKg = 80
        };

        _repositoryMock
            .Setup(r => r.GetByIdAsync(1))
            .ReturnsAsync(metric);

        _repositoryMock
            .Setup(r => r.UpdateAsync(It.IsAny<BodyMetric>()))
            .Returns(Task.CompletedTask);

        var command = new UpdateBodyMetricCommand(
            Id: 1,
            UserId: "user-123",
            WeightKg: 81,
            BodyFatPercentage: null,
            MuscleMassKg: null,
            ChestCm: null,
            WaistCm: null,
            HipsCm: null,
            BicepCm: null,
            ThighCm: null,
            Notes: "Updated"
        );

        var result = await _handler.Handle(command, CancellationToken.None);

        result.Should().NotBeNull();
        result.WeightKg.Should().Be(81);
    }

    [Test]
    public async Task Handle_WhenMetricNotFound_ThrowsKeyNotFoundException()
    {
        _repositoryMock
            .Setup(r => r.GetByIdAsync(999))
            .ReturnsAsync((BodyMetric?)null);

        var command = new UpdateBodyMetricCommand(
            Id: 999,
            UserId: "user-123",
            WeightKg: 80,
            BodyFatPercentage: null,
            MuscleMassKg: null,
            ChestCm: null,
            WaistCm: null,
            HipsCm: null,
            BicepCm: null,
            ThighCm: null,
            Notes: ""
        );

        var act = async () => await _handler.Handle(command, CancellationToken.None);

        await act.Should().ThrowAsync<KeyNotFoundException>();
    }

    [Test]
    public async Task Handle_WhenUserDoesNotOwnMetric_ThrowsUnauthorizedAccessException()
    {
        var metric = new BodyMetric
        {
            Id = 1,
            UserId = "other-user",
            Date = new DateOnly(2026, 5, 16),
            WeightKg = 80
        };

        _repositoryMock
            .Setup(r => r.GetByIdAsync(1))
            .ReturnsAsync(metric);

        var command = new UpdateBodyMetricCommand(
            Id: 1,
            UserId: "user-123",
            WeightKg: 81,
            BodyFatPercentage: null,
            MuscleMassKg: null,
            ChestCm: null,
            WaistCm: null,
            HipsCm: null,
            BicepCm: null,
            ThighCm: null,
            Notes: ""
        );

        var act = async () => await _handler.Handle(command, CancellationToken.None);

        await act.Should().ThrowAsync<UnauthorizedAccessException>();
    }
}