using AutoMapper;
using MediatR;
using JacksFitnessApp.Application.DTOs.Training.Programme;
using JacksFitnessApp.Domain.Interfaces.Training;
using JacksFitnessApp.Application.Commands.Training;

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
        var week = new Domain.Entities.Training.ProgrammeWeek
        {
            ProgrammeId = request.ProgrammeId,
            WeekNumber = request.WeekNumber,
            Notes = request.Notes
        };

        var created = await _repository.AddWeekAsync(week);
        return _mapper.Map<ProgrammeWeekDto>(created);
    }
}
