using JacksFitnessApp.Application.DTOs.Training.PersonalRecord;
using MediatR;

namespace JacksFitnessApp.Application.Queries.Training.Workout;

public record GetPersonalRecordsQuery(string UserId)
    : IRequest<IEnumerable<PersonalRecordDto>>;
