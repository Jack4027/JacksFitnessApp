using JacksFitnessApp.Domain.Common;

using JacksFitnessApp.Domain.Enums.Training;

namespace JacksFitnessApp.Domain.Entities.Training
{
    public class Exercise : BaseEntity
    {
        // Private constructor forces use of factory methods
        private Exercise() { }

        // Factory method for seeded/global exercises
        public static Exercise CreateGlobal(
            string name,
            string description,
            ExerciseType type,
            MuscleGroup? primaryMuscleGroup,
            MuscleGroup? secondaryMuscleGroup,
            ExerciseCategory category,
            EquipmentType equipment)
        {
            return new Exercise
            {
                Name = name,
                Description = description,
                Type = type,
                PrimaryMuscleGroup = primaryMuscleGroup,
                SecondaryMuscleGroup = secondaryMuscleGroup,
                Category = category,
                Equipment = equipment,
                IsCustom = false,
                UserId = null
            };
        }

        // Factory method for user created custom exercises
        public static Exercise CreateCustom(
            string name,
            string description,
            ExerciseType type,
            MuscleGroup? primaryMuscleGroup,
            MuscleGroup? secondaryMuscleGroup,
            ExerciseCategory category,
            EquipmentType equipment,
            string userId)
        {
            return new Exercise
            {
                Name = name,
                Description = description,
                Type = type,
                PrimaryMuscleGroup = primaryMuscleGroup,
                SecondaryMuscleGroup = secondaryMuscleGroup,
                Category = category,
                Equipment = equipment,
                IsCustom = true,
                UserId = userId
            };
        }

        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public ExerciseType Type { get; set; }
        public MuscleGroup? PrimaryMuscleGroup { get; set; }
        public MuscleGroup? SecondaryMuscleGroup { get; set; }
        public ExerciseCategory Category { get; set; }
        public EquipmentType Equipment { get; set; }
        public bool IsCustom { get; private set; }
        public string? UserId { get; private set; }

        public ICollection<WorkoutSet> WorkoutSets { get; set; } = new List<WorkoutSet>();
        public ICollection<CardioSet> CardioSets { get; set; } = new List<CardioSet>();
        public ICollection<PlannedExercise> PlannedExercises { get; set; } = new List<PlannedExercise>();
    }
}