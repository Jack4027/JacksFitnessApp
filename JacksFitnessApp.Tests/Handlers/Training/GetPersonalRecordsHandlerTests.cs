using AutoMapper;
using FluentAssertions;
using JacksFitnessApp.Application.Handlers.Training.Workout;
using JacksFitnessApp.Application.Queries.Training.Workout;
using JacksFitnessApp.Domain.Entities.Training;
using JacksFitnessApp.Domain.Enums.Training;
using JacksFitnessApp.Domain.Interfaces.Training;
using JacksFitnessApp.Tests.Helpers;
using Moq;
using NUnit.Framework;

namespace JacksFitnessApp.Tests.Handlers.Training;

[TestFixture]
public class GetPersonalRecordsHandlerTests
{
    private Mock<IWorkoutRepository> _repositoryMock;
    private IMapper _mapper;
    private GetPersonalRecordsHandler _handler;

    [SetUp]
    public void SetUp()
    {
        _repositoryMock = new Mock<IWorkoutRepository>();
        _mapper = MapperHelper.CreateMapper();
        _handler = new GetPersonalRecordsHandler(_repositoryMock.Object, _mapper);
    }

    [Test]
    public async Task Handle_ReturnsPersonalRecordsForUser()
    {
        var session = new WorkoutSession
        {
            Id = 1,
            UserId = "user-123",
            Date = DateTime.UtcNow
        };

        var exercise = Exercise.CreateGlobal("Bench Press", "", ExerciseType.Strength,
            MuscleGroup.Chest, null, ExerciseCategory.Compound, EquipmentType.Barbell);

        var records = new List<WorkoutSet>
        {
            new WorkoutSet
            {
                Id = 1,
                ExerciseId = 1,
                WeightKg = 100,
                RepsCompleted = 5,
                SetNumber = 1,
                IsPersonalRecord = true,
                Exercise = exercise,
                WorkoutSession = session
            }
        };

        _repositoryMock
            .Setup(r => r.GetPersonalRecordsAsync("user-123"))
            .ReturnsAsync(records);

        var result = await _handler.Handle(
            new GetPersonalRecordsQuery("user-123"), CancellationToken.None);

        result.Should().HaveCount(1);
        result.First().WeightKg.Should().Be(100);
        result.First().ExerciseName.Should().Be("Bench Press");
    }

    [Test]
    public async Task Handle_WhenNoRecords_ReturnsEmptyList()
    {
        _repositoryMock
            .Setup(r => r.GetPersonalRecordsAsync("user-123"))
            .ReturnsAsync(new List<WorkoutSet>());

        var result = await _handler.Handle(
            new GetPersonalRecordsQuery("user-123"), CancellationToken.None);

        result.Should().BeEmpty();
    }
}