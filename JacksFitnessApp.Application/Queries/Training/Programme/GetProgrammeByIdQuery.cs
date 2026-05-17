using JacksFitnessApp.Application.DTOs.Training.Programme;
using MediatR;

namespace JacksFitnessApp.Application.Queries.Training.Programme;

public record GetProgrammeByIdQuery(int Id, string UserId)
    : IRequest<ProgrammeDto>;