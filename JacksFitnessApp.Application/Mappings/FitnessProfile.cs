using AutoMapper;
using JacksFitnessApp.Application.DTOs.Metrics;
using JacksFitnessApp.Application.DTOs.Nutrition;
using JacksFitnessApp.Application.DTOs.Training.Exercise;
using JacksFitnessApp.Application.DTOs.Training.PersonalRecord;
using JacksFitnessApp.Application.DTOs.Training.Programme;
using JacksFitnessApp.Application.DTOs.Training.Workout;
using JacksFitnessApp.Domain.Entities.Metrics;
using JacksFitnessApp.Domain.Entities.Nutrition;
using JacksFitnessApp.Domain.Entities.Training;

namespace JacksFitnessApp.Application.Mappings;

public class FitnessProfile : Profile
{
    public FitnessProfile()
    {
        // Training
        CreateMap<Exercise, ExerciseDto>();
        CreateMap<Programme, ProgrammeDto>();
        CreateMap<ProgrammeWeek, ProgrammeWeekDto>();
        CreateMap<ProgrammeDay, ProgrammeDayDto>();
        CreateMap<PlannedExercise, PlannedExerciseDto>();
        CreateMap<WorkoutSession, WorkoutSessionDto>()
            .ForMember(dest => dest.ProgrammeDayName,
                opt => opt.MapFrom(src => src.ProgrammeDay != null ? src.ProgrammeDay.Name : null));
        CreateMap<WorkoutSet, WorkoutSetDto>();
        CreateMap<CardioSet, CardioSetDto>();
        CreateMap<WorkoutSet, PersonalRecordDto>()
            .ForMember(dest => dest.ExerciseName,
                opt => opt.MapFrom(src => src.Exercise.Name))
            .ForMember(dest => dest.WeightKg,
                opt => opt.MapFrom(src => src.WeightKg))
            .ForMember(dest => dest.Reps,
                opt => opt.MapFrom(src => src.RepsCompleted))
            .ForMember(dest => dest.AchievedOn,
                opt => opt.MapFrom(src => src.WorkoutSession.Date));

        // Nutrition
        CreateMap<NutritionLog, NutritionLogDto>()
            .ForMember(dest => dest.TotalCalories,
                opt => opt.MapFrom(src => src.TotalCalories))
            .ForMember(dest => dest.TotalProtein,
                opt => opt.MapFrom(src => src.TotalProtein))
            .ForMember(dest => dest.TotalCarbs,
                opt => opt.MapFrom(src => src.TotalCarbs))
            .ForMember(dest => dest.TotalFat,
                opt => opt.MapFrom(src => src.TotalFat));
        CreateMap<Meal, MealDto>()
            .ForMember(dest => dest.TotalCalories,
                opt => opt.MapFrom(src => src.TotalCalories))
            .ForMember(dest => dest.TotalProtein,
                opt => opt.MapFrom(src => src.TotalProtein))
            .ForMember(dest => dest.TotalCarbs,
                opt => opt.MapFrom(src => src.TotalCarbs))
            .ForMember(dest => dest.TotalFat,
                opt => opt.MapFrom(src => src.TotalFat));
        CreateMap<MealItem, MealItemDto>()
            .ForMember(dest => dest.Calories,
                opt => opt.MapFrom(src => src.Calories))
            .ForMember(dest => dest.Protein,
                opt => opt.MapFrom(src => src.Protein))
            .ForMember(dest => dest.Carbs,
                opt => opt.MapFrom(src => src.Carbs))
            .ForMember(dest => dest.Fat,
                opt => opt.MapFrom(src => src.Fat));
        CreateMap<FoodItem, FoodItemDto>();

        // Metrics
        CreateMap<BodyMetric, BodyMetricDto>();
    }
}