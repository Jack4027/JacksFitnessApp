using AutoMapper;
using MediatR;
using JacksFitnessApp.Application.DTOs.Training.Exercise;
using JacksFitnessApp.Domain.Interfaces.Training;
using JacksFitnessApp.Application.Commands.Training;

namespace JacksFitnessApp.Application.Handlers.Training.Exercise;

public class UpdateExerciseHandler : IRequestHandler<UpdateExerciseCommand, ExerciseDto>
{
    private readonly IExerciseRepository _repository;
    private readonly IMapper _mapper;

    public UpdateExerciseHandler(IExerciseRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<ExerciseDto> Handle(UpdateExerciseCommand request, CancellationToken cancellationToken)
    {
        var exercise = await _repository.GetByIdAsync(request.Id)
            ?? throw new KeyNotFoundException($"Exercise {request.Id} not found");

        exercise.Name = request.Name;
        exercise.Description = request.Description;
        exercise.PrimaryMuscleGroup = request.PrimaryMuscleGroup;
        exercise.SecondaryMuscleGroup = request.SecondaryMuscleGroup;
        exercise.Category = request.Category;
        exercise.Equipment = request.Equipment;
        exercise.UpdatedAt = DateTime.UtcNow;

        await _repository.UpdateAsync(exercise);
        return _mapper.Map<ExerciseDto>(exercise);
    }
}
