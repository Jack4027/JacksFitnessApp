using JacksFitnessApp.Application.DTOs.Training;
using JacksFitnessApp.Application.DTOs.Training.Exercise;
using JacksFitnessApp.Domain.Enums.Training;
using MediatR;

namespace JacksFitnessApp.Application.Queries.Training.Exercise;

public record GetExercisesQuery(MuscleGroup? MuscleGroup = null)
    : IRequest<IEnumerable<ExerciseDto>>;
