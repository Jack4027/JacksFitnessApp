namespace JacksFitnessApp.Application.DTOs.Training.PersonalRecord;

public class PersonalRecordDto
{
    public int ExerciseId { get; set; }
    public string ExerciseName { get; set; } = string.Empty;
    public decimal WeightKg { get; set; }
    public int Reps { get; set; }
    public DateTime AchievedOn { get; set; }
}
