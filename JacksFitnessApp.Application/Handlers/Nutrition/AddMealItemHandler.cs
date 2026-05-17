using AutoMapper;
using MediatR;
using JacksFitnessApp.Application.DTOs.Nutrition;
using JacksFitnessApp.Domain.Entities.Nutrition;
using JacksFitnessApp.Domain.Interfaces.Nutrition;
using JacksFitnessApp.Application.Commands.Nutrition;

namespace JacksFitnessApp.Application.Handlers.Nutrition;

public class AddMealItemHandler : IRequestHandler<AddMealItemCommand, MealItemDto>
{
    private readonly INutritionRepository _repository;
    private readonly IMapper _mapper;

    public AddMealItemHandler(INutritionRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<MealItemDto> Handle(AddMealItemCommand request, CancellationToken cancellationToken)
    {
        var item = new MealItem
        {
            MealId = request.MealId,
            FoodItemId = request.FoodItemId,
            QuantityGrams = request.QuantityGrams
        };

        var created = await _repository.AddMealItemAsync(item);
        return _mapper.Map<MealItemDto>(created);
    }
}
