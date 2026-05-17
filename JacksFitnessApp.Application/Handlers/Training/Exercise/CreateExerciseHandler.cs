using AutoMapper;
using MediatR;
using JacksFitnessApp.Application.DTOs.Training.Exercise;
using JacksFitnessApp.Domain.Interfaces.Training;
using JacksFitnessApp.Application.Commands.Training;
using DomainExercise = JacksFitnessApp.Domain.Entities.Training.Exercise;

namespace JacksFitnessApp.Application.Handlers.Training.Exercises;      

public class CreateExerciseHandler : IRequestHandler<CreateExerciseCommand, ExerciseDto>    
{
    private readonly IExerciseRepository _repository;
    private readonly IMapper _mapper;

    public CreateExerciseHandler(IExerciseRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<ExerciseDto> Handle(CreateExerciseCommand request, CancellationToken cancellationToken)
    {
        var exercise = request.UserId != null
            ? DomainExercise.CreateCustom(
                request.Name,
                request.Description,
                request.Type,
                request.PrimaryMuscleGroup,
                request.SecondaryMuscleGroup,
                request.Category,
                request.Equipment,
                request.UserId)
            : DomainExercise.CreateGlobal(
                request.Name,
                request.Description,
                request.Type,
                request.PrimaryMuscleGroup,
                request.SecondaryMuscleGroup,
                request.Category,
                request.Equipment);

        var created = await _repository.AddAsync(exercise);
        return _mapper.Map<ExerciseDto>(created);
    }
}
