import { ComponentFixture, TestBed } from '@angular/core/testing';

import { LogWorkoutComponent } from './log-workout';

describe('LogWorkoutComponent', () => {
  let component: LogWorkoutComponent;
  let fixture: ComponentFixture<LogWorkoutComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [LogWorkoutComponent],
    }).compileComponents();

    fixture = TestBed.createComponent(LogWorkoutComponent);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
