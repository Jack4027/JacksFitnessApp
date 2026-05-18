using AutoMapper;
using JacksFitnessApp.Application.Commands.Training;
using JacksFitnessApp.Application.DTOs.Training.Workout;
using JacksFitnessApp.Domain.Entities.Training;
using JacksFitnessApp.Domain.Interfaces.Training;
using MediatR;

namespace JacksFitnessApp.Application.Handlers.Training.Workout;

public class LogCardioSetHandler : IRequestHandler<LogCardioSetCommand, CardioSetDto>
{
    private readonly IWorkoutRepository _repository;
    private readonly IMapper _mapper;

    public LogCardioSetHandler(IWorkoutRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<CardioSetDto> Handle(LogCardioSetCommand request, CancellationToken cancellationToken)
    {
        var session = await _repository.GetByIdWithSetsAsync(request.WorkoutSessionId)
            ?? throw new KeyNotFoundException($"Workout session {request.WorkoutSessionId} not found");

        var set = new CardioSet
        {
            WorkoutSessionId = session.Id,
            ExerciseId = request.ExerciseId,
            SetNumber = request.SetNumber,
            DurationSeconds = request.DurationSeconds,
            DistanceKm = request.DistanceKm,
            CaloriesBurned = request.CaloriesBurned,
            AvgHeartRate = request.AvgHeartRate,
            MaxHeartRate = request.MaxHeartRate,
            Notes = request.Notes
        };

        var created = await _repository.AddCardioSetAsync(set);
        return _mapper.Map<CardioSetDto>(created);
    }
}
