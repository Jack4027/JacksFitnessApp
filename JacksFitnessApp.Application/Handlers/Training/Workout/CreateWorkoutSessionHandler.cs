using AutoMapper;
using MediatR;
using JacksFitnessApp.Application.DTOs.Training.Workout;
using JacksFitnessApp.Domain.Interfaces.Training;
using JacksFitnessApp.Application.Commands.Training;

namespace JacksFitnessApp.Application.Handlers.Training.Workout;

public class CreateWorkoutSessionHandler : IRequestHandler<CreateWorkoutSessionCommand, WorkoutSessionDto>
{
    private readonly IWorkoutRepository _repository;
    private readonly IMapper _mapper;

    public CreateWorkoutSessionHandler(IWorkoutRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<WorkoutSessionDto> Handle(CreateWorkoutSessionCommand request, CancellationToken cancellationToken)
    {
        var session = new Domain.Entities.Training.WorkoutSession
        {
            Date = request.Date,
            DurationMinutes = request.DurationMinutes,
            Notes = request.Notes,
            UserId = request.UserId,
            ProgrammeDayId = request.ProgrammeDayId
        };

        var created = await _repository.AddAsync(session);
        return _mapper.Map<WorkoutSessionDto>(created);
    }
}
