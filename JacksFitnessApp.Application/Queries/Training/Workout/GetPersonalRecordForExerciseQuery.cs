using JacksFitnessApp.Application.DTOs.Training.PersonalRecord;
using MediatR;

namespace JacksFitnessApp.Application.Queries.Training.Workout;

public record GetPersonalRecordForExerciseQuery(string UserId, int ExerciseId)
    : IRequest<PersonalRecordDto?>;
