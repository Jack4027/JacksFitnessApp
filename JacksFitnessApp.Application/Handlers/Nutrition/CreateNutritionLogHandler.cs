using AutoMapper;
using MediatR;
using JacksFitnessApp.Application.Commands.Nutrition;
using JacksFitnessApp.Application.DTOs.Nutrition;
using JacksFitnessApp.Domain.Entities.Nutrition;
using JacksFitnessApp.Domain.Interfaces.Nutrition;

namespace JacksFitnessApp.Application.Handlers.Nutrition;

public class CreateNutritionLogHandler : IRequestHandler<CreateNutritionLogCommand, NutritionLogDto>
{
    private readonly INutritionRepository _repository;
    private readonly IMapper _mapper;

    public CreateNutritionLogHandler(INutritionRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<NutritionLogDto> Handle(CreateNutritionLogCommand request, CancellationToken cancellationToken)
    {
        var log = new NutritionLog
        {
            UserId = request.UserId,
            Date = request.Date,
            Notes = request.Notes
        };

        var created = await _repository.AddAsync(log);
        return _mapper.Map<NutritionLogDto>(created);
    }
}
