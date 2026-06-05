import { ChangeDetectorRef, Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, Validators, ReactiveFormsModule, FormControl } from '@angular/forms';
import { Router } from '@angular/router';
import { MatCardModule } from '@angular/material/card';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { FoodSearchComponent } from '../food-search/food-search';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatSnackBarModule, MatSnackBar } from '@angular/material/snack-bar';
import { MatDividerModule } from '@angular/material/divider';
import { MatAutocompleteModule } from '@angular/material/autocomplete';
import { NutritionService } from '../../../core/services/nutrition';
import { NutritionLog, Meal, FoodItem, MealType } from '../../../models/nutrition.models';
import { debounceTime, distinctUntilChanged, Subject, switchMap, of } from 'rxjs';
import { tap } from 'rxjs/operators';

@Component({
  selector: 'app-nutrition-log',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    MatCardModule,
    MatButtonModule,
    MatIconModule,
    MatFormFieldModule,
    MatInputModule,
    MatSelectModule,
    MatProgressSpinnerModule,
    MatSnackBarModule,
    MatDividerModule,
    MatAutocompleteModule,
    FoodSearchComponent
  ],
  templateUrl: './nutrition-log.html',
  styleUrl: './nutrition-log.scss'
})
export class NutritionLogComponent implements OnInit {
  todayLog: NutritionLog | null = null;
  isLoading = true;
  isCreatingLog = false;

  // Meal form
  mealForm: FormGroup;
  selectedMealId: number | null = null;

  // Food search
  foodSearchResults: FoodItem[] = [];
  isSearching = false;
  selectedFood: FoodItem | null = null;
  foodSearch$ = new Subject<string>();
quantityForm = new FormGroup({
  quantityGrams: new FormControl<number | null>(null, [Validators.required, Validators.min(1)])
});

  mealTypes = Object.entries(MealType)
    .filter(([key]) => isNaN(Number(key)))
    .map(([key, value]) => ({ label: key, value }));

  MealType = MealType;
  today = new Date().toISOString().split('T')[0];
  errorMessage = '';

  constructor(
    private fb: FormBuilder,
    private nutritionService: NutritionService,
    public router: Router,
    private snackBar: MatSnackBar,
      private cdr: ChangeDetectorRef
  ) {
    this.mealForm = this.fb.group({
      name: ['', Validators.required],
      type: ['', Validators.required]
    });

    this.quantityForm = this.fb.group({
      quantityGrams: new FormControl<number | null>(null, [Validators.required, Validators.min(1)])
    });
  }

  ngOnInit(): void {
    this.loadTodayLog();
    this.setupFoodSearch();
  }
private searchCache = new Map<string, FoodItem[]>();

setupFoodSearch(): void {
  this.foodSearch$.pipe(
    debounceTime(500),
    distinctUntilChanged(),
    switchMap(query => {
      if (!query || query.length < 2) return of([]);
      if (this.searchCache.has(query)) {
        this.isSearching = false;
        return of(this.searchCache.get(query)!);
      }
      this.isSearching = true;
      return this.nutritionService.searchFood(query).pipe(
        tap(results => this.searchCache.set(query, results))
      );
    })
  ).subscribe({
    next: results => {
      this.foodSearchResults = results;
      this.isSearching = false;
      this.cdr.detectChanges();
    },
    error: () => this.isSearching = false
  });
}

loadTodayLog(): void {
  this.nutritionService.getNutritionLogByDate(this.today).subscribe({
    next: log => {
      this.todayLog = log ?? null;
      this.isLoading = false;
      this.cdr.detectChanges();
    },
    error: (err) => {
      if (err.status === 404 || err.status === 204) {
        // Expected — no log exists yet
        this.todayLog = null;
      } else {
        // Unexpected error — show error message
        this.errorMessage = 'Failed to load nutrition log. Please try again.';
      }
      this.isLoading = false;
      this.cdr.detectChanges();
    }
  });
}

  createTodayLog(): void {
    this.isCreatingLog = true;
    this.nutritionService.createNutritionLog({ date: this.today, notes: '' }).subscribe({
      next: log => {
        this.todayLog = log;
        this.isCreatingLog = false;
      },
      error: () => {
        this.isCreatingLog = false;
        this.snackBar.open('Failed to create log', 'OK', { duration: 3000 });
      }
    });
  }

