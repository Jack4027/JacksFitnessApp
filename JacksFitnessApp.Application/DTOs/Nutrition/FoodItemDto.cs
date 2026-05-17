using JacksFitnessApp.Domain.Enums.Nutrition;

namespace JacksFitnessApp.Application.DTOs.Nutrition;

public class FoodItemDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Brand { get; set; }
    public string? Barcode { get; set; }
    public decimal CaloriesPer100g { get; set; }
    public decimal ProteinPer100g { get; set; }
    public decimal CarbsPer100g { get; set; }
    public decimal FatPer100g { get; set; }
    public decimal FibrePer100g { get; set; }
    public FoodItemSource Source { get; set; }
}