using JacksFitnessApp.Application.DTOs.Nutrition;

namespace JacksFitnessApp.Application.Interfaces;

public interface IFoodSearchService
{
    Task<IEnumerable<FoodItemDto>> SearchAsync(string query);
    Task<FoodItemDto?> GetByBarcodeAsync(string barcode);
}