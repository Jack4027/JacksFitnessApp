using JacksFitnessApp.Domain.Common;
using JacksFitnessApp.Domain.Enums.Training;
using System;
using System.Collections.Generic;
using System.Text;

namespace JacksFitnessApp.Domain.Entities.Training
{
    public class CardioSet : BaseEntity
    {
        public int SetNumber { get; set; }
        public int? DurationSeconds { get; set; }
        public decimal? DistanceKm { get; set; }
        public int? CaloriesBurned { get; set; }
        public int? AvgHeartRate { get; set; }
        public int? MaxHeartRate { get; set; }
        public CardioIntensity Intensity { get; set; } = CardioIntensity.Moderate;
        public string Notes { get; set; } = string.Empty;
        public int WorkoutSessionId { get; set; }
        public int ExerciseId { get; set; }

        public WorkoutSession WorkoutSession { get; set; } = null!;
        public Exercise Exercise { get; set; } = null!;
    }
}
