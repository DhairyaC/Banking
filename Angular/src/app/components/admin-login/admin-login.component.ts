import { Component, OnInit } from '@angular/core';
import { FormBuilder, NgForm } from '@angular/forms';
import { Router } from '@angular/router';
import { AdminService } from 'src/app/shared/services/admin.service';

@Component({
  selector: 'app-admin-login',
  templateUrl: './admin-login.component.html',
  styleUrls: ['./admin-login.component.css']
})
export class AdminLoginComponent implements OnInit {

  constructor(private formBuilder:FormBuilder, public router:Router, public service:AdminService) { }

  ngOnInit(): void {
    if(sessionStorage.getItem('admin_id')! !='abc')
    {
      this.router.navigate(["admin-login"])
    }
  }

  adminLogin(loginForm:NgForm){
    // debugger;
    this.service.adminLogin(loginForm.value.adminEmail, loginForm.value.adminPassword).subscribe((data)=>{
      console.log(data)
      if(data == "invalid id"){
        alert("Invalid ID")
      } 
      else if(data == "invalid password")
      {
        alert("Invalid password")
      } 
      else if (data == "something went wrong")
      {
        alert("Somehing went wrong")
      }
      else 
      {
        alert("Successfully Logged in")
        
        sessionStorage.setItem('admin_id', loginForm.value.adminEmail)
        //sessionStorage.setItem('admin-logged-in', "true")
        this.router.navigate([`admin-dashboard/${loginForm.value.adminEmail}`])
      }
    }, (err)=> {
      console.log(err)
      
    })
  }
}
