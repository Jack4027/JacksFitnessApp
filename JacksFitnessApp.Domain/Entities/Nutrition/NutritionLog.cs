using JacksFitnessApp.Domain.Common;

namespace JacksFitnessApp.Domain.Entities.Nutrition;

public class NutritionLog : BaseEntity
{
    public DateOnly Date { get; set; }
    public string UserId { get; set; } = string.Empty;
    public string Notes { get; set; } = string.Empty;

    public ICollection<Meal> Meals { get; set; } = new List<Meal>();

    // Computed properties
    public decimal TotalCalories => Meals.Sum(m => m.TotalCalories);
    public decimal TotalProtein => Meals.Sum(m => m.TotalProtein);
    public decimal TotalCarbs => Meals.Sum(m => m.TotalCarbs);
    public decimal TotalFat => Meals.Sum(m => m.TotalFat);
}