using JacksFitnessApp.Application.DTOs.Training.Programme;
using JacksFitnessApp.Domain.Entities.Training;
using MediatR;

namespace JacksFitnessApp.Application.Commands.Training;

public record UpdateProgrammeCommand(
    int Id,
    string Name,
    string Description,
    int DurationWeeks,
    ProgrammeGoal Goal,
    bool IsActive,
    string UserId
) : IRequest<ProgrammeDto>;
