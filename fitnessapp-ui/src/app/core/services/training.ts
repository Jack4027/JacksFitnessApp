import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { API_BASE_URL } from '../config/api.config';
import {
  Exercise,
  MuscleGroup,
  PersonalRecord,
  Programme,
  WorkoutSession,
  WorkoutSet,
  CardioSet
} from '../../models/training.models';

@Injectable({
  providedIn: 'root'
})
export class TrainingService {
  private readonly apiUrl = `${API_BASE_URL}`;

  constructor(private http: HttpClient) {}

  // Exercises
  getExercises(muscleGroup?: MuscleGroup): Observable<Exercise[]> {
    const url = muscleGroup !== undefined
      ? `${this.apiUrl}/exercises?muscleGroup=${muscleGroup}`
      : `${this.apiUrl}/exercises`;
    return this.http.get<Exercise[]>(url);
  }

  getExerciseById(id: number): Observable<Exercise> {
    return this.http.get<Exercise>(`${this.apiUrl}/exercises/${id}`);
  }

  createExercise(exercise: Partial<Exercise>): Observable<Exercise> {
    return this.http.post<Exercise>(`${this.apiUrl}/exercises`, exercise);
  }

  // Workouts
  getWorkouts(): Observable<WorkoutSession[]> {
    return this.http.get<WorkoutSession[]>(`${this.apiUrl}/workouts`);
  }

  getWorkoutById(id: number): Observable<WorkoutSession> {
    return this.http.get<WorkoutSession>(`${this.apiUrl}/workouts/${id}`);
  }

  createWorkoutSession(session: Partial<WorkoutSession>): Observable<WorkoutSession> {
    return this.http.post<WorkoutSession>(`${this.apiUrl}/workouts`, session);
  }

  deleteWorkoutSession(id: number): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/workouts/${id}`);
  }

  logWorkoutSet(sessionId: number, set: any): Observable<WorkoutSet> {
    return this.http.post<WorkoutSet>(`${this.apiUrl}/workouts/${sessionId}/sets`, set);
  }

  deleteWorkoutSet(id: number): Observable<void> {
  return this.http.delete<void>(`${this.apiUrl}/workouts/sets/${id}`);
}

deleteCardioSet(id: number): Observable<void> {
  return this.http.delete<void>(`${this.apiUrl}/workouts/cardio/${id}`);
}

  logCardioSet(sessionId: number, set: any): Observable<CardioSet> {
    return this.http.post<CardioSet>(`${this.apiUrl}/workouts/${sessionId}/cardio`, set);
  }

  getPersonalRecords(): Observable<PersonalRecord[]> {
    return this.http.get<PersonalRecord[]>(`${this.apiUrl}/workouts/personal-records`);
  }

  // Programmes
  getProgrammes(): Observable<Programme[]> {
    return this.http.get<Programme[]>(`${this.apiUrl}/programmes`);
  }

  getProgrammeById(id: number): Observable<Programme> {
    return this.http.get<Programme>(`${this.apiUrl}/programmes/${id}`);
  }

  createProgramme(programme: Partial<Programme>): Observable<Programme> {
    return this.http.post<Programme>(`${this.apiUrl}/programmes`, programme);
  }

  deleteProgramme(id: number): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/programmes/${id}`);
  }

  addProgrammeWeek(programmeId: number, week: any): Observable<any> {
    return this.http.post<any>(`${this.apiUrl}/programmes/${programmeId}/weeks`, week);
  }

  addProgrammeDay(weekId: number, day: any): Observable<any> {
    return this.http.post<any>(`${this.apiUrl}/programmes/weeks/${weekId}/days`, day);
  }

  addPlannedExercise(dayId: number, exercise: any): Observable<any> {
    return this.http.post<any>(`${this.apiUrl}/programmes/days/${dayId}/exercises`, exercise);
  }
}