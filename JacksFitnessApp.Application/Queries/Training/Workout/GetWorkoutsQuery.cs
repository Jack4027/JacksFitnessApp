using JacksFitnessApp.Application.DTOs.Training;
using JacksFitnessApp.Application.DTOs.Training.Workout;
using MediatR;

namespace JacksFitnessApp.Application.Queries.Training.Workout;

public record GetWorkoutsQuery(string UserId)
    : IRequest<IEnumerable<WorkoutSessionDto>>;
