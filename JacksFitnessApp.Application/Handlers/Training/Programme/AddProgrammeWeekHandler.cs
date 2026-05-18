using AutoMapper;
using JacksFitnessApp.Application.Commands.Training;
using JacksFitnessApp.Application.DTOs.Training.Programme;
using JacksFitnessApp.Domain.Entities.Training;
using JacksFitnessApp.Domain.Interfaces.Training;
using MediatR;

namespace JacksFitnessApp.Application.Handlers.Training.Programme;

public class AddProgrammeWeekHandler : IRequestHandler<AddProgrammeWeekCommand, ProgrammeWeekDto>
{
    private readonly IProgrammeRepository _repository;
    private readonly IMapper _mapper;

    public AddProgrammeWeekHandler(IProgrammeRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<ProgrammeWeekDto> Handle(AddProgrammeWeekCommand request, CancellationToken cancellationToken)
    {
        var programme = await _repository.GetByIdWithDetailsAsync(request.ProgrammeId)
            ?? throw new KeyNotFoundException($"Programme {request.ProgrammeId} not found");

        var week = new ProgrammeWeek
        {
            ProgrammeId = programme.Id,
            WeekNumber = request.WeekNumber,
            Notes = request.Notes,
            Days = new List<ProgrammeDay>()
        };

        var created = await _repository.AddWeekAsync(week);
        return _mapper.Map<ProgrammeWeekDto>(created);
    }
}
