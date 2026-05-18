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
    private readonly IFoodRepository _foodRepository;
    private readonly IMapper _mapper;

    public AddMealItemHandler(
        INutritionRepository repository,
        IFoodRepository foodRepository,
        IMapper mapper)
    {
        _repository = repository;
        _foodRepository = foodRepository;
        _mapper = mapper;
    }

    public async Task<MealItemDto> Handle(AddMealItemCommand request, CancellationToken cancellationToken)
    {
        var foodItem = await _foodRepository.GetByIdAsync(request.FoodItemId)
            ?? throw new KeyNotFoundException($"Food item {request.FoodItemId} not found");

        var mealItem = new MealItem
        {
            MealId = request.MealId,
            FoodItemId = request.FoodItemId,
            QuantityGrams = request.QuantityGrams
        };

        var created = await _repository.AddMealItemAsync(mealItem);
        return _mapper.Map<MealItemDto>(created);
    }
}
