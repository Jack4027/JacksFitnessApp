using JacksFitnessApp.Domain.Entities.Nutrition;

namespace JacksFitnessApp.Domain.Interfaces.Nutrition;

public interface IFoodRepository
{
    Task<IEnumerable<FoodItem>> SearchAsync(string query);
    Task<FoodItem?> GetByBarcodeAsync(string barcode);
    Task<FoodItem?> GetByIdAsync(int id);
    Task<FoodItem?> GetByExternalIdAsync(string? externalId);
    Task<FoodItem> AddAsync(FoodItem foodItem);
}