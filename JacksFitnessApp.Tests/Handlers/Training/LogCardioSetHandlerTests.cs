using AutoMapper;
using FluentAssertions;
using JacksFitnessApp.Application.Commands.Training;
using JacksFitnessApp.Application.Handlers.Training.Workout;
using JacksFitnessApp.Domain.Entities.Training;
using JacksFitnessApp.Domain.Enums.Training;
using JacksFitnessApp.Domain.Interfaces.Training;
using JacksFitnessApp.Tests.Helpers;
using Moq;
using NUnit.Framework;

namespace JacksFitnessApp.Tests.Handlers.Training;

[TestFixture]
public class LogCardioSetHandlerTests
{
    private Mock<IWorkoutRepository> _repositoryMock;
    private IMapper _mapper;
    private LogCardioSetHandler _handler;

    private readonly string _userId = "user-123";
    private readonly int _sessionId = 1;
    private readonly int _exerciseId = 24;

    [SetUp]
    public void SetUp()
    {
        _repositoryMock = new Mock<IWorkoutRepository>();
        _mapper = MapperHelper.CreateMapper();
        _handler = new LogCardioSetHandler(_repositoryMock.Object, _mapper);

        var session = new WorkoutSession
        {
            Id = _sessionId,
            UserId = _userId,
            Date = DateTime.UtcNow,
            Sets = new List<WorkoutSet>(),
            CardioSets = new List<CardioSet>()
        };

        _repositoryMock
            .Setup(r => r.GetByIdWithSetsAsync(_sessionId))
            .ReturnsAsync(session);

        _repositoryMock
            .Setup(r => r.AddCardioSetAsync(It.IsAny<CardioSet>()))
            .ReturnsAsync((CardioSet set) =>
            {
                set.Id = 99;
                set.Exercise = Exercise.CreateGlobal(
                    "Treadmill", "Treadmill running",
                    ExerciseType.Cardio, null, null,
                    ExerciseCategory.Cardio, EquipmentType.Machine);
                return set;
            });
    }

    [Test]
    public async Task Handle_WhenSessionNotFound_ThrowsKeyNotFoundException()
    {
        _repositoryMock
            .Setup(r => r.GetByIdWithSetsAsync(999))
            .ReturnsAsync((WorkoutSession?)null);

        var command = new LogCardioSetCommand(
            WorkoutSessionId: 999,
            ExerciseId: _exerciseId,
            SetNumber: 1,
            DurationSeconds: 1800,
            DistanceKm: 5,
            CaloriesBurned: 300,
            AvgHeartRate: 145,
            MaxHeartRate: 165,
            Notes: ""
        );

        var act = async () => await _handler.Handle(command, CancellationToken.None);

        await act.Should().ThrowAsync<KeyNotFoundException>();
    }

    [Test]
    public async Task Handle_CorrectlyMapsCardioSetFieldsToDto()
    {
        var command = new LogCardioSetCommand(
            WorkoutSessionId: _sessionId,
            ExerciseId: _exerciseId,
            SetNumber: 1,
            DurationSeconds: 1800,
            DistanceKm: 5,
            CaloriesBurned: 300,
            AvgHeartRate: 145,
            MaxHeartRate: 165,
            Notes: "Good run"
        );

        var result = await _handler.Handle(command, CancellationToken.None);

        result.SetNumber.Should().Be(1);
        result.DurationSeconds.Should().Be(1800);
        result.DistanceKm.Should().Be(5);
        result.CaloriesBurned.Should().Be(300);
        result.AvgHeartRate.Should().Be(145);
        result.MaxHeartRate.Should().Be(165);
        result.Notes.Should().Be("Good run");
        result.Exercise.Should().NotBeNull();
    }

    [Test]
    public async Task Handle_WhenNullableFieldsAreNull_CreatesCardioSetSuccessfully()
    {
        var command = new LogCardioSetCommand(
            WorkoutSessionId: _sessionId,
            ExerciseId: _exerciseId,
            SetNumber: 1,
            DurationSeconds: null,
            DistanceKm: null,
            CaloriesBurned: null,
            AvgHeartRate: null,
            MaxHeartRate: null,
            Notes: ""
        );

        var result = await _handler.Handle(command, CancellationToken.None);

        result.Should().NotBeNull();
        result.DurationSeconds.Should().BeNull();
        result.DistanceKm.Should().BeNull();
    }

    [Test]
    public async Task Handle_CallsAddCardioSetAsyncOnce()
    {
        var command = new LogCardioSetCommand(
            WorkoutSessionId: _sessionId,
            ExerciseId: _exerciseId,
            SetNumber: 1,
            DurationSeconds: 1800,
            DistanceKm: 5,
            CaloriesBurned: 300,
            AvgHeartRate: 145,
            MaxHeartRate: 165,
            Notes: ""
        );

        await _handler.Handle(command, CancellationToken.None);

        _repositoryMock.Verify(r => r.AddCardioSetAsync(It.IsAny<CardioSet>()), Times.Once);
    }
}