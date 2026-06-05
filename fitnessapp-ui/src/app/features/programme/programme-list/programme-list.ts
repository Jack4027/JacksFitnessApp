import { ChangeDetectorRef, Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';
import { MatCardModule } from '@angular/material/card';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatChipsModule } from '@angular/material/chips';
import { MatSnackBarModule, MatSnackBar } from '@angular/material/snack-bar';
import { TrainingService } from '../../../core/services/training';
import { Programme, ProgrammeGoal } from '../../../models/training.models';

@Component({
  selector: 'app-programme-list',
  standalone: true,
  imports: [
    CommonModule,
    MatCardModule,
    MatButtonModule,
    MatIconModule,
    MatProgressSpinnerModule,
    MatChipsModule,
    MatSnackBarModule
  ],
  templateUrl: './programme-list.html',
  styleUrl: './programme-list.scss'
})
export class ProgrammeListComponent implements OnInit {
  programmes: Programme[] = [];
  isLoading = true;
  ProgrammeGoal = ProgrammeGoal;

  goalLabels: Record<number, string> = {
    [ProgrammeGoal.Strength]: 'Strength',
    [ProgrammeGoal.Hypertrophy]: 'Hypertrophy',
    [ProgrammeGoal.Endurance]: 'Endurance',
    [ProgrammeGoal.WeightLoss]: 'Weight loss',
    [ProgrammeGoal.GeneralFitness]: 'General fitness'
  };

  constructor(
    private trainingService: TrainingService,
    public router: Router,
    private snackBar: MatSnackBar,
      private cdr: ChangeDetectorRef
  ) {}

  ngOnInit(): void {
    this.trainingService.getProgrammes().subscribe({
      next: data => {
        this.programmes = data;
        this.isLoading = false;
        this.cdr.detectChanges();
      },
      error: () => {
        this.isLoading = false;
        this.cdr.detectChanges();
      }
    });
  }

  deleteProgramme(id: number): void {
    this.trainingService.deleteProgramme(id).subscribe({
      next: () => {
        this.programmes = this.programmes.filter(p => p.id !== id);
        this.snackBar.open('Programme deleted', 'OK', { duration: 2000 });
      },
      error: () => this.snackBar.open('Failed to delete programme', 'OK', { duration: 3000 })
    });
  }
}