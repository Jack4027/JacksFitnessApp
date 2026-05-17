import { ComponentFixture, TestBed } from '@angular/core/testing';

import { FoodSearchComponent } from './food-search';

describe('FoodSearchComponent', () => {
  let component: FoodSearchComponent;
  let fixture: ComponentFixture<FoodSearchComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [FoodSearchComponent],
    }).compileComponents();

    fixture = TestBed.createComponent(FoodSearchComponent);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
