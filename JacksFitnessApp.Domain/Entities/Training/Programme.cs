using JacksFitnessApp.Domain.Common;

namespace JacksFitnessApp.Domain.Entities.Training;

public class Programme : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public int DurationWeeks { get; set; }
    public ProgrammeGoal Goal { get; set; }
    public bool IsActive { get; set; } = false;
    public string UserId { get; set; } = string.Empty;

    public ICollection<ProgrammeWeek> Weeks { get; set; } = new List<ProgrammeWeek>();
}

public enum ProgrammeGoal
{
    Strength,
    Hypertrophy,
    Endurance,
    WeightLoss,
    GeneralFitness
}