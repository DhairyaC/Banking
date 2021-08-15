import { UserService } from './../../shared/services/user.service';
import { forgotUserId } from './../../shared/models/forgot-id.model';
import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';

@Component({
  selector: 'app-forgot-user-id',
  templateUrl: './forgot-user-id.component.html',
  styleUrls: ['./forgot-user-id.component.css']
})
export class ForgotUserIdComponent implements OnInit {


  // object of forgotUserId to bind with form
  forgot:forgotUserId = new forgotUserId()


  constructor(private serviceUser:UserService, private router:Router) { }

  ngOnInit(): void {
  }

  obj:any
  otp:number
  getOtp(ac:number){
    this.serviceUser.forgotIdOtp(ac).subscribe((data)=>{
      if(data == "wrong account number")
      {
        alert("Wrong account number")
      } 
      else 
      {
        alert("OTP sent to registered email id")
        this.obj = data
        this.otp = this.obj.otp
      }
    })
  }

  getUserIdByMail(ac:number, otp:number){
    if(this.otp == otp) {
      this.serviceUser.receiveUserId(ac).subscribe((data)=>{
        console.log(data)
        alert("Email sent with UserID")
        if(data == "userid recoverd")
        {
          alert("Please login")
          this.router.navigate(['login'])
        } 
        else 
        {
          alert("Something went wrong.")
          this.router.navigate(['login'])
        }
      }, (err=>{
        
      }))
    } else 
    {
      alert("Invalid OTP")
    }
    
  }

}
