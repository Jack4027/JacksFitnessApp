using AutoMapper;
using JacksFitnessApp.Application.DTOs.Nutrition;
using JacksFitnessApp.Application.Interfaces;
using JacksFitnessApp.Application.Queries.Nutrition;
using JacksFitnessApp.Domain.Interfaces.Nutrition;
using MediatR;

namespace JacksFitnessApp.Application.Handlers.Nutrition;

public class GetFoodItemByBarcodeHandler : IRequestHandler<GetFoodItemByBarcodeQuery, FoodItemDto>
{
    private readonly IFoodSearchService _foodSearchService;

    public GetFoodItemByBarcodeHandler(IFoodSearchService foodSearchService)
    {
        _foodSearchService = foodSearchService;
    }

    public async Task<FoodItemDto?> Handle(GetFoodItemByBarcodeQuery request, CancellationToken cancellationToken)
    {
        var item = await _foodSearchService.GetByBarcodeAsync(request.Barcode);

        if (item == null) return null;

        return item;
    }
}

