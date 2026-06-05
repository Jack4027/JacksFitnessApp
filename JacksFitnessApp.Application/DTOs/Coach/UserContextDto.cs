namespace JacksFitnessApp.Application.DTOs.Coach;

public class UserContextDto
{
    public List<RecentWorkoutSummary> RecentWorkouts { get; set; } = new();
    public List<PersonalRecordSummary> PersonalRecords { get; set; } = new();
    public LatestMetricsSummary? LatestMetrics { get; set; }
    public TodayNutritionSummary? TodayNutrition { get; set; }
}
