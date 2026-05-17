using JacksFitnessApp.Domain.Entities.Training;
using JacksFitnessApp.Domain.Enums.Training;

namespace JacksFitnessApp.Application.DTOs.Training.Programme;

public class ProgrammeDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public int DurationWeeks { get; set; }
    public ProgrammeGoal Goal { get; set; }
    public bool IsActive { get; set; }
    public List<ProgrammeWeekDto> Weeks { get; set; } = new();
}
