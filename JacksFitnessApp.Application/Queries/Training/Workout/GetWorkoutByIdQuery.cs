using JacksFitnessApp.Application.DTOs.Training.Workout;
using MediatR;

namespace JacksFitnessApp.Application.Queries.Training.Workout;

public record GetWorkoutByIdQuery(int Id, string UserId)
    : IRequest<WorkoutSessionDto>;
