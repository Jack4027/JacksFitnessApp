using AutoMapper;
using JacksFitnessApp.Application.DTOs.Nutrition;
using JacksFitnessApp.Application.Interfaces;
using JacksFitnessApp.Application.Queries.Nutrition;
using JacksFitnessApp.Domain.Interfaces.Nutrition;
using MediatR;

namespace JacksFitnessApp.Application.Handlers.Nutrition;

public class SearchFoodItemsHandler : IRequestHandler<SearchFoodItemsQuery, IEnumerable<FoodItemDto>>
{
    private readonly IFoodSearchService _foodSearchService;
    private readonly IMapper _mapper;

    public SearchFoodItemsHandler(IFoodSearchService foodSearchService, IMapper mapper)
    {
        _foodSearchService = foodSearchService;
        _mapper = mapper;
    }

    public async Task<IEnumerable<FoodItemDto>> Handle(SearchFoodItemsQuery request, CancellationToken cancellationToken)
    {
        var items = await _foodSearchService.SearchAsync(request.Query);
        return items;
    }
}
