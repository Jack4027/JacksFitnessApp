using AutoMapper;
using MediatR;
using JacksFitnessApp.Application.DTOs.Training.Programme;
using JacksFitnessApp.Domain.Interfaces.Training;
using JacksFitnessApp.Application.Commands.Training;

namespace JacksFitnessApp.Application.Handlers.Training.Programme;

public class CreateProgrammeHandler : IRequestHandler<CreateProgrammeCommand, ProgrammeDto>
{
    private readonly IProgrammeRepository _repository;
    private readonly IMapper _mapper;

    public CreateProgrammeHandler(IProgrammeRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<ProgrammeDto> Handle(CreateProgrammeCommand request, CancellationToken cancellationToken)
    {
        var programme = new Domain.Entities.Training.Programme
        {
            Name = request.Name,
            Description = request.Description,
            DurationWeeks = request.DurationWeeks,
            Goal = request.Goal,
            UserId = request.UserId
        };

        var created = await _repository.AddAsync(programme);
        return _mapper.Map<ProgrammeDto>(created);
    }
}
