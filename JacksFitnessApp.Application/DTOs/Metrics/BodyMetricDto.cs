namespace JacksFitnessApp.Application.DTOs.Metrics;

public class BodyMetricDto
{
    public int Id { get; set; }
    public DateOnly Date { get; set; }
    public decimal? WeightKg { get; set; }
    public decimal? BodyFatPercentage { get; set; }
    public decimal? MuscleMassKg { get; set; }
    public decimal? ChestCm { get; set; }
    public decimal? WaistCm { get; set; }
    public decimal? HipsCm { get; set; }
    public decimal? BicepCm { get; set; }
    public decimal? ThighCm { get; set; }
    public string Notes { get; set; } = string.Empty;
}