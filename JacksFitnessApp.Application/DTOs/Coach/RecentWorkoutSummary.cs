namespace JacksFitnessApp.Application.DTOs.Coach;

public class RecentWorkoutSummary
{
    public string Date { get; set; } = string.Empty;
    public int TotalSets { get; set; }
    public int? DurationMinutes { get; set; }
    public decimal TotalVolume { get; set; }
    public List<string> Exercises { get; set; } = new();
}
