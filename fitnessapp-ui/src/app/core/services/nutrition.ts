import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { API_BASE_URL } from '../config/api.config';
import { FoodItem, Meal, MealItem, NutritionLog } from '../../models/nutrition.models';

@Injectable({
  providedIn: 'root'
})
export class NutritionService {
  private readonly apiUrl = `${API_BASE_URL}/nutrition`;

  constructor(private http: HttpClient) {}

  getNutritionLogs(): Observable<NutritionLog[]> {
    return this.http.get<NutritionLog[]>(this.apiUrl);
  }

  getNutritionLogByDate(date: string): Observable<NutritionLog> {
    return this.http.get<NutritionLog>(`${this.apiUrl}/date/${date}`);
  }

  createNutritionLog(log: Partial<NutritionLog>): Observable<NutritionLog> {
    return this.http.post<NutritionLog>(this.apiUrl, log);
  }

  addMeal(logId: number, meal: Partial<Meal>): Observable<Meal> {
    return this.http.post<Meal>(`${this.apiUrl}/${logId}/meals`, meal);
  }

deleteMeal(logId: number, mealId: number): Observable<void> {
  return this.http.delete<void>(`${this.apiUrl}/${logId}/meals/${mealId}`);
}

  addMealItem(mealId: number, item: any): Observable<MealItem> {
    return this.http.post<MealItem>(`${this.apiUrl}/meals/${mealId}/items`, item);
  }

  deleteMealItem(id: number): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/meal-items/${id}`);
  }

  searchFood(query: string): Observable<FoodItem[]> {
    return this.http.get<FoodItem[]>(`${this.apiUrl}/food/search?q=${encodeURIComponent(query)}`);
  }

  getFoodByBarcode(barcode: string): Observable<FoodItem> {
    return this.http.get<FoodItem>(`${this.apiUrl}/food/barcode/${barcode}`);
  }

  createFoodItem(food: Partial<FoodItem>): Observable<FoodItem> {
    return this.http.post<FoodItem>(`${this.apiUrl}/food`, food);
  }
}