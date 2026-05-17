using AutoMapper;
using MediatR;
using JacksFitnessApp.Application.DTOs.Nutrition;
using JacksFitnessApp.Application.Queries.Nutrition;
using JacksFitnessApp.Domain.Interfaces.Nutrition;

namespace JacksFitnessApp.Application.Handlers.Nutrition;

public class GetNutritionLogByDateHandler : IRequestHandler<GetNutritionLogByDateQuery, NutritionLogDto>
{
    private readonly INutritionRepository _repository;
    private readonly IMapper _mapper;

    public GetNutritionLogByDateHandler(INutritionRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<NutritionLogDto?> Handle(GetNutritionLogByDateQuery request, CancellationToken cancellationToken)
    {
        var log = await _repository.GetByDateAsync(request.UserId, request.Date);

        if (log == null) return null;

        return _mapper.Map<NutritionLogDto>(log);
    }
}
