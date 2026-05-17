using AutoMapper;
using MediatR;
using JacksFitnessApp.Application.DTOs.Training.Exercise;
using JacksFitnessApp.Application.Queries.Training.Exercise;
using JacksFitnessApp.Domain.Interfaces.Training;

namespace JacksFitnessApp.Application.Handlers.Training.Exercise;

public class GetExerciseByIdHandler : IRequestHandler<GetExerciseByIdQuery, ExerciseDto>
{
    private readonly IExerciseRepository _repository;
    private readonly IMapper _mapper;

    public GetExerciseByIdHandler(IExerciseRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<ExerciseDto> Handle(GetExerciseByIdQuery request, CancellationToken cancellationToken)
    {
        var exercise = await _repository.GetByIdAsync(request.Id)
    ?? throw new KeyNotFoundException($"Exercise {request.Id} not found");

        return _mapper.Map<ExerciseDto>(exercise);
    }
}