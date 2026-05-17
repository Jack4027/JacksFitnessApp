using JacksFitnessApp.Domain.Entities.Training;
using JacksFitnessApp.Domain.Enums.Training;

namespace JacksFitnessApp.Infrastructure.Data.Seed;

public static class ExerciseSeed
{
    public static IEnumerable<Exercise> GetExercises()
    {
        return new List<Exercise>
        {
            // Chest
            Exercise.CreateGlobal("Bench Press", "Barbell bench press", ExerciseType.Strength, MuscleGroup.Chest, MuscleGroup.Triceps, ExerciseCategory.Compound, EquipmentType.Barbell),
            Exercise.CreateGlobal("Incline Bench Press", "Incline barbell press", ExerciseType.Strength, MuscleGroup.Chest, MuscleGroup.Shoulders, ExerciseCategory.Compound, EquipmentType.Barbell),
            Exercise.CreateGlobal("Dumbbell Fly", "Dumbbell chest fly", ExerciseType.Strength, MuscleGroup.Chest, null, ExerciseCategory.Isolation, EquipmentType.Dumbbell),
            Exercise.CreateGlobal("Push Up", "Bodyweight push up", ExerciseType.Strength, MuscleGroup.Chest, MuscleGroup.Triceps, ExerciseCategory.Compound, EquipmentType.Bodyweight),

            // Back
            Exercise.CreateGlobal("Deadlift", "Conventional deadlift", ExerciseType.Strength, MuscleGroup.Back, MuscleGroup.Hamstrings, ExerciseCategory.Compound, EquipmentType.Barbell),
            Exercise.CreateGlobal("Pull Up", "Bodyweight pull up", ExerciseType.Strength, MuscleGroup.Back, MuscleGroup.Biceps, ExerciseCategory.Compound, EquipmentType.Bodyweight),
            Exercise.CreateGlobal("Barbell Row", "Bent over barbell row", ExerciseType.Strength, MuscleGroup.Back, MuscleGroup.Biceps, ExerciseCategory.Compound, EquipmentType.Barbell),
            Exercise.CreateGlobal("Lat Pulldown", "Cable lat pulldown", ExerciseType.Strength, MuscleGroup.Back, MuscleGroup.Biceps, ExerciseCategory.Compound, EquipmentType.Cable),

            // Shoulders
            Exercise.CreateGlobal("Overhead Press", "Barbell overhead press", ExerciseType.Strength, MuscleGroup.Shoulders, MuscleGroup.Triceps, ExerciseCategory.Compound, EquipmentType.Barbell),
            Exercise.CreateGlobal("Lateral Raise", "Dumbbell lateral raise", ExerciseType.Strength, MuscleGroup.Shoulders, null, ExerciseCategory.Isolation, EquipmentType.Dumbbell),
            Exercise.CreateGlobal("Face Pull", "Cable face pull", ExerciseType.Strength, MuscleGroup.Shoulders, null, ExerciseCategory.Isolation, EquipmentType.Cable),

            // Legs
            Exercise.CreateGlobal("Squat", "Barbell back squat", ExerciseType.Strength, MuscleGroup.Quadriceps, MuscleGroup.Glutes, ExerciseCategory.Compound, EquipmentType.Barbell),
            Exercise.CreateGlobal("Romanian Deadlift", "Romanian deadlift", ExerciseType.Strength, MuscleGroup.Hamstrings, MuscleGroup.Glutes, ExerciseCategory.Compound, EquipmentType.Barbell),
            Exercise.CreateGlobal("Leg Press", "Machine leg press", ExerciseType.Strength, MuscleGroup.Quadriceps, MuscleGroup.Glutes, ExerciseCategory.Compound, EquipmentType.Machine),
            Exercise.CreateGlobal("Leg Curl", "Machine leg curl", ExerciseType.Strength, MuscleGroup.Hamstrings, null, ExerciseCategory.Isolation, EquipmentType.Machine),
            Exercise.CreateGlobal("Calf Raise", "Standing calf raise", ExerciseType.Strength, MuscleGroup.Calves, null, ExerciseCategory.Isolation, EquipmentType.Machine),
            Exercise.CreateGlobal("Lunges", "Dumbbell lunges", ExerciseType.Strength, MuscleGroup.Quadriceps, MuscleGroup.Glutes, ExerciseCategory.Compound, EquipmentType.Dumbbell),

            // Arms
            Exercise.CreateGlobal("Barbell Curl", "Barbell bicep curl", ExerciseType.Strength, MuscleGroup.Biceps, null, ExerciseCategory.Isolation, EquipmentType.Barbell),
            Exercise.CreateGlobal("Dumbbell Curl", "Dumbbell bicep curl", ExerciseType.Strength, MuscleGroup.Biceps, null, ExerciseCategory.Isolation, EquipmentType.Dumbbell),
            Exercise.CreateGlobal("Tricep Pushdown", "Cable tricep pushdown", ExerciseType.Strength, MuscleGroup.Triceps, null, ExerciseCategory.Isolation, EquipmentType.Cable),
            Exercise.CreateGlobal("Skull Crusher", "EZ bar skull crusher", ExerciseType.Strength, MuscleGroup.Triceps, null, ExerciseCategory.Isolation, EquipmentType.Barbell),

            // Core
            Exercise.CreateGlobal("Plank", "Bodyweight plank", ExerciseType.Strength, MuscleGroup.Abs, null, ExerciseCategory.Compound, EquipmentType.Bodyweight),
            Exercise.CreateGlobal("Crunch", "Bodyweight crunch", ExerciseType.Strength, MuscleGroup.Abs, null, ExerciseCategory.Isolation, EquipmentType.Bodyweight),

            // Cardio
            Exercise.CreateGlobal("Treadmill", "Treadmill running", ExerciseType.Cardio, null, null, ExerciseCategory.Cardio, EquipmentType.Machine),
            Exercise.CreateGlobal("Cycling", "Stationary bike", ExerciseType.Cardio, null, null, ExerciseCategory.Cardio, EquipmentType.Machine),
            Exercise.CreateGlobal("Rowing Machine", "Rowing ergometer", ExerciseType.Cardio, null, null, ExerciseCategory.Cardio, EquipmentType.Machine),
            Exercise.CreateGlobal("Jump Rope", "Skipping rope", ExerciseType.Cardio, null, null, ExerciseCategory.Cardio, EquipmentType.Other),
        };
    }
}