import { ChangeDetectorRef, Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, Validators, ReactiveFormsModule } from '@angular/forms';
import { Router } from '@angular/router';
import { MatCardModule } from '@angular/material/card';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatToolbarModule } from '@angular/material/toolbar';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';
import { MatDividerModule } from '@angular/material/divider';
import { MatChipsModule } from '@angular/material/chips';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatSnackBarModule, MatSnackBar } from '@angular/material/snack-bar';
import { TrainingService } from '../../../core/services/training';
import {
  Exercise,
  ExerciseType,
  WorkoutSession,
  WorkoutSet,
  CardioSet,
  MuscleGroup
} from '../../../models/training.models';

@Component({
  selector: 'app-log-workout',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    MatCardModule,
    MatButtonModule,
    MatIconModule,
    MatToolbarModule,
    MatFormFieldModule,
    MatInputModule,
    MatSelectModule,
    MatDividerModule,
    MatChipsModule,
    MatProgressSpinnerModule,
    MatSnackBarModule
  ],
  templateUrl: './log-workout.html',
  styleUrl: './log-workout.scss'
})
export class LogWorkoutComponent implements OnInit {
  // Session state
  session: WorkoutSession | null = null;
  isCreatingSession = false;
  sessionForm: FormGroup;

  // Exercise selection
  exercises: Exercise[] = [];
  filteredExercises: Exercise[] = [];
  selectedExercise: Exercise | null = null;
  muscleGroups = Object.entries(MuscleGroup)
    .filter(([key]) => isNaN(Number(key)))
    .map(([key, value]) => ({ label: key, value }));

  // Set logging
  setForm: FormGroup;
  cardioSetForm: FormGroup;
  isLoggingSet = false;

  ExerciseType = ExerciseType;

  constructor(
    private fb: FormBuilder,
    private trainingService: TrainingService,
    public router: Router,
    private snackBar: MatSnackBar,
    private cdr: ChangeDetectorRef
  ) {
    this.sessionForm = this.fb.group({
      notes: [''],
      programmeDayId: [null]
    });

    this.setForm = this.fb.group({
      setNumber: [1, Validators.required],
      repsCompleted: ['', [Validators.required, Validators.min(1)]],
      weightKg: ['', [Validators.required, Validators.min(0)]],
      restSeconds: ['', [Validators.min(0)]],
      notes: ['']
    });

    this.cardioSetForm = this.fb.group({
      setNumber: [1, Validators.required],
      durationSeconds: ['', [Validators.min(0)]],
      distanceKm: ['', [Validators.min(0)]],
      caloriesBurned: ['', [Validators.min(0)]],
      avgHeartRate: ['', [Validators.min(0)]],
      maxHeartRate: ['', [Validators.min(0)]],
      notes: ['']
    });
  }

  ngOnInit(): void {
    this.trainingService.getExercises().subscribe({
      next: exercises => {
        this.exercises = exercises;
        this.filteredExercises = exercises;
        this.cdr.detectChanges();
      }
    });
  }

  startSession(): void {
    this.isCreatingSession = true;
    const session = {
      date: new Date().toISOString(),
      notes: this.sessionForm.value.notes,
      programmeDayId: this.sessionForm.value.programmeDayId
    };

    this.trainingService.createWorkoutSession(session).subscribe({
      next: created => {
        this.session = created;
        this.isCreatingSession = false;
        this.cdr.detectChanges();
        this.snackBar.open('Workout session started', 'OK', { duration: 2000 });
      },
      error: () => {
        this.isCreatingSession = false;
        this.cdr.detectChanges();
        this.snackBar.open('Failed to start session', 'OK', { duration: 3000 });
      }
    });
  }

  filterByMuscleGroup(muscleGroup: number | null): void {
    this.filteredExercises = muscleGroup !== null
      ? this.exercises.filter(e =>
          e.primaryMuscleGroup === muscleGroup ||
          e.secondaryMuscleGroup === muscleGroup)
      : this.exercises;
  }

