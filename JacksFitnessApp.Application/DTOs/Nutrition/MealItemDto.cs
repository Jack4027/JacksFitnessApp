namespace JacksFitnessApp.Application.DTOs.Nutrition;

public class MealItemDto
{
    public int Id { get; set; }
    public decimal QuantityGrams { get; set; }
    public decimal Calories { get; set; }
    public decimal Protein { get; set; }
    public decimal Carbs { get; set; }
    public decimal Fat { get; set; }
    public FoodItemDto FoodItem { get; set; } = null!;
}