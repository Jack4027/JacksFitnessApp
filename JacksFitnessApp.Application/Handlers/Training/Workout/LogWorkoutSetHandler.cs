using AutoMapper;
using MediatR;
using JacksFitnessApp.Application.DTOs.Training.Workout;
using JacksFitnessApp.Domain.Interfaces.Training;
using JacksFitnessApp.Application.Commands.Training;

namespace JacksFitnessApp.Application.Handlers.Training.Workout;

public class LogWorkoutSetHandler : IRequestHandler<LogWorkoutSetCommand, WorkoutSetDto>
{
    private readonly IWorkoutRepository _repository;
    private readonly IMapper _mapper;

    public LogWorkoutSetHandler(IWorkoutRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<WorkoutSetDto> Handle(LogWorkoutSetCommand request, CancellationToken cancellationToken)
    {
        // Check if this is a personal record before saving
        var currentPR = await _repository.GetPersonalRecordForExerciseAsync(
            (await _repository.GetByIdWithSetsAsync(request.WorkoutSessionId))!.UserId,
            request.ExerciseId);

        var isPersonalRecord = currentPR == null || request.WeightKg > currentPR.WeightKg;

        var set = new Domain.Entities.Training.WorkoutSet
        {
            WorkoutSessionId = request.WorkoutSessionId,
            ExerciseId = request.ExerciseId,
            SetNumber = request.SetNumber,
            RepsCompleted = request.RepsCompleted,
            WeightKg = request.WeightKg,
            RestSeconds = request.RestSeconds,
            Notes = request.Notes,
            IsPersonalRecord = isPersonalRecord
        };

        var created = await _repository.AddSetAsync(set);
        return _mapper.Map<WorkoutSetDto>(created);
    }
}
