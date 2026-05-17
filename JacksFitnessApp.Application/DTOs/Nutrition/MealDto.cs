using JacksFitnessApp.Domain.Enums.Nutrition;

namespace JacksFitnessApp.Application.DTOs.Nutrition;

public class MealDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public MealType Type { get; set; }
    public decimal TotalCalories { get; set; }
    public decimal TotalProtein { get; set; }
    public decimal TotalCarbs { get; set; }
    public decimal TotalFat { get; set; }
    public List<MealItemDto> Items { get; set; } = new();
}
