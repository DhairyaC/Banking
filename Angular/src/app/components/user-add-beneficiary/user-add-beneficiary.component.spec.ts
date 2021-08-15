import { ComponentFixture, TestBed } from '@angular/core/testing';

import { UserAddBeneficiaryComponent } from './user-add-beneficiary.component';

describe('UserAddBeneficiaryComponent', () => {
  let component: UserAddBeneficiaryComponent;
  let fixture: ComponentFixture<UserAddBeneficiaryComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ UserAddBeneficiaryComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(UserAddBeneficiaryComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
