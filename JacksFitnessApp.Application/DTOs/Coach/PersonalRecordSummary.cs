namespace JacksFitnessApp.Application.DTOs.Coach;

public class PersonalRecordSummary
{
    public string ExerciseName { get; set; } = string.Empty;
    public decimal WeightKg { get; set; }
    public int Reps { get; set; }
    public string AchievedOn { get; set; } = string.Empty;
}
