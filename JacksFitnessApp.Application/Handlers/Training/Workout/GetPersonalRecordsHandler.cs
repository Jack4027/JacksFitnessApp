using AutoMapper;
using JacksFitnessApp.Application.DTOs.Training.PersonalRecord;
using JacksFitnessApp.Application.Queries.Training.Workout;
using JacksFitnessApp.Domain.Interfaces.Training;
using MediatR;

namespace JacksFitnessApp.Application.Handlers.Training.Workout;

public class GetPersonalRecordsHandler : IRequestHandler<GetPersonalRecordsQuery, IEnumerable<PersonalRecordDto>>
{
    private readonly IWorkoutRepository _repository;
    private readonly IMapper _mapper;

    public GetPersonalRecordsHandler(IWorkoutRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<PersonalRecordDto>> Handle(GetPersonalRecordsQuery request, CancellationToken cancellationToken)
    {
        var records = await _repository.GetPersonalRecordsAsync(request.UserId);
        return _mapper.Map<IEnumerable<PersonalRecordDto>>(records);
    }
}