namespace JacksFitnessApp.Application.DTOs.Coach;

public class TodayNutritionSummary
{
    public decimal TotalCalories { get; set; }
    public decimal TotalProtein { get; set; }
    public decimal TotalCarbs { get; set; }
    public decimal TotalFat { get; set; }
    public int MealCount { get; set; }
}