using JacksFitnessApp.Domain.Common;

namespace JacksFitnessApp.Domain.Entities.Training;

public class ProgrammeDay : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public DayOfWeek? DayOfWeek { get; set; }
    public int OrderIndex { get; set; }
    public int ProgrammeWeekId { get; set; }

    public ProgrammeWeek ProgrammeWeek { get; set; } = null!;
    public ICollection<PlannedExercise> PlannedExercises { get; set; } = new List<PlannedExercise>();
    public ICollection<WorkoutSession> WorkoutSessions { get; set; } = new List<WorkoutSession>();
}