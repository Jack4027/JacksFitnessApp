using JacksFitnessApp.Application.DTOs.Training.Exercise;

namespace JacksFitnessApp.Application.DTOs.Training.Programme;

public class PlannedExerciseDto
{
    public int Id { get; set; }
    public int OrderIndex { get; set; }
    public int TargetSets { get; set; }
    public int TargetRepsMin { get; set; }
    public int TargetRepsMax { get; set; }
    public decimal? TargetWeight { get; set; }
    public string Notes { get; set; } = string.Empty;
    public ExerciseDto Exercise { get; set; } = null!;
}