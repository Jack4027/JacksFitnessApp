using MediatR;
using JacksFitnessApp.Application.DTOs.Nutrition;

namespace JacksFitnessApp.Application.Commands.Nutrition;

public record CreateFoodItemCommand(
    string Name,
    string? Brand,
    string? Barcode,
    decimal CaloriesPer100g,
    decimal ProteinPer100g,
    decimal CarbsPer100g,
    decimal FatPer100g,
    decimal FibrePer100g
) : IRequest<FoodItemDto>;