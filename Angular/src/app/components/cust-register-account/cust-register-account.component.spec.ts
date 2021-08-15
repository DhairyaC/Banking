import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CustRegisterAccountComponent } from './cust-register-account.component';

describe('CustRegisterAccountComponent', () => {
  let component: CustRegisterAccountComponent;
  let fixture: ComponentFixture<CustRegisterAccountComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ CustRegisterAccountComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(CustRegisterAccountComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
