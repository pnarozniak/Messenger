import { ComponentFixture, TestBed } from '@angular/core/testing';

import { FloatingBoxComponent } from './floating-box.component';

describe('FloatingBoxComponent', () => {
  let component: FloatingBoxComponent;
  let fixture: ComponentFixture<FloatingBoxComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ FloatingBoxComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(FloatingBoxComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
