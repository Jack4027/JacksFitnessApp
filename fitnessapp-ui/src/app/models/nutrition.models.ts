export interface NutritionLog {
  id: number;
  date: string;
  notes: string;
  totalCalories: number;
  totalProtein: number;
  totalCarbs: number;
  totalFat: number;
  meals: Meal[];
}

export interface Meal {
  id: number;
  name: string;
  type: MealType;
  totalCalories: number;
  totalProtein: number;
  totalCarbs: number;
  totalFat: number;
  items: MealItem[];
}

export interface MealItem {
  id: number;
  quantityGrams: number;
  calories: number;
  protein: number;
  carbs: number;
  fat: number;
  foodItem: FoodItem;
}

export interface FoodItem {
  id: number;
  name: string;
  brand: string | null;
  barcode: string | null;
  caloriesPer100g: number;
  proteinPer100g: number;
  carbsPer100g: number;
  fatPer100g: number;
  fibrePer100g: number;
  source: FoodItemSource;
}

export enum MealType {
  Breakfast = 0,
  Lunch = 1,
  Dinner = 2,
  Snack = 3,
  PreWorkout = 4,
  PostWorkout = 5
}

export enum FoodItemSource {
  OpenFoodFacts = 0,
  UserCreated = 1,
  Seeded = 2
}