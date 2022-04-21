import { ComponentFixture, TestBed } from '@angular/core/testing';

import { RegisterStep2InfoComponent } from './register-step2-info.component';

describe('RegisterStep2InfoComponent', () => {
  let component: RegisterStep2InfoComponent;
  let fixture: ComponentFixture<RegisterStep2InfoComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ RegisterStep2InfoComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(RegisterStep2InfoComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
