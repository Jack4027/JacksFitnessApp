using JacksFitnessApp.Application.DTOs.Training.Exercise;

namespace JacksFitnessApp.Application.DTOs.Training.Workout;

public class WorkoutSetDto
{
    public int Id { get; set; }
    public int SetNumber { get; set; }
    public int RepsCompleted { get; set; }
    public decimal WeightKg { get; set; }
    public int? RestSeconds { get; set; }
    public bool IsPersonalRecord { get; set; }
    public string Notes { get; set; } = string.Empty;
    public ExerciseDto Exercise { get; set; } = null!;
}
