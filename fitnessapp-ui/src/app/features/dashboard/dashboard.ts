import { Component, OnInit, ChangeDetectorRef } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';
import { forkJoin } from 'rxjs';
import { MatCardModule } from '@angular/material/card';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatToolbarModule } from '@angular/material/toolbar';
import { MatMenuModule } from '@angular/material/menu';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { AuthService } from '../../core/services/auth';
import { TrainingService } from '../../core/services/training';
import { MetricsService } from '../../core/services/metrics';
import { WorkoutSession, PersonalRecord } from '../../models/training.models';
import { BodyMetric } from '../../models/metrics.models';

@Component({
  selector: 'app-dashboard',
  standalone: true,
  imports: [
    CommonModule,
    MatCardModule,
    MatButtonModule,
    MatIconModule,
    MatToolbarModule,
    MatMenuModule,
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
  isLoading = true;

  constructor(
    private authService: AuthService,
    private trainingService: TrainingService,
    private metricsService: MetricsService,
    private router: Router,
    private cdr: ChangeDetectorRef
  ) {}

  ngOnInit(): void {
    this.displayName = this.authService.getDisplayName();
    this.loadDashboardData();
  }

  loadDashboardData(): void {
    forkJoin({
      workouts: this.trainingService.getWorkouts(),
      personalRecords: this.trainingService.getPersonalRecords(),
      latestMetric: this.metricsService.getLatestBodyMetric()
    }).subscribe({
      next: results => {
        this.recentWorkouts = results.workouts.slice(0, 5);
        this.personalRecords = results.personalRecords.slice(0, 5);
        this.latestMetric = results.latestMetric;
        this.isLoading = false;
        this.cdr.detectChanges();
      },
      error: (err) => {
        console.error('dashboard error', err);
        this.isLoading = false;
        this.cdr.detectChanges();
      }
    });
  }

  navigate(path: string): void {
    this.router.navigate([path]);
  }

  logout(): void {
    this.authService.logout();
  }

  formatDate(date: string): string {
    return new Date(date).toLocaleDateString('en-GB', {
      day: 'numeric',
      month: 'short',
      year: 'numeric'
    });
  }
}