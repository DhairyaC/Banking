import { RegisterAccount } from './../models/register-account.model';
import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { AdminLogin } from '../models/admin-login.model';

@Injectable({
  providedIn: 'root'
})
export class AdminService {

  constructor(private http:HttpClient) { }

  // base url
  baseUrl:string="http://localhost:16739/api/Admin"

  // admin login
  adminLogin(userId:number, password:string){
    return this.http.get(`${this.baseUrl}/login?id=${userId}&password=${password}`, {responseType:'text'})
  }

  // object of AdminModel
  admin:AdminLogin = new AdminLogin()
  
  //object of RegisterModel
  customer:RegisterAccount = new RegisterAccount()


  // fill after backend api is active
  


  // get list of all unapproved and non-rejected accounts
  getUnapprovedAccountList(adminId:number){
    return this.http.get(`${this.baseUrl}/pending-list?adminid=${adminId}`)
  }



  // accept approval
  sendApproval(custId:number){
    return this.http.post(`${this.baseUrl}/after-approval?custid=${custId}`, null, {responseType:'text'})
  }


  // reject approval
  rejectApproval(custId:number){
    return this.http.put(`${this.baseUrl}/after-rejection?custid=${custId}`, null, {responseType:'text'})
  }



  // return particular account
  getParticularAccountDetails(custId:number):any{
    // let customer:RegisterAccount = new RegisterAccount()
    // this.unapprovedAccountList.forEach(cust => {
    //   if(cust.customerId === custId){
    //     customer = cust
    //   }
    //   return customer
    // });
  }

  // store adminId
  adminId:number
}
