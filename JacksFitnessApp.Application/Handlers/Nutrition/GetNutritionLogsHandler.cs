using AutoMapper;
using MediatR;
using JacksFitnessApp.Application.DTOs.Nutrition;
using JacksFitnessApp.Application.Queries.Nutrition;
using JacksFitnessApp.Domain.Interfaces.Nutrition;

namespace JacksFitnessApp.Application.Handlers.Nutrition;

public class GetNutritionLogsHandler : IRequestHandler<GetNutritionLogsQuery, IEnumerable<NutritionLogDto>>
{
    private readonly INutritionRepository _repository;
    private readonly IMapper _mapper;

    public GetNutritionLogsHandler(INutritionRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<NutritionLogDto>> Handle(GetNutritionLogsQuery request, CancellationToken cancellationToken)
    {
        var logs = await _repository.GetByUserIdAsync(request.UserId);
        return _mapper.Map<IEnumerable<NutritionLogDto>>(logs);
    }
}
