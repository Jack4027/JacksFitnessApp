using AutoMapper;
using FluentAssertions;
using JacksFitnessApp.Application.Handlers.Training.Workout;
using JacksFitnessApp.Application.Queries.Training.Workout;
using JacksFitnessApp.Domain.Entities.Training;
using JacksFitnessApp.Domain.Interfaces.Training;
using JacksFitnessApp.Tests.Helpers;
using Moq;
using NUnit.Framework;

namespace JacksFitnessApp.Tests.Handlers.Training;

[TestFixture]
public class GetWorkoutByIdHandlerTests
{
    private Mock<IWorkoutRepository> _repositoryMock;
    private IMapper _mapper;
    private GetWorkoutByIdHandler _handler;

    [SetUp]
    public void SetUp()
    {
        _repositoryMock = new Mock<IWorkoutRepository>();
        _mapper = MapperHelper.CreateMapper();
        _handler = new GetWorkoutByIdHandler(_repositoryMock.Object, _mapper);
    }

    [Test]
    public async Task Handle_WhenSessionExistsAndUserOwnsIt_ReturnsMappedDto()
    {
        var session = new WorkoutSession
        {
            Id = 1,
            UserId = "user-123",
            Date = DateTime.UtcNow,
            Sets = new List<WorkoutSet>(),
            CardioSets = new List<CardioSet>()
        };

        _repositoryMock
            .Setup(r => r.GetByIdWithSetsAsync(1))
            .ReturnsAsync(session);

        var result = await _handler.Handle(
            new GetWorkoutByIdQuery(1, "user-123"), CancellationToken.None);

        result.Should().NotBeNull();
        result.Id.Should().Be(1);
    }

    [Test]
    public async Task Handle_WhenSessionNotFound_ThrowsKeyNotFoundException()
    {
        _repositoryMock
            .Setup(r => r.GetByIdWithSetsAsync(999))
            .ReturnsAsync((WorkoutSession?)null);

        var act = async () => await _handler.Handle(
            new GetWorkoutByIdQuery(999, "user-123"), CancellationToken.None);

        await act.Should().ThrowAsync<KeyNotFoundException>();
    }

    [Test]
    public async Task Handle_WhenUserDoesNotOwnSession_ThrowsUnauthorizedAccessException()
    {
        var session = new WorkoutSession
        {
            Id = 1,
            UserId = "other-user",
            Date = DateTime.UtcNow,
            Sets = new List<WorkoutSet>(),
            CardioSets = new List<CardioSet>()
        };

        _repositoryMock
            .Setup(r => r.GetByIdWithSetsAsync(1))
            .ReturnsAsync(session);

        var act = async () => await _handler.Handle(
            new GetWorkoutByIdQuery(1, "user-123"), CancellationToken.None);

        await act.Should().ThrowAsync<UnauthorizedAccessException>();
    }
}