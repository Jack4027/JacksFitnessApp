import { Component, OnInit, ChangeDetectorRef } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';
import { forkJoin } from 'rxjs';
import { catchError, of } from 'rxjs';
import { MatCardModule } from '@angular/material/card';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { AuthService } from '../../core/services/auth';
import { TrainingService } from '../../core/services/training';
import { MetricsService } from '../../core/services/metrics';
import { NutritionService } from '../../core/services/nutrition';
import { WorkoutSession, PersonalRecord } from '../../models/training.models';
import { BodyMetric } from '../../models/metrics.models';
import { NutritionLog } from '../../models/nutrition.models';

@Component({
  selector: 'app-dashboard',
  standalone: true,
  imports: [
    CommonModule,
    MatCardModule,
    MatButtonModule,
    MatIconModule,
    MatProgressSpinnerModule
  ],
  templateUrl: './dashboard.html',
  styleUrl: './dashboard.scss'
})
export class DashboardComponent implements OnInit {
  displayName = '';
  recentWorkouts: WorkoutSession[] = [];
  latestMetric: BodyMetric | null = null;
  personalRecords: PersonalRecord[] = [];
  todayLog: NutritionLog | null = null;
  isLoading = true;

  // Macro goals — could be user-configurable in future
  calorieGoal = 2800;
  proteinGoal = 180;
  carbGoal = 300;
  fatGoal = 80;
  errorMessage = '';

  constructor(
    private authService: AuthService,
    private trainingService: TrainingService,
    private metricsService: MetricsService,
    private nutritionService: NutritionService,
    private router: Router,
    private cdr: ChangeDetectorRef
  ) {}

  ngOnInit(): void {
    console.log('Hostname:', window.location.hostname);
    this.displayName = this.authService.getDisplayName();
    this.loadDashboardData();
  }

  loadDashboardData(): void {
    const today = new Date().toISOString().split('T')[0];

    forkJoin({
  workouts: this.trainingService.getWorkouts().pipe(
    catchError(() => of([]))
  ),
  personalRecords: this.trainingService.getPersonalRecords().pipe(
    catchError(() => of([]))
  ),
  latestMetric: this.metricsService.getLatestBodyMetric().pipe(
    catchError(() => of(null))
  ),
  todayLog: this.nutritionService.getNutritionLogByDate(today).pipe(
    catchError(() => of(null))
  )
    }).subscribe({
      next: results => {
        this.recentWorkouts = results.workouts.slice(0, 5);
        this.personalRecords = results.personalRecords.slice(0, 5);
        this.latestMetric = results.latestMetric;
        this.todayLog = results.todayLog;
        this.isLoading = false;
        this.cdr.detectChanges();
      },
    error: () => {
      this.errorMessage = 'Failed to load dashboard. Please refresh.';
      this.isLoading = false;
      this.cdr.detectChanges();
    }
    });
  }

  navigate(path: string): void {
    this.router.navigate([path]);
  }

  formatDate(date: string): string {
    return new Date(date).toLocaleDateString('en-GB', {
      weekday: 'short',
      day: 'numeric',
      month: 'short'
    });
  }

  getTotalVolume(session: WorkoutSession): number {
    return session.sets.reduce((acc, s) => acc + (s.weightKg * s.repsCompleted), 0);
  }

  getMacroPercent(value: number, goal: number): number {
    return Math.min(100, Math.round((value / goal) * 100));
  }

  getGreeting(): string {
    const hour = new Date().getHours();
    if (hour < 12) return 'Good morning';
    if (hour < 18) return 'Good afternoon';
    return 'Good evening';
  }

  getTodayDate(): string {
    return new Date().toLocaleDateString('en-GB', {
      weekday: 'long',
      day: 'numeric',
      month: 'long'
    });
  }

  round(value: number): number {
    return Math.round(value * 10) / 10;
  }
}