namespace JacksFitnessApp.Application.DTOs.Training.Programme;

public class ProgrammeWeekDto
{
    public int Id { get; set; }
    public int WeekNumber { get; set; }
    public string Notes { get; set; } = string.Empty;
    public List<ProgrammeDayDto> Days { get; set; } = new();
}
