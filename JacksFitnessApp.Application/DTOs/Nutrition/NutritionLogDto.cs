namespace JacksFitnessApp.Application.DTOs.Nutrition;

public class NutritionLogDto
{
    public int Id { get; set; }
    public DateOnly Date { get; set; }
    public string Notes { get; set; } = string.Empty;
    public decimal TotalCalories { get; set; }
    public decimal TotalProtein { get; set; }
    public decimal TotalCarbs { get; set; }
    public decimal TotalFat { get; set; }
    public List<MealDto> Meals { get; set; } = new();
}
