using JacksFitnessApp.Domain.Common;
using JacksFitnessApp.Domain.Enums.Nutrition;

namespace JacksFitnessApp.Domain.Entities.Nutrition;

public class Meal : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public MealType Type { get; set; }
    public int NutritionLogId { get; set; }

    public NutritionLog NutritionLog { get; set; } = null!;
    public ICollection<MealItem> Items { get; set; } = new List<MealItem>();

    public decimal TotalCalories => Items.Sum(i => i.Calories);
    public decimal TotalProtein => Items.Sum(i => i.Protein);
    public decimal TotalCarbs => Items.Sum(i => i.Carbs);
    public decimal TotalFat => Items.Sum(i => i.Fat);
}