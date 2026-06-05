import { Component } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { NavComponent } from '../nav/nav';
import { CoachWidgetComponent } from '../../coach-widget/coach-widget'

@Component({
  selector: 'app-layout',
  standalone: true,
  imports: [RouterOutlet, NavComponent, CoachWidgetComponent],
  template: `
    <div class="app-layout">
      <app-nav></app-nav>
      <main class="main-content">
        <router-outlet></router-outlet>
      </main>
      <app-coach-widget></app-coach-widget>
    </div>
  `,
  styles: [`
    .app-layout {
      display: flex;
      min-height: 100vh;
    }

    .main-content {
      flex: 1;
      margin-left: 220px;
      min-height: 100vh;
      background: var(--bg-primary);
      overflow-y: auto;
    }
  `]
})
export class LayoutComponent {}