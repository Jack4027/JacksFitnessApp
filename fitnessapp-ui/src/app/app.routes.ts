import { Routes } from '@angular/router';
import { AuthGuard } from './core/guards/auth.guard';

export const routes: Routes = [
  {
    path: 'login',
    loadComponent: () =>
      import('./features/auth/login/login').then(m => m.LoginComponent)
  },
  {
    path: 'dashboard',
    loadComponent: () =>
      import('./features/dashboard/dashboard').then(m => m.DashboardComponent),
    canActivate: [AuthGuard]
  },
  {
    path: 'workout',
    loadComponent: () =>
      import('./features/workout/log-workout/log-workout').then(m => m.LogWorkoutComponent),
    canActivate: [AuthGuard]
  },
  {
    path: 'workout/history',
    loadComponent: () =>
      import('./features/workout/workout-history/workout-history').then(m => m.WorkoutHistoryComponent),
    canActivate: [AuthGuard]
  },
  {
    path: 'workout/personal-records',
    loadComponent: () =>
      import('./features/workout/personal-records/personal-records').then(m => m.PersonalRecordsComponent),
    canActivate: [AuthGuard]
  },
  {
    path: 'programmes',
    loadComponent: () =>
      import('./features/programme/programme-list/programme-list').then(m => m.ProgrammeListComponent),
    canActivate: [AuthGuard]
  },
  {
    path: 'programmes/builder',
    loadComponent: () =>
      import('./features/programme/programme-builder/programme-builder').then(m => m.ProgrammeBuilderComponent),
    canActivate: [AuthGuard]
  },
  {
    path: 'nutrition',
    loadComponent: () =>
      import('./features/nutrition/nutrition-log/nutrition-log').then(m => m.NutritionLogComponent),
    canActivate: [AuthGuard]
  },
  {
    path: 'metrics',
    loadComponent: () =>
      import('./features/metrics/body-metrics/body-metrics').then(m => m.BodyMetricsComponent),
    canActivate: [AuthGuard]
  },
  {
    path: '',
    redirectTo: 'dashboard',
    pathMatch: 'full'
  },
  {
    path: '**',
    redirectTo: 'dashboard'
  }
];