import { ComponentFixture, TestBed } from '@angular/core/testing';

import { BodyMetricsComponent } from './body-metrics';

describe('BodyMetricsComponent', () => {
  let component: BodyMetricsComponent;
  let fixture: ComponentFixture<BodyMetricsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [BodyMetricsComponent],
    }).compileComponents();

    fixture = TestBed.createComponent(BodyMetricsComponent);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
