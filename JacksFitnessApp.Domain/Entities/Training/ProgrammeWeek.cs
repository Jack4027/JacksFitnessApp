using JacksFitnessApp.Domain.Common;

namespace JacksFitnessApp.Domain.Entities.Training;

public class ProgrammeWeek : BaseEntity
{
    public int WeekNumber { get; set; }
    public string Notes { get; set; } = string.Empty;
    public int ProgrammeId { get; set; }

    public Programme Programme { get; set; } = null!;
    public ICollection<ProgrammeDay> Days { get; set; } = new List<ProgrammeDay>();
}