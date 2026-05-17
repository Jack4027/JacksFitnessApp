using JacksFitnessApp.Domain.Common;

namespace JacksFitnessApp.Domain.Entities.Training;

public class WorkoutSession : BaseEntity
{
    public DateTime Date { get; set; }
    public int? DurationMinutes { get; set; }
    public string Notes { get; set; } = string.Empty;
    public string UserId { get; set; } = string.Empty;
    public int? ProgrammeDayId { get; set; }

    public ProgrammeDay? ProgrammeDay { get; set; }

    // Separate collections for strength and cardio sets
    public ICollection<WorkoutSet> Sets { get; set; } = new List<WorkoutSet>();
    public ICollection<CardioSet> CardioSets { get; set; } = new List<CardioSet>();
}