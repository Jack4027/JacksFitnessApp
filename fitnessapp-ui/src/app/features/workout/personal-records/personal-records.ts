import { ChangeDetectorRef, Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';
import { MatCardModule } from '@angular/material/card';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { TrainingService } from '../../../core/services/training';
import { PersonalRecord } from '../../../models/training.models';

@Component({
  selector: 'app-personal-records',
  standalone: true,
  imports: [
    CommonModule,
    MatCardModule,
    MatButtonModule,
    MatIconModule,
    MatProgressSpinnerModule
  ],
  templateUrl: './personal-records.html',
  styleUrl: './personal-records.scss'
})
export class PersonalRecordsComponent implements OnInit {
  personalRecords: PersonalRecord[] = [];
  isLoading = true;

  constructor(
    private trainingService: TrainingService,
    public router: Router,
      private cdr: ChangeDetectorRef
  ) {}

    ngOnInit(): void {
      this.trainingService.getPersonalRecords().subscribe({
        next: data => {
          this.personalRecords = data;
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
      day: 'numeric',
      month: 'short',
      year: 'numeric'
    });
  }
}