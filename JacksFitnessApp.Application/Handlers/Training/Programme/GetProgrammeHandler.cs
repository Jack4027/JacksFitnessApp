using AutoMapper;
using MediatR;
using JacksFitnessApp.Application.DTOs.Training.Programme;
using JacksFitnessApp.Application.Queries.Training.Programme;
using JacksFitnessApp.Domain.Interfaces.Training;

namespace JacksFitnessApp.Application.Handlers.Training.Programme;

public class GetProgrammesHandler : IRequestHandler<GetProgrammesQuery, IEnumerable<ProgrammeDto>>
{
    private readonly IProgrammeRepository _repository;
    private readonly IMapper _mapper;

    public GetProgrammesHandler(IProgrammeRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<ProgrammeDto>> Handle(GetProgrammesQuery request, CancellationToken cancellationToken)
    {
        var programmes = await _repository.GetByUserIdAsync(request.UserId);
        return _mapper.Map<IEnumerable<ProgrammeDto>>(programmes);
    }
}
