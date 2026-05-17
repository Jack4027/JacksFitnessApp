using JacksFitnessApp.Application.DTOs.Training;
using JacksFitnessApp.Application.DTOs.Training.Exercise;
using JacksFitnessApp.Domain.Enums.Training;
using MediatR;

namespace JacksFitnessApp.Application.Commands.Training;

public record CreateExerciseCommand(
    string Name,
    string Description,
    ExerciseType Type,
    MuscleGroup? PrimaryMuscleGroup,
    MuscleGroup? SecondaryMuscleGroup,
    ExerciseCategory Category,
    EquipmentType Equipment,
    string? UserId
) : IRequest<ExerciseDto>;
