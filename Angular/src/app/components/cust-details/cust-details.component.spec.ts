import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CustDetailsComponent } from './cust-details.component';

describe('CustDetailsComponent', () => {
  let component: CustDetailsComponent;
  let fixture: ComponentFixture<CustDetailsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ CustDetailsComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(CustDetailsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
