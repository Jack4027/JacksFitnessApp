import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, Validators, ReactiveFormsModule } from '@angular/forms';
import { Router } from '@angular/router';
import { MatCardModule } from '@angular/material/card';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatTabsModule } from '@angular/material/tabs';
import { AuthService } from '../../../core/services/auth';
import { MatIcon } from "@angular/material/icon";

@Component({
  selector: 'app-login',
  standalone: true,
imports: [
    CommonModule,
    ReactiveFormsModule,
    MatInputModule,
    MatButtonModule,
    MatFormFieldModule,
    MatProgressSpinnerModule,
    MatTabsModule,
    MatIcon
],
  templateUrl: './login.html',
  styleUrl: './login.scss'
})
export class LoginComponent {
  loginForm: FormGroup;
  registerForm: FormGroup;
  isLoading = false;
  errorMessage = '';

  constructor(
    private fb: FormBuilder,
    private authService: AuthService,
    private router: Router
  ) {
    this.loginForm = this.fb.group({
      email: ['', [Validators.required, Validators.email]],
      password: ['', [Validators.required, Validators.minLength(8)]]
    });

    this.registerForm = this.fb.group({
      email: ['', [Validators.required, Validators.email]],
      password: ['', [Validators.required, Validators.minLength(8)]],
      displayName: ['', [Validators.required]]
    });
  }

  onLogin(): void {
    if (this.loginForm.invalid) return;
    this.isLoading = true;
    this.errorMessage = '';

    this.authService.login(this.loginForm.value).subscribe({
      next: () => this.router.navigate(['/dashboard']),
      error: () => {
        this.errorMessage = 'Invalid email or password.';
        this.isLoading = false;
      }
    });
  }

  onRegister(): void {
    if (this.registerForm.invalid) return;
    this.isLoading = true;
    this.errorMessage = '';

    this.authService.register(this.registerForm.value).subscribe({
      next: () => {
        // Auto login after registration
        const { email, password } = this.registerForm.value;
        this.authService.login({ email, password }).subscribe({
          next: () => this.router.navigate(['/dashboard']),
          error: () => {
            this.errorMessage = 'Registration successful. Please log in.';
            this.isLoading = false;
          }
        });
      },
      error: (err) => {
        this.errorMessage = err.error?.[0] ?? 'Registration failed.';
        this.isLoading = false;
      }
    });
  }
}