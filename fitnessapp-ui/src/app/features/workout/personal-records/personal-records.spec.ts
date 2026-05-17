import { ComponentFixture, TestBed } from '@angular/core/testing';

import { PersonalRecordsComponent } from './personal-records';

describe('PersonalRecordsComponent', () => {
  let component: PersonalRecordsComponent;
  let fixture: ComponentFixture<PersonalRecordsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [PersonalRecordsComponent],
    }).compileComponents();

    fixture = TestBed.createComponent(PersonalRecordsComponent);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
