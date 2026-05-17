using JacksFitnessApp.Domain.Enums.Training;

namespace JacksFitnessApp.Application.DTOs.Training.Exercise;

public class ExerciseDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public ExerciseType Type { get; set; }
    public MuscleGroup? PrimaryMuscleGroup { get; set; }
    public MuscleGroup? SecondaryMuscleGroup { get; set; }
    public ExerciseCategory Category { get; set; }
    public EquipmentType Equipment { get; set; }
    public bool IsCustom { get; set; }
}