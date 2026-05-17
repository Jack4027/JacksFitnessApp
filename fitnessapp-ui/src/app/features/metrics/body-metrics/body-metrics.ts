import { ChangeDetectorRef, Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, ReactiveFormsModule } from '@angular/forms';
import { Router } from '@angular/router';
import { MatCardModule } from '@angular/material/card';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatToolbarModule } from '@angular/material/toolbar';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatSnackBarModule, MatSnackBar } from '@angular/material/snack-bar';
import { MatExpansionModule } from '@angular/material/expansion';
import { MetricsService } from '../../../core/services/metrics';
import { BodyMetric } from '../../../models/metrics.models';

@Component({
  selector: 'app-body-metrics',
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
    MatProgressSpinnerModule,
    MatSnackBarModule,
    MatExpansionModule
  ],
  templateUrl: './body-metrics.html',
  styleUrl: './body-metrics.scss'
})
export class BodyMetricsComponent implements OnInit {
  metrics: BodyMetric[] = [];
  latestMetric: BodyMetric | null = null;
  isLoading = true;
  isSaving = false;
  showForm = false;

  metricForm: FormGroup;

  constructor(
    private fb: FormBuilder,
    private metricsService: MetricsService,
    public router: Router,
    private snackBar: MatSnackBar,
      private cdr: ChangeDetectorRef
  ) {
    this.metricForm = this.fb.group({
      date: [new Date().toISOString().split('T')[0]],
      weightKg: [''],
      bodyFatPercentage: [''],
      muscleMassKg: [''],
      chestCm: [''],
      waistCm: [''],
      hipsCm: [''],
      bicepCm: [''],
      thighCm: [''],
      notes: ['']
    });
  }

  ngOnInit(): void {
    this.loadMetrics();
  }

loadMetrics(): void {
  this.metricsService.getBodyMetrics().subscribe({
    next: data => {
      this.metrics = data;
      this.latestMetric = data.length > 0 ? data[0] : null;
      this.isLoading = false;
      this.cdr.detectChanges();
    },
    error: () => {
      this.isLoading = false;
      this.cdr.detectChanges();
    }
  });
}

  saveMetric(): void {
    this.isSaving = true;
    const values = this.metricForm.value;

    const metric = {
      date: values.date,
      weightKg: values.weightKg || null,
      bodyFatPercentage: values.bodyFatPercentage || null,
      muscleMassKg: values.muscleMassKg || null,
      chestCm: values.chestCm || null,
      waistCm: values.waistCm || null,
      hipsCm: values.hipsCm || null,
      bicepCm: values.bicepCm || null,
      thighCm: values.thighCm || null,
      notes: values.notes || ''
    };

    this.metricsService.createBodyMetric(metric).subscribe({
      next: created => {
        this.metrics.unshift(created);
        this.latestMetric = created;
        this.showForm = false;
        this.metricForm.reset({ date: new Date().toISOString().split('T')[0] });
        this.isSaving = false;
        this.snackBar.open('Metrics saved', 'OK', { duration: 2000 });
      },
      error: () => {
        this.isSaving = false;
        this.snackBar.open('Failed to save metrics', 'OK', { duration: 3000 });
      }
    });
  }

  formatDate(date: string): string {
    return new Date(date).toLocaleDateString('en-GB', {
      day: 'numeric',
      month: 'short',
      year: 'numeric'
    });
  }
}