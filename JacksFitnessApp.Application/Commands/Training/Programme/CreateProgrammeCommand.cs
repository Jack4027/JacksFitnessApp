using JacksFitnessApp.Application.DTOs.Training;
using JacksFitnessApp.Application.DTOs.Training.Programme;
using JacksFitnessApp.Domain.Entities.Training;
using JacksFitnessApp.Domain.Enums.Training;
using MediatR;

namespace JacksFitnessApp.Application.Commands.Training;

public record CreateProgrammeCommand(
    string Name,
    string Description,
    int DurationWeeks,
    ProgrammeGoal Goal,
    string UserId
) : IRequest<ProgrammeDto>;
