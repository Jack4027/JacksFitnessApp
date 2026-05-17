import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { BodyMetric } from '../../models/metrics.models';
import { API_BASE_URL } from '../config/api.config';

@Injectable({
  providedIn: 'root'
})
export class MetricsService {
  private readonly apiUrl = `${API_BASE_URL}/metrics`;

  constructor(private http: HttpClient) {}

  getBodyMetrics(): Observable<BodyMetric[]> {
    return this.http.get<BodyMetric[]>(this.apiUrl);
  }

  getLatestBodyMetric(): Observable<BodyMetric> {
    return this.http.get<BodyMetric>(`${this.apiUrl}/latest`);
  }

  createBodyMetric(metric: Partial<BodyMetric>): Observable<BodyMetric> {
    return this.http.post<BodyMetric>(this.apiUrl, metric);
  }

  updateBodyMetric(id: number, metric: Partial<BodyMetric>): Observable<BodyMetric> {
    return this.http.put<BodyMetric>(`${this.apiUrl}/${id}`, metric);
  }

  deleteBodyMetric(id: number): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${id}`);
  }
}