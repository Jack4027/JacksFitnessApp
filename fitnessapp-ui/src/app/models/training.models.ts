export interface Exercise {
  id: number;
  name: string;
  description: string;
  type: ExerciseType;
  primaryMuscleGroup: MuscleGroup | null;
  secondaryMuscleGroup: MuscleGroup | null;
  category: ExerciseCategory;
  equipment: EquipmentType;
  isCustom: boolean;
}

export interface WorkoutSession {
  id: number;
  date: string;
  durationMinutes: number | null;
  notes: string;
  programmeDayId: number | null;
  programmeDayName: string | null;
  sets: WorkoutSet[];
  cardioSets: CardioSet[];
}

export interface WorkoutSet {
  id: number;
  setNumber: number;
  repsCompleted: number;
  weightKg: number;
  restSeconds: number | null;
  isPersonalRecord: boolean;
  notes: string;
  exercise: Exercise;
}

export interface CardioSet {
  id: number;
  setNumber: number;
  durationSeconds: number | null;
  distanceKm: number | null;
  caloriesBurned: number | null;
  avgHeartRate: number | null;
  maxHeartRate: number | null;
  notes: string;
  exercise: Exercise;
}

export interface PersonalRecord {
  exerciseId: number;
  exerciseName: string;
  weightKg: number;
  reps: number;
  achievedOn: string;
}

export interface Programme {
  id: number;
  name: string;
  description: string;
  durationWeeks: number;
  goal: ProgrammeGoal;
  isActive: boolean;
  weeks: ProgrammeWeek[];
}

export interface ProgrammeWeek {
  id: number;
  weekNumber: number;
  notes: string;
  days: ProgrammeDay[];
}

export interface ProgrammeDay {
  id: number;
  name: string;
  dayOfWeek: number | null;
  orderIndex: number;
  plannedExercises: PlannedExercise[];
}

export interface PlannedExercise {
  id: number;
  orderIndex: number;
  targetSets: number;
  targetRepsMin: number;
  targetRepsMax: number;
  targetWeight: number | null;
  notes: string;
  exercise: Exercise;
}

export enum ExerciseType {
  Strength = 0,
  Cardio = 1
}

export enum MuscleGroup {
  Chest = 0,
  Back = 1,
  Shoulders = 2,
  Biceps = 3,
  Triceps = 4,
  Forearms = 5,
  Quadriceps = 6,
  Hamstrings = 7,
  Glutes = 8,
  Calves = 9,
  Abs = 10,
  FullBody = 11
}

export enum ExerciseCategory {
  Compound = 0,
  Isolation = 1,
  Cardio = 2,
  Stretching = 3
}

export enum EquipmentType {
  Barbell = 0,
  Dumbbell = 1,
  Cable = 2,
  Machine = 3,
  Bodyweight = 4,
  Kettlebell = 5,
  Bands = 6,
  Other = 7
}

export enum ProgrammeGoal {
  Strength = 0,
  Hypertrophy = 1,
  Endurance = 2,
  WeightLoss = 3,
  GeneralFitness = 4
}