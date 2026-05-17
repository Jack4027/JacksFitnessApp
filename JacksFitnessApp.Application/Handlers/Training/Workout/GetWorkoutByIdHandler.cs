using AutoMapper;
using JacksFitnessApp.Application.DTOs.Training.Workout;
using JacksFitnessApp.Application.Queries.Training.Workout;
using JacksFitnessApp.Domain.Interfaces.Training;
using MediatR;

namespace JacksFitnessApp.Application.Handlers.Training.Workout;

public class GetWorkoutByIdHandler : IRequestHandler<GetWorkoutByIdQuery, WorkoutSessionDto>
{
    private readonly IWorkoutRepository _repository;
    private readonly IMapper _mapper;

    public GetWorkoutByIdHandler(IWorkoutRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<WorkoutSessionDto> Handle(GetWorkoutByIdQuery request, CancellationToken cancellationToken)
    {
        var session = await _repository.GetByIdWithSetsAsync(request.Id)
            ?? throw new KeyNotFoundException($"Workout session {request.Id} not found");

        if (session.UserId != request.UserId)
            throw new UnauthorizedAccessException("You do not own this workout session");

        return _mapper.Map<WorkoutSessionDto>(session);
    }
}
