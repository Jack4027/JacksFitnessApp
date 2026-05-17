using MediatR;
using JacksFitnessApp.Application.DTOs.Nutrition;

namespace JacksFitnessApp.Application.Queries.Nutrition;

public record GetFoodItemByBarcodeQuery(string Barcode) : IRequest<FoodItemDto?>;