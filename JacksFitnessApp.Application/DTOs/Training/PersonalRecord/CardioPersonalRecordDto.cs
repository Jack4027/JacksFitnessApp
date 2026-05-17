namespace JacksFitnessApp.Application.DTOs.Training.PersonalRecord;

public class CardioPersonalRecordDto
{
    public int ExerciseId { get; set; }
    public string ExerciseName { get; set; } = string.Empty;
    public decimal? BestDistanceKm { get; set; }
    public int? BestTimeSeconds { get; set; }
    public DateTime AchievedOn { get; set; }
}