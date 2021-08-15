import { AccountDetails } from './../../shared/models/account-details.model';
import { UserService } from './../../shared/services/user.service';
import { Component, OnInit } from '@angular/core';
import { NgForm } from '@angular/forms';
import { Address } from 'src/app/shared/models/address.model';

@Component({
  selector: 'app-user-account-details',
  templateUrl: './user-account-details.component.html',
  styleUrls: ['./user-account-details.component.css']
})
export class UserAccountDetailsComponent implements OnInit {

  constructor(private serviceUser:UserService) { }

  ngOnInit(): void {
    this.fetchDetails()
    this.fetchAddress()
    console.log(this.details)
  }

  //update address
  updateAddress(line1:string, line2:string, landmark:string, city:string, state:string, pincode:number) {
    
  }

  // submit
  submitUpdate(title:string, fname:string, lname:string, faname:string, mname:string,
    mnum:number, email:string, aadhar:number, pan:string, occtype:string, incsource:string,
    gai:string){
    this.serviceUser.updateBasicDetails(this.custId,title,fname,lname,mname,faname,mnum,email,aadhar,pan,occtype,incsource,gai).subscribe(
      data=>{
        if(data == "updated")
        {
          alert("Details updated")
        } 
        else if (data == "Exception")
        {
          alert("Some fields must be unique. Current field not updated")
        }
        else 
        {
          alert("Customer not found")
        }
      }
    )
  }

  custId:number
  obj:any
  details:AccountDetails = new AccountDetails()
  // fetch details
  fetchDetails(){
    this.serviceUser.fetchUserDetails().subscribe(
      data=>{
        // console.log(data)
        this.obj=data
        this.details=this.obj[0]
        this.custId = this.details.cust_id
      }, err=>{
        console.log(err)
      }
    )
  }


  tmp:any
  address:Address = new Address()
  // fetch address
  fetchAddress(){
    this.serviceUser.getAddress().subscribe(
      data=>{
        console.log(data)
        this.tmp = data
        if(this.tmp.length == 1)
        {
          this.address = this.tmp[0]
        }
        else
        {
          this.address = this.tmp[1]
        }
      }
    )
  }
}
