import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CoachWidgetComponent } from './coach-widget';

describe('CoachWidgetComponent', () => {
  let component: CoachWidgetComponent;
  let fixture: ComponentFixture<CoachWidgetComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [CoachWidgetComponent],
    }).compileComponents();

    fixture = TestBed.createComponent(CoachWidgetComponent);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
