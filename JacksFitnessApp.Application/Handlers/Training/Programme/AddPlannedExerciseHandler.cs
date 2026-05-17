using AutoMapper;
using MediatR;
using JacksFitnessApp.Application.DTOs.Training.Programme;
using JacksFitnessApp.Domain.Interfaces.Training;
using JacksFitnessApp.Application.Commands.Training;

namespace JacksFitnessApp.Application.Handlers.Training.Programme;

public class AddPlannedExerciseHandler : IRequestHandler<AddPlannedExerciseCommand, PlannedExerciseDto>
{
    private readonly IProgrammeRepository _repository;
    private readonly IMapper _mapper;

    public AddPlannedExerciseHandler(IProgrammeRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<PlannedExerciseDto> Handle(AddPlannedExerciseCommand request, CancellationToken cancellationToken)
    {
        var plannedExercise = new Domain.Entities.Training.PlannedExercise
        {
            ProgrammeDayId = request.ProgrammeDayId,
            ExerciseId = request.ExerciseId,
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