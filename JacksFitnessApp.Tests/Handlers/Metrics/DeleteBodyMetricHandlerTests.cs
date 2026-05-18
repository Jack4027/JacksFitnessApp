using FluentAssertions;
using JacksFitnessApp.Application.Commands.Metrics;
using JacksFitnessApp.Application.Handlers.Metrics;
using JacksFitnessApp.Domain.Entities.Metrics;
using JacksFitnessApp.Domain.Interfaces.Metrics;
using Moq;
using NUnit.Framework;

namespace JacksFitnessApp.Tests.Handlers.Metrics;

[TestFixture]
public class DeleteBodyMetricHandlerTests
{
    private Mock<IBodyMetricRepository> _repositoryMock;
    private DeleteBodyMetricHandler _handler;

    [SetUp]
    public void SetUp()
    {
        _repositoryMock = new Mock<IBodyMetricRepository>();
        _handler = new DeleteBodyMetricHandler(_repositoryMock.Object);
    }

    [Test]
    public async Task Handle_WhenMetricExistsAndUserOwnsIt_DeletesMetric()
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
            .Setup(r => r.DeleteAsync(1))
            .Returns(Task.CompletedTask);

        await _handler.Handle(
            new DeleteBodyMetricCommand(1, "user-123"), CancellationToken.None);

        _repositoryMock.Verify(r => r.DeleteAsync(1), Times.Once);
    }

    [Test]
    public async Task Handle_WhenMetricNotFound_ThrowsKeyNotFoundException()
    {
        _repositoryMock
            .Setup(r => r.GetByIdAsync(999))
            .ReturnsAsync((BodyMetric?)null);

        var act = async () => await _handler.Handle(
            new DeleteBodyMetricCommand(999, "user-123"), CancellationToken.None);

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

        var act = async () => await _handler.Handle(
            new DeleteBodyMetricCommand(1, "user-123"), CancellationToken.None);

        await act.Should().ThrowAsync<UnauthorizedAccessException>();
    }

    [Test]
    public async Task Handle_WhenUserDoesNotOwnMetric_DoesNotCallDeleteAsync()
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

        try
        {
            await _handler.Handle(
                new DeleteBodyMetricCommand(1, "user-123"), CancellationToken.None);
        }
        catch (UnauthorizedAccessException) { }

        _repositoryMock.Verify(r => r.DeleteAsync(It.IsAny<int>()), Times.Never);
    }
}