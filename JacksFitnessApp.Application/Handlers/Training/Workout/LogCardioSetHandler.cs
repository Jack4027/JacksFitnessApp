using AutoMapper;
using MediatR;
using JacksFitnessApp.Application.DTOs.Training.Workout;
using JacksFitnessApp.Domain.Interfaces.Training;
using JacksFitnessApp.Application.Commands.Training;

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
        var set = new Domain.Entities.Training.CardioSet
        {
            WorkoutSessionId = request.WorkoutSessionId,
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
