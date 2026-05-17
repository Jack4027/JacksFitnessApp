using AutoMapper;
using JacksFitnessApp.Application.DTOs.Training.Workout;
using JacksFitnessApp.Application.Queries.Training.Workout;
using JacksFitnessApp.Domain.Interfaces.Training;
using MediatR;

namespace JacksFitnessApp.Application.Handlers.Training.Workout;

public class GetWorkoutsHandler : IRequestHandler<GetWorkoutsQuery, IEnumerable<WorkoutSessionDto>>
{
    private readonly IWorkoutRepository _repository;
    private readonly IMapper _mapper;

    public GetWorkoutsHandler(IWorkoutRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<WorkoutSessionDto>> Handle(GetWorkoutsQuery request, CancellationToken cancellationToken)
    {
        var sessions = await _repository.GetByUserIdAsync(request.UserId);
        return _mapper.Map<IEnumerable<WorkoutSessionDto>>(sessions);
    }
}
