import { UserService } from './../../shared/services/user.service';
import { trackStatus } from './../../shared/models/track-status.model';
import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})
export class HomeComponent implements OnInit {


  // object of track status to bind with form
  track:trackStatus = new trackStatus()
  

  constructor(private serviceUser:UserService) { }

  ngOnInit(): void {
  }

  
  obj:any
  // otp to track status by srn
  getOtp(srn:number){
    this.serviceUser.trackStatus(srn).subscribe((data)=>{
      this.obj=data
      if(this.obj == "Rejected") 
      {
        alert("Rejected. Please apply again.")
      }
      if(this.obj == "invalid srn") 
      {
        alert("Invalid SRN")
      }
      else if(this.obj == "something went wrong")
      {
        alert("Something went wrong.")
      } 
      else if(this.obj == "error in sending mail")
      {
        alert("Error in sending mail")
      } 
      else {
        alert("OTP Sent on registered email.")
      }
    })
  }

  

  // check status
  trackStatus(otp:number){
    if(this.obj.otp == otp) 
    {
      alert(`Status : ${this.obj.status}`)
    } 
    else 
    {
      alert("Invalid OTP")
    }
  }

}
