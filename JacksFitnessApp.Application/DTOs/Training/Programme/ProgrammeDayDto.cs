namespace JacksFitnessApp.Application.DTOs.Training.Programme;

public class ProgrammeDayDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public DayOfWeek? DayOfWeek { get; set; }
    public int OrderIndex { get; set; }
    public List<PlannedExerciseDto> PlannedExercises { get; set; } = new();
}
