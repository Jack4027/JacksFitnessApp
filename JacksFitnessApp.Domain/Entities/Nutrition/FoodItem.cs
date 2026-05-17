using JacksFitnessApp.Domain.Common;
using JacksFitnessApp.Domain.Enums.Nutrition;

namespace JacksFitnessApp.Domain.Entities.Nutrition;

public class FoodItem : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string? Brand { get; set; }
    public string? Barcode { get; set; }
    public decimal CaloriesPer100g { get; set; }
    public decimal ProteinPer100g { get; set; }
    public decimal CarbsPer100g { get; set; }
    public decimal FatPer100g { get; set; }
    public decimal FibrePer100g { get; set; }
    public FoodItemSource Source { get; set; }
    public string? ExternalId { get; set; }
}

