using AutoMapper;
using MediatR;
using JacksFitnessApp.Application.DTOs.Nutrition;
using JacksFitnessApp.Domain.Entities.Nutrition;
using JacksFitnessApp.Domain.Interfaces.Nutrition;
using JacksFitnessApp.Application.Commands.Nutrition;

namespace JacksFitnessApp.Application.Handlers.Nutrition;

public class AddMealHandler : IRequestHandler<AddMealCommand, MealDto>
{
    private readonly INutritionRepository _repository;
    private readonly IMapper _mapper;

    public AddMealHandler(INutritionRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<MealDto> Handle(AddMealCommand request, CancellationToken cancellationToken)
    {
        var meal = new Meal
        {
            NutritionLogId = request.NutritionLogId,
            Name = request.Name,
            Type = request.Type
        };

        var created = await _repository.AddMealAsync(meal);
        return _mapper.Map<MealDto>(created);
    }
}
