import { UserAddBeneficiaryComponent } from './components/user-add-beneficiary/user-add-beneficiary.component';
import { UserChangePasswordComponent } from './components/user-change-password/user-change-password.component';
import { UserAccountStatementComponent } from './components/user-account-statement/user-account-statement.component';
import { SetPasswordComponent } from './components/set-password/set-password.component';

import { FundTransferComponent } from './components/fund-transfer/fund-transfer.component';
import { UserAccountDetailsComponent } from './components/user-account-details/user-account-details.component';
import { UserAccountSummaryComponent } from './components/user-account-summary/user-account-summary.component';
import { UserDashboardComponent } from './components/user-dashboard/user-dashboard.component';
import { ForgotUserIdComponent } from './components/forgot-user-id/forgot-user-id.component';
import { CustLoginComponent } from './components/cust-login/cust-login.component';
import { CustRegisterAccountComponent } from './components/cust-register-account/cust-register-account.component';
import { AccountListComponent } from './components/account-list/account-list.component';
import { CustDetailsComponent } from './components/cust-details/cust-details.component';
import { AdminLoginComponent } from './components/admin-login/admin-login.component';
import { HomeComponent } from './components/home/home.component';
import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AdminDashboardComponent } from './components/admin-dashboard/admin-dashboard.component';
import { CustRegisterIbComponent } from './components/cust-register-ib/cust-register-ib.component';
import { ForgotLoginPasswordComponent } from './components/forgot-login-password/forgot-login-password.component';
import { ContactUsComponent } from './components/contact-us/contact-us.component';
import { PageNotFoundComponent } from './components/page-not-found/page-not-found.component';



const routes: Routes = [
  {
    path:'home', component:HomeComponent
  },
  {
    path:'', redirectTo:'/home', pathMatch:'full'
  },
  {
    path:'contact-us', component:ContactUsComponent
  },
  {
    path:'admin-login', component:AdminLoginComponent
  },
  {
    path:'admin-dashboard/:id', component:AdminDashboardComponent,
    children:[
      {path:'details/:id', component:CustDetailsComponent},
      {path:'', component:AccountListComponent}
    ]
  },
  {
    path:'register-account', component:CustRegisterAccountComponent
  },
  {
    path:'register-ib', component:CustRegisterIbComponent
  },
  {
    path:'login', component:CustLoginComponent,
  },
  {
    path:'forgot-password', component:ForgotLoginPasswordComponent
  },
  {
    path:'forgot-id', component:ForgotUserIdComponent
  },
  {
    path:'dashboard/:id', component:UserDashboardComponent,
    children:[
      {path:'', component:UserAccountSummaryComponent},
      {path:'account-summary', component:UserAccountSummaryComponent},
      {path:'account-details', component:UserAccountDetailsComponent},
      {path:'fund-transfer', component:FundTransferComponent},
      {path:'statement', component:UserAccountStatementComponent},
      {path:'add-beneficiary', component:UserAddBeneficiaryComponent},
      {path:'change-password', component:UserChangePasswordComponent} 
    ]
  },
  {
    path:'set-new-password/:id', component:SetPasswordComponent
  },
  {
    path:'**', component:PageNotFoundComponent
  }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }

export const routingComponents = [HomeComponent, AdminLoginComponent, AdminDashboardComponent, CustDetailsComponent, 
  CustRegisterAccountComponent, CustRegisterIbComponent, CustLoginComponent, ForgotLoginPasswordComponent, ForgotUserIdComponent]