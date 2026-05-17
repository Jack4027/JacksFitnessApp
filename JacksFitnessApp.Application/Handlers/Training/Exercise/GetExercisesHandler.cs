using AutoMapper;
using MediatR;
using JacksFitnessApp.Application.DTOs.Training.Exercise;
using JacksFitnessApp.Application.Queries.Training.Exercise;
using JacksFitnessApp.Domain.Interfaces.Training;

namespace JacksFitnessApp.Application.Handlers.Training.Exercise;

public class GetExercisesHandler : IRequestHandler<GetExercisesQuery, IEnumerable<ExerciseDto>>
{
    private readonly IExerciseRepository _repository;
    private readonly IMapper _mapper;

    public GetExercisesHandler(IExerciseRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<ExerciseDto>> Handle(GetExercisesQuery request, CancellationToken cancellationToken)
    {
        var exercises = request.MuscleGroup.HasValue
            ? await _repository.GetByMuscleGroupAsync(request.MuscleGroup.Value)
            : await _repository.GetAllAsync();

        return _mapper.Map<IEnumerable<ExerciseDto>>(exercises);
    }
}
