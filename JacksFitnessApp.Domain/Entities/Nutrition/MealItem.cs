using JacksFitnessApp.Domain.Common;

namespace JacksFitnessApp.Domain.Entities.Nutrition;

public class MealItem : BaseEntity
{
    public decimal QuantityGrams { get; set; }
    public int MealId { get; set; }
    public int FoodItemId { get; set; }

    public Meal Meal { get; set; } = null!;
    public FoodItem FoodItem { get; set; } = null!;

    // Computed from quantity and food item macros per 100g
    public decimal Calories => FoodItem.CaloriesPer100g * QuantityGrams / 100;
    public decimal Protein => FoodItem.ProteinPer100g * QuantityGrams / 100;
    public decimal Carbs => FoodItem.CarbsPer100g * QuantityGrams / 100;
    public decimal Fat => FoodItem.FatPer100g * QuantityGrams / 100;
}