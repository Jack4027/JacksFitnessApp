using JacksFitnessApp.Application.DTOs.Training.Programme;
using MediatR;

namespace JacksFitnessApp.Application.Commands.Training;

public record AddProgrammeDayCommand(
    int ProgrammeWeekId,
    string Name,
    DayOfWeek? DayOfWeek,
    int OrderIndex
) : IRequest<ProgrammeDayDto>;