  getMuscleGroupLabel(value: number | null): string {
  if (value === null) return 'Cardio';
  return this.muscleGroups.find(mg => mg.value === value)?.label ?? '';
}

  selectExercise(exercise: Exercise): void {
    this.selectedExercise = exercise;
    // Reset set number based on existing sets
    const existingSets = exercise.type === ExerciseType.Strength
      ? this.session?.sets.filter(s => s.exercise.id === exercise.id).length ?? 0
      : this.session?.cardioSets.filter(s => s.exercise.id === exercise.id).length ?? 0;

    if (exercise.type === ExerciseType.Strength) {
      this.setForm.patchValue({ setNumber: existingSets + 1 });
    } else {
      this.cardioSetForm.patchValue({ setNumber: existingSets + 1 });
    }
  }

  logSet(): void {
  if (!this.session || !this.selectedExercise) return;
  if (this.setForm.invalid) return;

  this.isLoggingSet = true;
  const formValue = this.setForm.value;

  const set = {
    exerciseId: this.selectedExercise.id,
    setNumber: Number(formValue.setNumber),
    repsCompleted: Number(formValue.repsCompleted),
    weightKg: Number(formValue.weightKg),
    restSeconds: formValue.restSeconds ? Number(formValue.restSeconds) : null,
    notes: formValue.notes ?? ''
  };

    this.trainingService.logWorkoutSet(this.session.id, set).subscribe({
      next: created => {
        this.session!.sets.push(created);
        this.setForm.patchValue({ setNumber: this.setForm.value.setNumber + 1 });
        this.isLoggingSet = false;
        this.cdr.detectChanges();

        const prMessage = created.isPersonalRecord ? ' New personal record!' : '';
        this.snackBar.open(`Set logged${prMessage}`, 'OK', { duration: 2000 });
      },
      error: () => {
        this.isLoggingSet = false;
        this.cdr.detectChanges();
        this.snackBar.open('Failed to log set', 'OK', { duration: 3000 });
      }
    });
  }

  logCardioSet(): void {
  if (!this.session || !this.selectedExercise) return;

  this.isLoggingSet = true;
  const formValue = this.cardioSetForm.value;

  const set = {
    exerciseId: this.selectedExercise.id,
    setNumber: Number(formValue.setNumber),
    durationSeconds: formValue.durationSeconds ? Number(formValue.durationSeconds) * 60 : null,
    distanceKm: formValue.distanceKm ? Number(formValue.distanceKm) : null,
    caloriesBurned: formValue.caloriesBurned ? Number(formValue.caloriesBurned) : null,
    avgHeartRate: formValue.avgHeartRate ? Number(formValue.avgHeartRate) : null,
    maxHeartRate: formValue.maxHeartRate ? Number(formValue.maxHeartRate) : null,
    notes: formValue.notes ?? ''
  };
    this.trainingService.logCardioSet(this.session.id, set).subscribe({
      next: created => {
        this.session!.cardioSets.push(created);
        this.cardioSetForm.patchValue({ setNumber: this.cardioSetForm.value.setNumber + 1 });
        this.isLoggingSet = false;
        this.cdr.detectChanges();
        this.snackBar.open('Cardio set logged', 'OK', { duration: 2000 });
      },
      error: () => {
        this.isLoggingSet = false;
        this.cdr.detectChanges();
        this.snackBar.open('Failed to log cardio set', 'OK', { duration: 3000 });
      }
    });
  }

  finishWorkout(): void {
    this.router.navigate(['/dashboard']);
  }

  getSetsForExercise(exerciseId: number): WorkoutSet[] {
    return this.session?.sets.filter(s => s.exercise.id === exerciseId) ?? [];
  }

  getCardioSetsForExercise(exerciseId: number): CardioSet[] {
    return this.session?.cardioSets.filter(s => s.exercise.id === exerciseId) ?? [];
  }

formatDuration(seconds: number | null): string {
  if (!seconds) return '-';
  const mins = Math.floor(seconds / 60);
  const secs = seconds % 60;
  return secs > 0 ? `${mins}m ${secs}s` : `${mins}m`;
}
}