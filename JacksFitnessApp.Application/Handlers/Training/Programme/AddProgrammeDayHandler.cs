using AutoMapper;
using JacksFitnessApp.Application.Commands.Training;
using JacksFitnessApp.Application.DTOs.Training.Programme;
using JacksFitnessApp.Domain.Entities.Training;
using JacksFitnessApp.Domain.Interfaces.Training;
using MediatR;

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
        var week = await _repository.GetWeekByIdAsync(request.ProgrammeWeekId)
            ?? throw new KeyNotFoundException($"Programme week {request.ProgrammeWeekId} not found");

        var day = new ProgrammeDay
        {
            ProgrammeWeekId = week.Id,
            Name = request.Name,
            OrderIndex = request.OrderIndex
        };

        var created = await _repository.AddDayAsync(day);
        return _mapper.Map<ProgrammeDayDto>(created);
    }
}
