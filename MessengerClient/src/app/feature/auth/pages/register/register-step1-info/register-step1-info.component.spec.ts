import { ComponentFixture, TestBed } from '@angular/core/testing';

import { RegisterStep1InfoComponent } from './register-step1-info.component';

describe('RegisterStep1InfoComponent', () => {
  let component: RegisterStep1InfoComponent;
  let fixture: ComponentFixture<RegisterStep1InfoComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ RegisterStep1InfoComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(RegisterStep1InfoComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
