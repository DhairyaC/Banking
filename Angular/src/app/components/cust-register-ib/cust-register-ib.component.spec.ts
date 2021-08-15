import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CustRegisterIbComponent } from './cust-register-ib.component';

describe('CustRegisterIbComponent', () => {
  let component: CustRegisterIbComponent;
  let fixture: ComponentFixture<CustRegisterIbComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ CustRegisterIbComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(CustRegisterIbComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
