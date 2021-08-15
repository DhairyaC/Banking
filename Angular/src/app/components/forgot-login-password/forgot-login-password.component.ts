import { Router } from '@angular/router';
import { UserService } from './../../shared/services/user.service';
import { Component, OnInit } from '@angular/core';
import { forgotLoginPassword } from 'src/app/shared/models/forgot-login-password.model';

@Component({
  selector: 'app-forgot-login-password',
  templateUrl: './forgot-login-password.component.html',
  styleUrls: ['./forgot-login-password.component.css']
})
export class ForgotLoginPasswordComponent implements OnInit {


  // object of forgot-login-password to bind with form
  forgot:forgotLoginPassword = new forgotLoginPassword()


  constructor(private serviceUser:UserService, private router:Router) { }

  ngOnInit(): void {
  }


  otp:number
  obj:any
  // get otp for forgot password
  getOtp(userId:number){
    this.serviceUser.forgotPasswordOtp(userId).subscribe((data)=>{
      console.log(data)
      if(data != -1)
      {
        alert("OTP sent to registered email id")
      }
      this.obj = data
      this.otp = this.obj
      if(this.otp == -1) 
      {
        alert("Invalid userID")
      }
      else if (data == "something went wrong")
      {
        alert("Something went wrong.")
      }
    }, (err)=>{
      console.log(err)
    })
  }


  goToSetNewPassword(otp:number, userId:number){
    if(this.otp == otp)
    {
      sessionStorage.setItem('setPassword', userId.toString())
      this.router.navigate(['set-new-password/', userId])
    } 
    else 
    {
      alert("Invalid OTP")
    }
  }

}
