using JacksFitnessApp.Application.DTOs.Training.Exercise;

namespace JacksFitnessApp.Application.DTOs.Training.Workout;

public class CardioSetDto
{
    public int Id { get; set; }
    public int SetNumber { get; set; }
    public int? DurationSeconds { get; set; }
    public decimal? DistanceKm { get; set; }
    public int? CaloriesBurned { get; set; }
    public int? AvgHeartRate { get; set; }
    public int? MaxHeartRate { get; set; }
    public string Notes { get; set; } = string.Empty;
    public ExerciseDto Exercise { get; set; } = null!;
}