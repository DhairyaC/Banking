import { ComponentFixture, TestBed } from '@angular/core/testing';

import { UserAccountStatementComponent } from './user-account-statement.component';

describe('UserAccountStatementComponent', () => {
  let component: UserAccountStatementComponent;
  let fixture: ComponentFixture<UserAccountStatementComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ UserAccountStatementComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(UserAccountStatementComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
