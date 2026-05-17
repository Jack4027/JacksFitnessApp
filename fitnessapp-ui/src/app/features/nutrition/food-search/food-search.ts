import { Component, EventEmitter, Output } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormControl, ReactiveFormsModule } from '@angular/forms';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatAutocompleteModule } from '@angular/material/autocomplete';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatIconModule } from '@angular/material/icon';
import { debounceTime, distinctUntilChanged, Subject, switchMap, of } from 'rxjs';
import { NutritionService } from '../../../core/services/nutrition';
import { FoodItem } from '../../../models/nutrition.models';

@Component({
  selector: 'app-food-search',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    MatFormFieldModule,
    MatInputModule,
    MatAutocompleteModule,
    MatProgressSpinnerModule,
    MatIconModule
  ],
  templateUrl: './food-search.html',
  styleUrl: './food-search.scss'
})
export class FoodSearchComponent {
  // Emits the selected food item to the parent component
  @Output() foodSelected = new EventEmitter<FoodItem>();

  searchControl = new FormControl('');
  searchResults: FoodItem[] = [];
  isSearching = false;
  private search$ = new Subject<string>();

  constructor(private nutritionService: NutritionService) {
    this.search$.pipe(
      debounceTime(300),
      distinctUntilChanged(),
      switchMap(query => {
        if (!query || query.length < 2) {
          this.searchResults = [];
          return of([]);
        }
        this.isSearching = true;
        return this.nutritionService.searchFood(query);
      })
    ).subscribe({
      next: results => {
        this.searchResults = results;
        this.isSearching = false;
      },
      error: () => this.isSearching = false
    });
  }

  onInput(event: Event): void {
    const query = (event.target as HTMLInputElement).value;
    this.search$.next(query);
  }

  onFoodSelected(food: FoodItem): void {
    this.foodSelected.emit(food);
    this.searchControl.reset();
    this.searchResults = [];
  }

  displayFn(food: FoodItem): string {
    return food ? food.name : '';
  }
}