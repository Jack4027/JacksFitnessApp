using JacksFitnessApp.Application.DTOs.Training.Exercise;
using MediatR;

namespace JacksFitnessApp.Application.Queries.Training.Exercise;

public record GetExerciseByIdQuery(int Id)
    : IRequest<ExerciseDto>;