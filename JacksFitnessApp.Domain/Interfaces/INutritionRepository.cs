using JacksFitnessApp.Domain.Entities.Nutrition;

namespace JacksFitnessApp.Domain.Interfaces.Nutrition;

public interface INutritionRepository
{
    Task<NutritionLog?> GetByDateAsync(string userId, DateOnly date);
    Task<NutritionLog?> GetByIdAsync(int id);
    Task<IEnumerable<NutritionLog>> GetByUserIdAsync(string userId);
    Task<NutritionLog> AddAsync(NutritionLog log);
    Task UpdateAsync(NutritionLog log);
    Task DeleteAsync(int id);
    Task<Meal> AddMealAsync(Meal meal);

    Task DeleteMealAsync(int id);
    Task<MealItem> AddMealItemAsync(MealItem mealItem);
    Task DeleteMealItemAsync(int id);
}