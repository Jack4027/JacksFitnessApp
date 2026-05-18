using AutoMapper;
using JacksFitnessApp.Application.Commands.Training;
using JacksFitnessApp.Application.DTOs.Training.Programme;
using JacksFitnessApp.Domain.Entities.Training;
using JacksFitnessApp.Domain.Interfaces.Training;
using MediatR;

namespace JacksFitnessApp.Application.Handlers.Training.Programme;

public class AddPlannedExerciseHandler : IRequestHandler<AddPlannedExerciseCommand, PlannedExerciseDto>
{
    private readonly IProgrammeRepository _repository;
    private readonly IExerciseRepository _exerciseRepository;
    private readonly IMapper _mapper;

    public AddPlannedExerciseHandler(
        IProgrammeRepository repository,
        IExerciseRepository exerciseRepository,
        IMapper mapper)
    {
        _repository = repository;
        _exerciseRepository = exerciseRepository;
        _mapper = mapper;
    }

    public async Task<PlannedExerciseDto> Handle(AddPlannedExerciseCommand request, CancellationToken cancellationToken)
    {
        var day = await _repository.GetDayByIdAsync(request.ProgrammeDayId)
            ?? throw new KeyNotFoundException($"Programme day {request.ProgrammeDayId} not found");

        var exercise = await _exerciseRepository.GetByIdAsync(request.ExerciseId)
            ?? throw new KeyNotFoundException($"Exercise {request.ExerciseId} not found");

        var plannedExercise = new PlannedExercise
        {
            ProgrammeDayId = day.Id,
            ExerciseId = exercise.Id,
            OrderIndex = request.OrderIndex,
            TargetSets = request.TargetSets,
            TargetRepsMin = request.TargetRepsMin,
            TargetRepsMax = request.TargetRepsMax,
            TargetWeight = request.TargetWeight,
            Notes = request.Notes
        };

        var created = await _repository.AddPlannedExerciseAsync(plannedExercise);
        return _mapper.Map<PlannedExerciseDto>(created);
    }
}