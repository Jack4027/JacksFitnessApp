import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router, RouterModule } from '@angular/router';
import { MatIconModule } from '@angular/material/icon';
import { MatButtonModule } from '@angular/material/button';
import { MatTooltipModule } from '@angular/material/tooltip';
import { AuthService } from '../../../core/services/auth';
import { ThemeService } from '../../../core/services/theme.service';

@Component({
  selector: 'app-nav',
  standalone: true,
  imports: [
    CommonModule,
    RouterModule,
    MatIconModule,
    MatButtonModule,
    MatTooltipModule
  ],
  templateUrl: './nav.html',
  styleUrl: './nav.scss'
})
export class NavComponent implements OnInit {
  displayName = '';
  isDark = true;

  navItems = [
    { path: '/dashboard', icon: 'dashboard', label: 'Dashboard' },
    { path: '/workout', icon: 'fitness_center', label: 'Log workout' },
    { path: '/workout/history', icon: 'history', label: 'History' },
    { path: '/workout/personal-records', icon: 'emoji_events', label: 'Records' },
    { path: '/nutrition', icon: 'restaurant', label: 'Nutrition' },
    { path: '/programmes', icon: 'calendar_today', label: 'Programmes' },
    { path: '/metrics', icon: 'monitor_weight', label: 'Metrics' },
  ];

  constructor(
    private authService: AuthService,
    private themeService: ThemeService,
    public router: Router
  ) {}

  ngOnInit(): void {
    this.displayName = this.authService.getDisplayName();
    this.themeService.isDark.subscribe(isDark => this.isDark = isDark);
  }

  toggleTheme(): void {
    this.themeService.toggle();
  }

  logout(): void {
    this.authService.logout();
  }

  getInitials(): string {
    return this.displayName
      .split(' ')
      .map(n => n[0])
      .join('')
      .toUpperCase()
      .slice(0, 2);
  }
}