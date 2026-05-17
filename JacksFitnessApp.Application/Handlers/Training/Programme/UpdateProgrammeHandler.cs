using AutoMapper;
using MediatR;
using JacksFitnessApp.Application.DTOs.Training.Programme;
using JacksFitnessApp.Domain.Interfaces.Training;
using JacksFitnessApp.Application.Commands.Training;

namespace JacksFitnessApp.Application.Handlers.Training.Programme;

public class UpdateProgrammeHandler : IRequestHandler<UpdateProgrammeCommand, ProgrammeDto>
{
    private readonly IProgrammeRepository _repository;
    private readonly IMapper _mapper;

    public UpdateProgrammeHandler(IProgrammeRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<ProgrammeDto> Handle(UpdateProgrammeCommand request, CancellationToken cancellationToken)
    {
        var programme = await _repository.GetByIdWithDetailsAsync(request.Id)
            ?? throw new KeyNotFoundException($"Programme {request.Id} not found");

        if (programme.UserId != request.UserId)
            throw new UnauthorizedAccessException("You do not own this programme");

        programme.Name = request.Name;
        programme.Description = request.Description;
        programme.DurationWeeks = request.DurationWeeks;
        programme.Goal = request.Goal;
        programme.IsActive = request.IsActive;
        programme.UpdatedAt = DateTime.UtcNow;

        await _repository.UpdateAsync(programme);
        return _mapper.Map<ProgrammeDto>(programme);
    }
}