    addMeal(): void {
      if (!this.todayLog || this.mealForm.invalid) return;

      const payload = {
        name: this.mealForm.value.name,
        type: Number(this.mealForm.value.type)
      };

      this.nutritionService.addMeal(this.todayLog.id, payload).subscribe({
        next: meal => {
          this.todayLog!.meals.push(meal);
          this.mealForm.reset();
          this.snackBar.open('Meal added', 'OK', { duration: 2000 });
        },
        error: () => this.snackBar.open('Failed to add meal', 'OK', { duration: 3000 })
      });
    }

  onFoodSearch(event: Event): void {
    const query = (event.target as HTMLInputElement).value;
    this.foodSearch$.next(query);
  }

  selectFood(food: FoodItem): void {
    this.selectedFood = food;
    this.foodSearchResults = [];
  }

  deleteMeal(mealId: number): void {
  this.nutritionService.deleteMeal(this.todayLog!.id, mealId).subscribe({
    next: () => {
      const meal = this.todayLog!.meals.find(m => m.id === mealId);
      if (meal) {
        this.todayLog!.totalCalories = Math.max(0, this.todayLog!.totalCalories - meal.totalCalories);
        this.todayLog!.totalProtein = Math.max(0, this.todayLog!.totalProtein - meal.totalProtein);
        this.todayLog!.totalCarbs = Math.max(0, this.todayLog!.totalCarbs - meal.totalCarbs);
        this.todayLog!.totalFat = Math.max(0, this.todayLog!.totalFat - meal.totalFat);
      }
      this.todayLog!.meals = this.todayLog!.meals.filter(m => m.id !== mealId);
      this.cdr.detectChanges();
      this.snackBar.open('Meal deleted', 'OK', { duration: 2000 });
    },
    error: () => this.snackBar.open('Failed to delete meal', 'OK', { duration: 3000 })
  });
}

  addMealItem(mealId: number): void {
    if (!this.selectedFood || this.quantityForm.invalid) return;

    const item = {
      foodItemId: this.selectedFood.id,
      quantityGrams: this.quantityForm.value.quantityGrams
    };

    this.nutritionService.addMealItem(mealId, item).subscribe({
      next: created => {
        const meal = this.todayLog!.meals.find(m => m.id === mealId);
        if (meal) {
          meal.items.push(created);
          meal.totalCalories += created.calories;
          meal.totalProtein += created.protein;
          meal.totalCarbs += created.carbs;
          meal.totalFat += created.fat;
          this.todayLog!.totalCalories += created.calories;
          this.todayLog!.totalProtein += created.protein;
          this.todayLog!.totalCarbs += created.carbs;
          this.todayLog!.totalFat += created.fat;
        }
        this.selectedFood = null;
        this.selectedMealId = null;
        this.quantityForm.reset();
        this.snackBar.open('Food added', 'OK', { duration: 2000 });
      },
      error: () => this.snackBar.open('Failed to add food', 'OK', { duration: 3000 })
    });
  }

  deleteMealItem(mealId: number, itemId: number, calories: number, protein: number, carbs: number, fat: number): void {
    this.nutritionService.deleteMealItem(itemId).subscribe({
      next: () => {
        const meal = this.todayLog!.meals.find(m => m.id === mealId);
        if (meal) {
          meal.items = meal.items.filter(i => i.id !== itemId);
          meal.totalCalories -= calories;
          meal.totalProtein -= protein;
          meal.totalCarbs -= carbs;
          meal.totalFat -= fat;
          this.todayLog!.totalCalories = Math.max(0, this.todayLog!.totalCalories - calories);
          this.todayLog!.totalProtein = Math.max(0, this.todayLog!.totalProtein - protein);
          this.todayLog!.totalCarbs = Math.max(0, this.todayLog!.totalCarbs - carbs);
          this.todayLog!.totalFat = Math.max(0, this.todayLog!.totalFat - fat);
        }
        this.snackBar.open('Item removed', 'OK', { duration: 2000 });
      },
      error: () => this.snackBar.open('Failed to remove item', 'OK', { duration: 3000 })
    });
  }

  round(value: number): number {
    return Math.round(value * 10) / 10;
  }
}