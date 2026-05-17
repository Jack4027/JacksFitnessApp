using JacksFitnessApp.Application.DTOs.Training.Programme;
using MediatR;

namespace JacksFitnessApp.Application.Commands.Training;

public record AddProgrammeWeekCommand(
    int ProgrammeId,
    int WeekNumber,
    string Notes
) : IRequest<ProgrammeWeekDto>;
