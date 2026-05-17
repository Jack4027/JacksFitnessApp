using JacksFitnessApp.Application.DTOs.Training;
using JacksFitnessApp.Application.DTOs.Training.Programme;
using MediatR;

namespace JacksFitnessApp.Application.Queries.Training.Programme;

public record GetProgrammesQuery(string UserId)
    : IRequest<IEnumerable<ProgrammeDto>>;
