import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Router } from '@angular/router';
import { Observable, tap } from 'rxjs';
import { API_BASE_URL } from '../config/api.config';
import { AuthResponse, LoginRequest, RegisterRequest } from '../../models/auth.models';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private readonly apiUrl = `${API_BASE_URL}/auth`;

  constructor(private http: HttpClient, private router: Router) {}

  register(request: RegisterRequest): Observable<any> {
    return this.http.post(`${this.apiUrl}/register`, request);
  }

  login(request: LoginRequest): Observable<AuthResponse> {
    return this.http.post<AuthResponse>(`${this.apiUrl}/login`, request).pipe(
      tap(response => {
        localStorage.setItem('token', response.token);
        localStorage.setItem('email', response.email);
        localStorage.setItem('displayName', response.displayName);
      })
    );
  }

  logout(): void {
    localStorage.removeItem('token');
    localStorage.removeItem('email');
    localStorage.removeItem('displayName');
    this.router.navigate(['/login']);
  }

  isAuthenticated(): boolean {
    const token = localStorage.getItem('token');
    if (!token) return false;
    const payload = JSON.parse(atob(token.split('.')[1]));
    return payload.exp > Date.now() / 1000;
  }

  getToken(): string | null {
    return localStorage.getItem('token');
  }

getDisplayName(): string {
  const name = localStorage.getItem('displayName') ?? '';
  // Fall back to email prefix if no display name set
  return name.includes('@') ? name.split('@')[0] : name;
}

  getEmail(): string {
    return localStorage.getItem('email') ?? '';
  }
}