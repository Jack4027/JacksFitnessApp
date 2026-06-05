namespace JacksFitnessApp.Application.DTOs.Coach;

public class LatestMetricsSummary
{
    public decimal? WeightKg { get; set; }
    public decimal? BodyFatPercentage { get; set; }
    public decimal? MuscleMassKg { get; set; }
    public string Date { get; set; } = string.Empty;
}
