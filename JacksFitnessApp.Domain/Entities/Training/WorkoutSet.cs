using JacksFitnessApp.Domain.Common;

namespace JacksFitnessApp.Domain.Entities.Training;

public class WorkoutSet : BaseEntity
{
    public int SetNumber { get; set; }
    public int RepsCompleted { get; set; }
    public decimal WeightKg { get; set; }
    public int? RestSeconds { get; set; }
    public bool IsPersonalRecord { get; set; } = false;
    public string Notes { get; set; } = string.Empty;
    public int WorkoutSessionId { get; set; }
    public int ExerciseId { get; set; }

    public WorkoutSession WorkoutSession { get; set; } = null!;
    public Exercise Exercise { get; set; } = null!;
}