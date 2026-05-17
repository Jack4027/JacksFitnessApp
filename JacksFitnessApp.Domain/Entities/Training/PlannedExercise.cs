using JacksFitnessApp.Domain.Common;

namespace JacksFitnessApp.Domain.Entities.Training;

public class PlannedExercise : BaseEntity
{
    public int OrderIndex { get; set; }
    public int TargetSets { get; set; }
    public int TargetRepsMin { get; set; }
    public int TargetRepsMax { get; set; }
    public decimal? TargetWeight { get; set; }
    public string Notes { get; set; } = string.Empty;
    public int ProgrammeDayId { get; set; }
    public int ExerciseId { get; set; }

    public ProgrammeDay ProgrammeDay { get; set; } = null!;
    public Exercise Exercise { get; set; } = null!;
}