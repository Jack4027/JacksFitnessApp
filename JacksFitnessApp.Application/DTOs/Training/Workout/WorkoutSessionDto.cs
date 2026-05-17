namespace JacksFitnessApp.Application.DTOs.Training.Workout;

public class WorkoutSessionDto
{
    public int Id { get; set; }
    public DateTime Date { get; set; }
    public int? DurationMinutes { get; set; }
    public string Notes { get; set; } = string.Empty;
    public int? ProgrammeDayId { get; set; }
    public string? ProgrammeDayName { get; set; }
    public List<WorkoutSetDto> Sets { get; set; } = new();
    public List<CardioSetDto> CardioSets { get; set; } = new();
}
