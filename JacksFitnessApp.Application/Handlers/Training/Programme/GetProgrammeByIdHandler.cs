using AutoMapper;
using MediatR;
using JacksFitnessApp.Application.DTOs.Training.Programme;
using JacksFitnessApp.Application.Queries.Training.Programme;
using JacksFitnessApp.Domain.Interfaces.Training;

namespace JacksFitnessApp.Application.Handlers.Training.Programme;

public class GetProgrammeByIdHandler : IRequestHandler<GetProgrammeByIdQuery, ProgrammeDto>
{
    private readonly IProgrammeRepository _repository;
    private readonly IMapper _mapper;

    public GetProgrammeByIdHandler(IProgrammeRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<ProgrammeDto> Handle(GetProgrammeByIdQuery request, CancellationToken cancellationToken)
    {
        var programme = await _repository.GetByIdWithDetailsAsync(request.Id)
            ?? throw new KeyNotFoundException($"Programme {request.Id} not found");

        if (programme.UserId != request.UserId)
            throw new UnauthorizedAccessException("You do not own this programme");

        return _mapper.Map<ProgrammeDto>(programme);
    }
}