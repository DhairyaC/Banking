import { UserService } from './../../shared/services/user.service';
import { RegisterAccount } from './../../shared/models/register-account.model';
import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { AdminService } from 'src/app/shared/services/admin.service';
import {Router} from '@angular/router';
import {Location} from '@angular/common';

@Component({
  selector: 'app-cust-details',
  templateUrl: './cust-details.component.html',
  styleUrls: ['./cust-details.component.css']
})
export class CustDetailsComponent implements OnInit {

  constructor(private route:ActivatedRoute, private service:AdminService, private serviceUser:UserService, 
    private router:Router, private _location: Location) { }

  custId:number
  customer:RegisterAccount = new RegisterAccount()
  adminId:number
  
  ngOnInit(): void {
    this.custId = this.route.snapshot.params.id
    this.adminId = this.service.adminId
    this.getAccount()
  }

  custDetails:any
  // get particular account details
  getAccount(){
    this.serviceUser.getUserDetails(this.custId).subscribe((data)=>{
      console.log(data)
      if(data == "something went wrong")
      {
        alert("Something went wrong.")
      }
      this.custDetails = data
    }, (err)=>{
      console.log(err)
    })
  }


  // send approval
  sendApproval(){
    this.service.sendApproval(this.custId).subscribe((data)=>{
      console.log(data)
      if(data == "process completed")
      {
        alert("Approved")      
        this._location.back()
      } 
      else if (data == "something went wrong")
      {
        alert("Something went wrong.")
      }
      else
      {
        alert("Something went wrong. Not Approved.")
      }
    }, (err)=>{
      console.log(err)
      
    })
  }

  // reject approval
  rejectApproval(){
    alert("Are you sure ?")
    this.service.rejectApproval(this.custId).subscribe((data)=>{
      console.log(data)
      if(data == "rejected approval") 
      {
        alert("Rejected")
        this._location.back()
      } 
      else if (data == "something went wrong")
      {
        alert("Something went wrong.")
      }
      else 
      {
        alert("Something went wrong. Not rejected.")
      }
    }, (err)=>{
      console.log(err)
      
      // }
    })
  }
}
