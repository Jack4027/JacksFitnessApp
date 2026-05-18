using AutoMapper;
using FluentAssertions;
using JacksFitnessApp.Application.Commands.Training;
using JacksFitnessApp.Application.Handlers.Training.Programme;
using JacksFitnessApp.Domain.Entities.Training;
using JacksFitnessApp.Domain.Enums.Training;
using JacksFitnessApp.Domain.Interfaces.Training;
using JacksFitnessApp.Tests.Helpers;
using Moq;

namespace JacksFitnessApp.Tests.Handlers.Training;

[TestFixture]
public class AddPlannedExerciseHandlerTests
{
    private Mock<IProgrammeRepository> _repositoryMock;
    private Mock<IExerciseRepository> _exerciseRepositoryMock;
    private IMapper _mapper;
    private AddPlannedExerciseHandler _handler;

    [SetUp]
    public void SetUp()
    {
        _repositoryMock = new Mock<IProgrammeRepository>();
        _exerciseRepositoryMock = new Mock<IExerciseRepository>();
        _mapper = MapperHelper.CreateMapper();
        _handler = new AddPlannedExerciseHandler(
            _repositoryMock.Object,
            _exerciseRepositoryMock.Object,
            _mapper);

        var exercise = Exercise.CreateGlobal("Bench Press", "",
            ExerciseType.Strength, MuscleGroup.Chest, null,
            ExerciseCategory.Compound, EquipmentType.Barbell);

        _exerciseRepositoryMock
            .Setup(r => r.GetByIdAsync(1))
            .ReturnsAsync(exercise);

        _repositoryMock
            .Setup(r => r.AddPlannedExerciseAsync(It.IsAny<PlannedExercise>()))
            .ReturnsAsync((PlannedExercise pe) =>
            {
                pe.Id = 1;
                pe.Exercise = exercise;
                return pe;
            });
    }

    [Test]
    public async Task Handle_WhenDayNotFound_ThrowsKeyNotFoundException()
    {
        _repositoryMock
            .Setup(r => r.GetDayByIdAsync(999))
            .ReturnsAsync((ProgrammeDay?)null);

        var command = new AddPlannedExerciseCommand(
            ProgrammeDayId: 999,
            ExerciseId: 1,
            OrderIndex: 1,
            TargetSets: 3,
            TargetRepsMin: 8,
            TargetRepsMax: 12,
            TargetWeight: null,
            Notes: ""
        );

        var act = async () => await _handler.Handle(command, CancellationToken.None);

        await act.Should().ThrowAsync<KeyNotFoundException>();
    }

    [Test]
    public async Task Handle_WhenExerciseNotFound_ThrowsKeyNotFoundException()
    {
        var day = new ProgrammeDay
        {
            Id = 1,
            Name = "Push Day",
            ProgrammeWeekId = 1,
            PlannedExercises = new List<PlannedExercise>()
        };

        _repositoryMock
            .Setup(r => r.GetDayByIdAsync(1))
            .ReturnsAsync(day);

        _exerciseRepositoryMock
            .Setup(r => r.GetByIdAsync(999))
            .ReturnsAsync((Exercise?)null);

        var command = new AddPlannedExerciseCommand(
            ProgrammeDayId: 1,
            ExerciseId: 999,
            OrderIndex: 1,
            TargetSets: 3,
            TargetRepsMin: 8,
            TargetRepsMax: 12,
            TargetWeight: null,
            Notes: ""
        );

        var act = async () => await _handler.Handle(command, CancellationToken.None);

        await act.Should().ThrowAsync<KeyNotFoundException>();
    }

    [Test]
    public async Task Handle_WhenDayAndExerciseExist_AddsPlannedExercise()
    {
        var day = new ProgrammeDay
        {
            Id = 1,
            Name = "Push Day",
            ProgrammeWeekId = 1,
            PlannedExercises = new List<PlannedExercise>()
        };

        _repositoryMock
            .Setup(r => r.GetDayByIdAsync(1))
            .ReturnsAsync(day);

        var command = new AddPlannedExerciseCommand(
            ProgrammeDayId: 1,
            ExerciseId: 1,
            OrderIndex: 1,
            TargetSets: 3,
            TargetRepsMin: 8,
            TargetRepsMax: 12,
            TargetWeight: 80,
            Notes: "Focus on form"
        );

        var result = await _handler.Handle(command, CancellationToken.None);

        result.Should().NotBeNull();
        result.TargetSets.Should().Be(3);
        result.TargetRepsMin.Should().Be(8);
        result.TargetRepsMax.Should().Be(12);
        result.Exercise.Should().NotBeNull();
        result.Exercise.Name.Should().Be("Bench Press");
    }
}