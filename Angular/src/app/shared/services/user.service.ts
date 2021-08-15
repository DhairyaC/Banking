import { ActivatedRoute } from '@angular/router';
import { AdminService } from 'src/app/shared/services/admin.service';
import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { CustomerLogin } from '../models/cust-login.model';

@Injectable({
  providedIn: 'root'
})
export class UserService {

  // base URL
  baseUrl = "http://localhost:16739/api/Admin"

  // object of customerLogin to bind data with customer login form
  customer:CustomerLogin = new CustomerLogin()

  constructor(private service:AdminService, private http:HttpClient, private route:ActivatedRoute) { }

  // get user details
  getUserDetails(custId:number){
    return this.http.get(`${this.baseUrl}/user-details?custid=${custId}`)
  }

  // gives otp and status
  trackStatus(srn:number){
    return this.http.get(`${this.baseUrl}/track-status?srn=${srn}`)
  }


  baseUserUrl:string = "http://localhost:16739/api/User"


  // user login
  userLogin(userId:number, password:string){
    return this.http.get(`${this.baseUserUrl}/login?userid=${userId}&password=${password}`, {responseType:'text'})
  }

  // forgot id otp
  forgotIdOtp(ac:number){
    return this.http.get(`${this.baseUserUrl}/forgot-id?accountnumber=${ac}`)
  }

  // receive forgotton userid
  receiveUserId(ac:number){
    return this.http.get(`${this.baseUserUrl}/receive-userid?accountnumber=${ac}`, {responseType:'text'})
    
  }

  // forgot password - getOtp
  forgotPasswordOtp(userId:number){
    return this.http.get(`${this.baseUserUrl}/forgot-password?userid=${userId}`)
  }

  // set new password
  setNewLoginPassword(userId:number, p1:string, p2:string){
    return this.http.put(`${this.baseUserUrl}/set-password?userid=${userId}&p1=${p1}&p2=${p2}`, null, {responseType:'text'})
  }

  // get account summary
  accountSummary(userId:number){
    return this.http.get(`${this.baseUserUrl}/account-summary?userid=${userId}`)
  }

  // lock account
  lockAccount(userId:number){
    return this.http.put(`${this.baseUserUrl}/lock-account?userid=${userId}`, null)
  }


  // save user ID
  userId:number
  
  // add beneficiary
  addBeneficiary(name:string, to1:number, to2:number){
    return this.http.put(`${this.baseUserUrl}/add-beneficiaries?userid=${this.userId}&name=${name}&b1=${to1}&b2=${to2}`, null, {responseType:'text'})
  }

  getTransactionList(){
    return this.http.get(`${this.baseUserUrl}/get-beneficiaries?userid=${this.userId}`)
  }

  // fund-transfer
  fundTransfer(amount:number, to:number, mode:string, remark?:string){
    return this.http.post(`${this.baseUserUrl}/transaction?userid=${this.userId}&amount=${amount}&to=${to}&mode=${mode}&remark=${remark}`, null)
  }

  // change password in dashboard
  changePassword(lp:string, clp:string, tp:string, ctp:string){
     return this.http.get(`${this.baseUserUrl}/change-password?userid=${this.userId}&lp=${lp}&clp=${clp}&tp=${tp}&ctp=${ctp}`)
  }

  // get otp for register ib
  getOtpIb(acNo:number){
    return this.http.get(`${this.baseUserUrl}/register-ib-otp?accnum=${acNo}`)
  }

  // register for internet banking
  registerIb(ac:number, lp:string, clp:string, tp:string, ctp:string){
    return this.http.get(`${this.baseUserUrl}/register-ib?accnum=${ac}&lp=${lp}&clp=${clp}&tp=${tp}&ctp=${ctp}`, {responseType:'text'})
  }

  // fetch user-details
  fetchUserDetails(){
    return this.http.get(`${this.baseUserUrl}/fetch-details?userid=${this.userId}`)
  }

  // get name for dashboard
  getName(userId:number){
    return this.http.get(`${this.baseUserUrl}/get-name?userid=${userId}`, {responseType:'text'})
  }

  // get info - account statement
  getInfo(){
    return this.http.get(`${this.baseUserUrl}/get-info?userid=${this.userId}`, {responseType:'json'})
  }

  // get tr range
  getTrRange(ac:number, from:string, to:string){
    return this.http.get(`${this.baseUserUrl}/get-tr-range?acno=${ac}&from=${from}&to=${to}`)
  }

  // get customer address for account details
  getAddress(){
    return this.http.get(`${this.baseUserUrl}/fetch-address?userid=${this.userId}`, {responseType:'json'})
  }

  updateBasicDetails(custId:number,title:string, fname:string, lname:string, mname:string, faname:string, mnum:number, email:string,
    aadhar:number, pan:string, occtype:string, incsource:string, gai:string){
      return this.http.put(`${this.baseUserUrl}/update-basic?id=${custId}&title=${title}&fname=${fname}&lname=${lname}&mname=${mname}&faname=${faname}&mnum=${mnum}&email=${email}&aadhar=${aadhar}&pan=${pan}&occtype=${occtype}&incsource=${incsource}&gai=${gai}`, null, {responseType:'text'})
  }

  // user registration
  userRegistration(obj:any){
    return this.http.post("http://localhost:16739/api/Registration/register", obj, {responseType:'text'})
  }

  // only change user dashboard login password
  changeLoginPassword(pass:string, confirmPass:string){
    return this.http.post(`${this.baseUserUrl}/only-login-password?userid=${this.userId}&pass1=${pass}&pass2=${confirmPass}`, null, {responseType:'text'})
  }

  // only change user dashboard transaction password
  changeTransactionPassword(pass:string, confirmPass:string){
    return this.http.post(`${this.baseUserUrl}/only-transaction-password?userid=${this.userId}&pass1=${pass}&pass2=${confirmPass}`, null, {responseType:'text'})
  }
}
