using JacksFitnessApp.Application.DTOs.Training.PersonalRecord;
using MediatR;

namespace JacksFitnessApp.Application.Queries.Training.Workout;

public record GetCardioPersonalRecordsQuery(string UserId)
    : IRequest<IEnumerable<CardioPersonalRecordDto>>;