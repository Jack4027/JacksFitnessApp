using AutoMapper;
using FluentAssertions;
using JacksFitnessApp.Application.Commands.Training;
using JacksFitnessApp.Application.Handlers.Training.Exercises;
using JacksFitnessApp.Domain.Entities.Training;
using JacksFitnessApp.Domain.Enums.Training;
using JacksFitnessApp.Domain.Interfaces.Training;
using JacksFitnessApp.Tests.Helpers;
using Moq;
using NUnit.Framework;

namespace JacksFitnessApp.Tests.Handlers.Training;

[TestFixture]
public class CreateExerciseHandlerTests
{
    private Mock<IExerciseRepository> _repositoryMock;
    private IMapper _mapper;
    private CreateExerciseHandler _handler;

    [SetUp]
    public void SetUp()
    {
        _repositoryMock = new Mock<IExerciseRepository>();
        _mapper = MapperHelper.CreateMapper();
        _handler = new CreateExerciseHandler(_repositoryMock.Object, _mapper);

        _repositoryMock
            .Setup(r => r.AddAsync(It.IsAny<Exercise>()))
            .ReturnsAsync((Exercise e) => e);
    }

    [Test]
    public async Task Handle_WhenUserIdProvided_CreatesCustomExercise()
    {
        var command = new CreateExerciseCommand(
            Name: "My Exercise",
            Description: "Custom",
            Type: ExerciseType.Strength,
            PrimaryMuscleGroup: MuscleGroup.Chest,
            SecondaryMuscleGroup: null,
            Category: ExerciseCategory.Isolation,
            Equipment: EquipmentType.Dumbbell,
            UserId: "user-123"
        );

        var result = await _handler.Handle(command, CancellationToken.None);

        result.IsCustom.Should().BeTrue();
    }

    [Test]
    public async Task Handle_WhenNoUserIdProvided_CreatesGlobalExercise()
    {
        var command = new CreateExerciseCommand(
            Name: "Global Exercise",
            Description: "Seeded",
            Type: ExerciseType.Strength,
            PrimaryMuscleGroup: MuscleGroup.Back,
            SecondaryMuscleGroup: null,
            Category: ExerciseCategory.Compound,
            Equipment: EquipmentType.Barbell,
            UserId: null
        );

        var result = await _handler.Handle(command, CancellationToken.None);

        result.IsCustom.Should().BeFalse();
    }

    [Test]
    public async Task Handle_CardioExercise_HasNullMuscleGroup()
    {
        var command = new CreateExerciseCommand(
            Name: "Treadmill",
            Description: "Cardio",
            Type: ExerciseType.Cardio,
            PrimaryMuscleGroup: null,
            SecondaryMuscleGroup: null,
            Category: ExerciseCategory.Cardio,
            Equipment: EquipmentType.Machine,
            UserId: null
        );

        var result = await _handler.Handle(command, CancellationToken.None);

        result.PrimaryMuscleGroup.Should().BeNull();
        result.Type.Should().Be(ExerciseType.Cardio);
    }
}