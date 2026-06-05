import { Component, OnInit, ChangeDetectorRef } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';
import { MatCardModule } from '@angular/material/card';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatExpansionModule } from '@angular/material/expansion';
import { MatDividerModule } from '@angular/material/divider';
import { TrainingService } from '../../../core/services/training';
import { WorkoutSession, ExerciseType, MuscleGroup } from '../../../models/training.models';

@Component({
  selector: 'app-workout-history',
  standalone: true,
  imports: [
    CommonModule,
    MatCardModule,
    MatButtonModule,
    MatIconModule,
    MatProgressSpinnerModule,
    MatExpansionModule,
    MatDividerModule
  ],
  templateUrl: './workout-history.html',
  styleUrl: './workout-history.scss'
})
export class WorkoutHistoryComponent implements OnInit {
  workouts: WorkoutSession[] = [];
  isLoading = true;
  ExerciseType = ExerciseType;
  MuscleGroup = MuscleGroup;

  constructor(
    private trainingService: TrainingService,
    public router: Router,
    private cdr: ChangeDetectorRef
  ) {}

ngOnInit(): void {
  this.trainingService.getWorkouts().subscribe({
    next: data => {
      this.workouts = data;
      this.isLoading = false;
      this.cdr.detectChanges();
    },
    error: () => {
      this.isLoading = false;
      this.cdr.detectChanges();
    }
  });
}

  formatDate(date: string): string {
    return new Date(date).toLocaleDateString('en-GB', {
      weekday: 'long',
      day: 'numeric',
      month: 'short',
      year: 'numeric'
    });
  }

  formatDuration(seconds: number | null): string {
    if (!seconds) return '-';
    const mins = Math.floor(seconds / 60);
    const secs = seconds % 60;
    return `${mins}m ${secs}s`;
  }

  getTotalSets(session: WorkoutSession): number {
    return session.sets.length + session.cardioSets.length;
  }

  getUniqueExercises(session: WorkoutSession): string[] {
    const strengthExercises = session.sets.map(s => s.exercise.name);
    const cardioExercises = session.cardioSets.map(s => s.exercise.name);
    return [...new Set([...strengthExercises, ...cardioExercises])];
  }
}