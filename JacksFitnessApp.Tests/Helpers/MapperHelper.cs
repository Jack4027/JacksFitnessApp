using AutoMapper;
using JacksFitnessApp.Application.Mappings;
using Microsoft.Extensions.Logging;

namespace JacksFitnessApp.Tests.Helpers;

public static class MapperHelper
{
    public static IMapper CreateMapper()
    {
        var config = new MapperConfiguration(
            cfg => cfg.AddProfile<FitnessProfile>(),
            new LoggerFactory()
        );
        return config.CreateMapper();
    }
}