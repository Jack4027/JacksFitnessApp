import { ChangeDetectorRef, Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, Validators, ReactiveFormsModule } from '@angular/forms';
import { Router, ActivatedRoute } from '@angular/router';
import { MatCardModule } from '@angular/material/card';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatToolbarModule } from '@angular/material/toolbar';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';
import { MatExpansionModule } from '@angular/material/expansion';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatSnackBarModule, MatSnackBar } from '@angular/material/snack-bar';
import { MatDividerModule } from '@angular/material/divider';
import { TrainingService } from '../../../core/services/training';
import {
  Programme,
  ProgrammeGoal,
  Exercise,
  ExerciseType
} from '../../../models/training.models';

@Component({
  selector: 'app-programme-builder',
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
    MatExpansionModule,
    MatProgressSpinnerModule,
    MatSnackBarModule,
    MatDividerModule
  ],
  templateUrl: './programme-builder.html',
  styleUrl: './programme-builder.scss'
})
export class ProgrammeBuilderComponent implements OnInit {
  programme: Programme | null = null;
  isLoading = false;
  isSaving = false;

  programmeForm: FormGroup;
  weekForm: FormGroup;
  dayForm: FormGroup;
  exerciseForm: FormGroup;

  exercises: Exercise[] = [];
  selectedWeekId: number | null = null;
  selectedDayId: number | null = null;
  pageTitle = 'New programme';

  goals = Object.entries(ProgrammeGoal)
    .filter(([key]) => isNaN(Number(key)))
    .map(([key, value]) => ({ label: key.replace(/([A-Z])/g, ' $1').trim(), value }));

  ExerciseType = ExerciseType;

  constructor(
    private fb: FormBuilder,
    private trainingService: TrainingService,
    public router: Router,
    private route: ActivatedRoute,
    private snackBar: MatSnackBar,
    private cdr: ChangeDetectorRef
  ) {
    this.programmeForm = this.fb.group({
      name: ['', Validators.required],
      description: [''],
      durationWeeks: [4, [Validators.required, Validators.min(1)]],
      goal: ['', Validators.required]
    });

    this.weekForm = this.fb.group({
      weekNumber: [1, Validators.required],
      notes: ['']
    });

    this.dayForm = this.fb.group({
      name: ['', Validators.required],
      orderIndex: [1, Validators.required]
    });

    this.exerciseForm = this.fb.group({
      exerciseId: ['', Validators.required],
      orderIndex: [1, Validators.required],
      targetSets: [3, [Validators.required, Validators.min(1)]],
      targetRepsMin: [8, [Validators.required, Validators.min(1)]],
      targetRepsMax: [12, [Validators.required, Validators.min(1)]],
      targetWeight: [null],
      notes: ['']
    });
  }

ngOnInit(): void {
  this.trainingService.getExercises().subscribe({
    next: exercises => {
      this.exercises = exercises;
      this.cdr.detectChanges();
    }
  });

  const id = this.route.snapshot.queryParams['id'];
  if (id) {
    this.isLoading = true;
    this.trainingService.getProgrammeById(+id).subscribe({
      next: programme => {
        this.programme = programme;
        this.programmeForm.patchValue(programme);
        this.pageTitle = 'Edit programme';
        this.isLoading = false;
        this.cdr.detectChanges();
      },
      error: () => {
        this.isLoading = false;
        this.cdr.detectChanges();
      }
    });
  }
}

  createProgramme(): void {
    if (this.programmeForm.invalid) return;
    this.isSaving = true;

    this.trainingService.createProgramme(this.programmeForm.value).subscribe({
      next: created => {
        this.programme = created;
        this.pageTitle = 'Edit programme';
        this.isSaving = false;
        this.cdr.detectChanges();
        this.snackBar.open('Programme created', 'OK', { duration: 2000 });
      },
      error: () => {
        this.isSaving = false;
        this.snackBar.open('Failed to create programme', 'OK', { duration: 3000 });
      }
    });
  }

  addWeek(): void {
    if (!this.programme || this.weekForm.invalid) return;

    this.trainingService.addProgrammeWeek(this.programme.id, this.weekForm.value).subscribe({
      next: week => {
        this.programme!.weeks.push({ ...week, days: [] });
        this.weekForm.patchValue({ weekNumber: this.programme!.weeks.length + 1 });
        this.snackBar.open('Week added', 'OK', { duration: 2000 });
      },
      error: () => this.snackBar.open('Failed to add week', 'OK', { duration: 3000 })
    });
  }

  addDay(weekId: number): void {
    if (this.dayForm.invalid) return;

    this.trainingService.addProgrammeDay(weekId, this.dayForm.value).subscribe({
      next: day => {
        const week = this.programme!.weeks.find(w => w.id === weekId);
        if (week) week.days.push({ ...day, plannedExercises: [] });
        this.dayForm.reset({ orderIndex: (week?.days.length ?? 0) + 1 });
        this.selectedWeekId = null;
        this.snackBar.open('Day added', 'OK', { duration: 2000 });
      },
      error: () => this.snackBar.open('Failed to add day', 'OK', { duration: 3000 })
    });
  }

  addExercise(dayId: number): void {
    if (this.exerciseForm.invalid) return;

    this.trainingService.addPlannedExercise(dayId, this.exerciseForm.value).subscribe({
      next: planned => {
        const day = this.programme!.weeks
          .flatMap(w => w.days)
          .find(d => d.id === dayId);
        if (day) day.plannedExercises.push(planned);
        this.selectedDayId = null;
        this.exerciseForm.reset({
          targetSets: 3,
          targetRepsMin: 8,
          targetRepsMax: 12,
          orderIndex: (day?.plannedExercises.length ?? 0) + 1
        });
        this.snackBar.open('Exercise added', 'OK', { duration: 2000 });
      },
      error: () => this.snackBar.open('Failed to add exercise', 'OK', { duration: 3000 })
    });
  }

  getExerciseName(id: number): string {
    return this.exercises.find(e => e.id === id)?.name ?? '';
  }
}