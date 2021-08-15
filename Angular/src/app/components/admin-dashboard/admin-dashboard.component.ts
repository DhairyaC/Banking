import { ActivatedRoute, Router } from '@angular/router';
import { Component, OnInit } from '@angular/core';
import { AdminService } from 'src/app/shared/services/admin.service';

@Component({
  selector: 'app-admin-dashboard',
  templateUrl: './admin-dashboard.component.html',
  styleUrls: ['./admin-dashboard.component.css']
})
export class AdminDashboardComponent implements OnInit {


  adminId:number
  
  constructor(private router:Router, private service:AdminService, private route:ActivatedRoute) { }

  //adminId:number

  ngOnInit(): void {
    this.adminId = this.route.snapshot.params.id

    if(sessionStorage.getItem('admin_id')! !=this.adminId.toString())
    {
      this.router.navigate(["admin-login"])
    }
  }

  

  //logout
  adminLogout(){
    alert("Logged Out Successfully.")
    
    sessionStorage.clear()
    this.router.navigate(['admin-login'])
  }

}
