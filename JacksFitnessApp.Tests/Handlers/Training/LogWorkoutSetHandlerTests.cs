using AutoMapper;
using FluentAssertions;
using JacksFitnessApp.Application.Commands.Training;
using JacksFitnessApp.Application.Handlers.Training.Workout;
using JacksFitnessApp.Domain.Entities.Training;
using JacksFitnessApp.Domain.Enums.Training;
using JacksFitnessApp.Domain.Interfaces.Training;
using JacksFitnessApp.Tests.Helpers;
using Moq;

namespace JacksFitnessApp.Tests.Handlers.Training;

[TestFixture]
public class LogWorkoutSetHandlerTests
{
    private Mock<IWorkoutRepository> _repositoryMock;
    private IMapper _mapper;
    private LogWorkoutSetHandler _handler;

    private readonly string _userId = "user-123";
    private readonly int _sessionId = 1;
    private readonly int _exerciseId = 1;

    [SetUp]
    public void SetUp()
    {
        _repositoryMock = new Mock<IWorkoutRepository>();
        _mapper = MapperHelper.CreateMapper();
        _handler = new LogWorkoutSetHandler(_repositoryMock.Object, _mapper);

        // Default session setup
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

        // Default AddSetAsync — returns the set with exercise loaded
        _repositoryMock
            .Setup(r => r.AddSetAsync(It.IsAny<WorkoutSet>()))
            .ReturnsAsync((WorkoutSet set) =>
            {
                set.Id = 99;
                set.Exercise = Exercise.CreateGlobal(
                    "Bench Press",
                    "Barbell bench press",
                    ExerciseType.Strength,
                    MuscleGroup.Chest,
                    null,
                    ExerciseCategory.Compound,
                    EquipmentType.Barbell
                );
                return set;
            });
    }

    [Test]
    public async Task Handle_WhenNoPreviousRecord_SetsIsPersonalRecordTrue()
    {
        // Arrange — no existing PR for this exercise
        _repositoryMock
            .Setup(r => r.GetPersonalRecordForExerciseAsync(_userId, _exerciseId))
            .ReturnsAsync((WorkoutSet?)null);

        var command = new LogWorkoutSetCommand(
            WorkoutSessionId: _sessionId,
            ExerciseId: _exerciseId,
            SetNumber: 1,
            RepsCompleted: 8,
            WeightKg: 80,
            RestSeconds: 90,
            Notes: ""
        );

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsPersonalRecord.Should().BeTrue();
    }

    [Test]
    public async Task Handle_WhenNewWeightExceedsPreviousRecord_SetsIsPersonalRecordTrue()
    {
        // Arrange — existing PR of 80kg
        var existingPR = new WorkoutSet
        {
            Id = 1,
            ExerciseId = _exerciseId,
            WeightKg = 80,
            WorkoutSession = new WorkoutSession { UserId = _userId }
        };

        _repositoryMock
            .Setup(r => r.GetPersonalRecordForExerciseAsync(_userId, _exerciseId))
            .ReturnsAsync(existingPR);

        var command = new LogWorkoutSetCommand(
            WorkoutSessionId: _sessionId,
            ExerciseId: _exerciseId,
            SetNumber: 1,
            RepsCompleted: 8,
            WeightKg: 85, // heavier than existing PR
            RestSeconds: 90,
            Notes: ""
        );

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsPersonalRecord.Should().BeTrue();
    }

    [Test]
    public async Task Handle_WhenNewWeightBelowPreviousRecord_SetsIsPersonalRecordFalse()
    {
        // Arrange — existing PR of 100kg
        var existingPR = new WorkoutSet
        {
            Id = 1,
            ExerciseId = _exerciseId,
            WeightKg = 100,
            WorkoutSession = new WorkoutSession { UserId = _userId }
        };

        _repositoryMock
            .Setup(r => r.GetPersonalRecordForExerciseAsync(_userId, _exerciseId))
            .ReturnsAsync(existingPR);

        var command = new LogWorkoutSetCommand(
            WorkoutSessionId: _sessionId,
            ExerciseId: _exerciseId,
            SetNumber: 1,
            RepsCompleted: 8,
            WeightKg: 80, // lighter than existing PR
            RestSeconds: 90,
            Notes: ""
        );

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsPersonalRecord.Should().BeFalse();
    }

    [Test]
    public async Task Handle_WhenNewWeightEqualsToPreviousRecord_SetsIsPersonalRecordFalse()
    {
        // Arrange — existing PR of 80kg
        var existingPR = new WorkoutSet
        {
            Id = 1,
            ExerciseId = _exerciseId,
            WeightKg = 80,
            WorkoutSession = new WorkoutSession { UserId = _userId }
        };

        _repositoryMock
            .Setup(r => r.GetPersonalRecordForExerciseAsync(_userId, _exerciseId))
            .ReturnsAsync(existingPR);

        var command = new LogWorkoutSetCommand(
            WorkoutSessionId: _sessionId,
            ExerciseId: _exerciseId,
            SetNumber: 1,
            RepsCompleted: 8,
            WeightKg: 80, // equal to existing PR — not a new record
            RestSeconds: 90,
            Notes: ""
        );

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsPersonalRecord.Should().BeFalse();
    }

    [Test]
    public async Task Handle_WhenSessionNotFound_ThrowsKeyNotFoundException()
    {
        // Arrange
        _repositoryMock
            .Setup(r => r.GetByIdWithSetsAsync(999))
            .ReturnsAsync((WorkoutSession?)null);

        var command = new LogWorkoutSetCommand(
            WorkoutSessionId: 999,
            ExerciseId: _exerciseId,
            SetNumber: 1,
            RepsCompleted: 8,
            WeightKg: 80,
            RestSeconds: null,
            Notes: ""
        );

        // Act
        var act = async () => await _handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<KeyNotFoundException>();
    }

    [Test]
    public async Task Handle_CorrectlyMapsSetFieldsToDto()
    {
        // Arrange
        _repositoryMock
            .Setup(r => r.GetPersonalRecordForExerciseAsync(_userId, _exerciseId))
            .ReturnsAsync((WorkoutSet?)null);

        var command = new LogWorkoutSetCommand(
            WorkoutSessionId: _sessionId,
            ExerciseId: _exerciseId,
            SetNumber: 3,
            RepsCompleted: 10,
            WeightKg: 75,
            RestSeconds: 120,
            Notes: "Felt strong"
        );

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.SetNumber.Should().Be(3);
        result.RepsCompleted.Should().Be(10);
        result.WeightKg.Should().Be(75);
        result.RestSeconds.Should().Be(120);
        result.Notes.Should().Be("Felt strong");
        result.Exercise.Should().NotBeNull();
        result.Exercise.Name.Should().Be("Bench Press");
    }
}