import { Routes } from '@angular/router';
import { AuthGuard } from './core/guards/auth.guard';

export const routes: Routes = [
  {
    path: 'login',
    loadComponent: () =>
      import('./features/auth/login/login').then(m => m.LoginComponent)
  },
  {
    path: '',
    loadComponent: () =>
      import('./shared/components/layout/layout').then(m => m.LayoutComponent),
    canActivate: [AuthGuard],
    children: [
      {
        path: 'dashboard',
        loadComponent: () =>
          import('./features/dashboard/dashboard').then(m => m.DashboardComponent)
      },
      {
        path: 'workout',
        loadComponent: () =>
          import('./features/workout/log-workout/log-workout').then(m => m.LogWorkoutComponent)
      },
      {
        path: 'workout/history',
        loadComponent: () =>
          import('./features/workout/workout-history/workout-history').then(m => m.WorkoutHistoryComponent)
      },
      {
        path: 'workout/personal-records',
        loadComponent: () =>
          import('./features/workout/personal-records/personal-records').then(m => m.PersonalRecordsComponent)
      },
      {
        path: 'programmes',
        loadComponent: () =>
          import('./features/programme/programme-list/programme-list').then(m => m.ProgrammeListComponent)
      },
      {
        path: 'programmes/builder',
        loadComponent: () =>
          import('./features/programme/programme-builder/programme-builder').then(m => m.ProgrammeBuilderComponent)
      },
      {
        path: 'nutrition',
        loadComponent: () =>
          import('./features/nutrition/nutrition-log/nutrition-log').then(m => m.NutritionLogComponent)
      },
      {
        path: 'metrics',
        loadComponent: () =>
          import('./features/metrics/body-metrics/body-metrics').then(m => m.BodyMetricsComponent)
      },
      {
        path: '',
        redirectTo: 'dashboard',
        pathMatch: 'full'
      }
    ]
  },
  {
    path: '**',
    redirectTo: 'dashboard'
  }
];