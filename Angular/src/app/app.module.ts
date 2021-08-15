import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { FormsModule } from '@angular/forms';
import { ReactiveFormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { BackButtonDisableModule } from 'angular-disable-browser-back-button';

import { AppRoutingModule, routingComponents } from './app-routing.module';
import { AppComponent } from './app.component';
import { AccountListComponent } from './components/account-list/account-list.component';
import { UserService } from './shared/services/user.service';
import { AdminService } from './shared/services/admin.service';
import { UserDashboardComponent } from './components/user-dashboard/user-dashboard.component';
import { UserAccountDetailsComponent } from './components/user-account-details/user-account-details.component';
import { UserAccountSummaryComponent } from './components/user-account-summary/user-account-summary.component';
import { FundTransferComponent } from './components/fund-transfer/fund-transfer.component';
import { UserAccountStatementComponent } from './components/user-account-statement/user-account-statement.component';
import { SetPasswordComponent } from './components/set-password/set-password.component';
import { UserChangePasswordComponent } from './components/user-change-password/user-change-password.component';
import { UserAddBeneficiaryComponent } from './components/user-add-beneficiary/user-add-beneficiary.component';
import { CustRegisterAccountComponent } from './components/cust-register-account/cust-register-account.component';
import { TransactionReceiptComponent } from './components/fund-transfer/transaction-receipt/transaction-receipt.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { MaterialModule } from './shared/material/material.module';
import { ContactUsComponent } from './components/contact-us/contact-us.component';
import { PageNotFoundComponent } from './components/page-not-found/page-not-found.component';



@NgModule({
  declarations: [
    AppComponent,
    routingComponents,
    AccountListComponent,
    UserDashboardComponent,
    UserAccountDetailsComponent,
    UserAccountSummaryComponent,
    FundTransferComponent,
    UserAccountStatementComponent,
    
    SetPasswordComponent,
    UserChangePasswordComponent,
    UserAddBeneficiaryComponent,
    CustRegisterAccountComponent,
    TransactionReceiptComponent,
    ContactUsComponent,
    PageNotFoundComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    FormsModule,
    ReactiveFormsModule,
    HttpClientModule,
    BrowserAnimationsModule,
    MaterialModule
    // BackButtonDisableModule.forRoot({
    //   preserveScrollPosition: true
    // })
  ],
  providers: [UserService, AdminService],
  bootstrap: [AppComponent]
})
export class AppModule { }
