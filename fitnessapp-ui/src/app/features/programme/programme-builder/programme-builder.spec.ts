import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ProgrammeBuilderComponent } from './programme-builder';

describe('ProgrammeBuilderComponent', () => {
  let component: ProgrammeBuilderComponent;
  let fixture: ComponentFixture<ProgrammeBuilderComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ProgrammeBuilderComponent],
    }).compileComponents();

    fixture = TestBed.createComponent(ProgrammeBuilderComponent);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
