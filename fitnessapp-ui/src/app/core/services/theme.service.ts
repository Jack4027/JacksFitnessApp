import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class ThemeService {
  private readonly THEME_KEY = 'jfa-theme';
  private isDark$ = new BehaviorSubject<boolean>(this.loadTheme());

  isDark = this.isDark$.asObservable();

  private loadTheme(): boolean {
    const saved = localStorage.getItem(this.THEME_KEY);
    return saved !== null ? saved === 'dark' : true; // dark by default
  }

  toggle(): void {
    const isDark = !this.isDark$.value;
    this.isDark$.next(isDark);
    localStorage.setItem(this.THEME_KEY, isDark ? 'dark' : 'light');
    this.applyTheme(isDark);
  }

  apply(): void {
    this.applyTheme(this.isDark$.value);
  }

  private applyTheme(isDark: boolean): void {
    if (isDark) {
      document.body.classList.remove('light-theme');
    } else {
      document.body.classList.add('light-theme');
    }
  }

  get currentIsDark(): boolean {
    return this.isDark$.value;
  }
}