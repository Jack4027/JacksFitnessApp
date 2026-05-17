using AutoMapper;
using MediatR;
using JacksFitnessApp.Application.DTOs.Training.Programme;
using JacksFitnessApp.Domain.Interfaces.Training;
using JacksFitnessApp.Application.Commands.Training;

namespace JacksFitnessApp.Application.Handlers.Training.Programme;

public class AddProgrammeDayHandler : IRequestHandler<AddProgrammeDayCommand, ProgrammeDayDto>
{
    private readonly IProgrammeRepository _repository;
    private readonly IMapper _mapper;

    public AddProgrammeDayHandler(IProgrammeRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<ProgrammeDayDto> Handle(AddProgrammeDayCommand request, CancellationToken cancellationToken)
    {
        var day = new Domain.Entities.Training.ProgrammeDay
        {
            ProgrammeWeekId = request.ProgrammeWeekId,
            Name = request.Name,
            DayOfWeek = request.DayOfWeek,
            OrderIndex = request.OrderIndex
        };

        var created = await _repository.AddDayAsync(day);
        return _mapper.Map<ProgrammeDayDto>(created);
    }
}
