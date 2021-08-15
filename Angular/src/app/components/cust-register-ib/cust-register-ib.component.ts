import { RegisterInternetBanking } from './../../shared/models/register-internet-banking.model';
import { UserService } from './../../shared/services/user.service';
import { Component, OnInit } from '@angular/core';
import { AbstractControl, NgForm } from '@angular/forms';
import {Location} from '@angular/common';


@Component({
  selector: 'app-cust-register-ib',
  templateUrl: './cust-register-ib.component.html',
  styleUrls: ['./cust-register-ib.component.css']
})
export class CustRegisterIbComponent implements OnInit {

  confirmPasswordValidation(control:AbstractControl):{[key: string]:boolean}|null{
    if(control.parent?.get('password')?.value !== control.value){
      return {'cpass':true}
    }
    return null;
  }

  constructor(public custservice:UserService, private _location:Location) { }

  ngOnInit(): void {
  }


  // object to bind with register internet banking form
  ibRegister:RegisterInternetBanking = new RegisterInternetBanking()


  // register form internet banking
  registerInternetBanking(otp:number, lp:string, clp:string, tp:string, ctp:string, acNo:number){
    if(this.otp == otp){
      this.registerMainIb(acNo, lp, clp, tp, ctp)
    } 
    else 
    {
      alert("Invalid OTP")
    }
  }

  otp:number
  obj:any
  // otp
  getOtp(acNo:number){
    this.custservice.getOtpIb(acNo).subscribe(
      data=>{
        alert("OTP sent on registered email.")
        console.log(data)
        this.obj = data
        this.otp = this.obj
      }, err=>{
        console.log(err)
        if(err.error == "invalid accnum")
        {
          alert("Invalid Account Number")
        }
      }
    )
  }

  registerMainIb(ac:number, lp:string, clp:string, tp:string, ctp:string){
    this.custservice.registerIb(ac, lp, clp, tp, ctp).subscribe(
      data=>{
        console.log(data)
        if(data == "set")
        {
          alert("Password set successfully. You can login into your account.")
          this._location.back()
        } 
        else if(data == "invalid accnum")
        {
          alert("Invalid account number")
        } 
        else if(data == "login password didn't matched")
        {
          alert("Login password didn't matched")
        } 
        else if(data == "transaction password didn't matched") 
        {
          alert("Transaction password didn't matched")
        }
      }, err=>{
        console.log(err)
        
      }
    )
  }
}
