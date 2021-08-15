import { UserService } from './../../shared/services/user.service';
import { ActivatedRoute, Router } from '@angular/router';
import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-user-dashboard',
  templateUrl: './user-dashboard.component.html',
  styleUrls: ['./user-dashboard.component.css']
})
export class UserDashboardComponent implements OnInit {

  constructor(private route:ActivatedRoute, private serviceUser:UserService, private router:Router) { }

  lastLogin: string
  userId:number
  ngOnInit(): void {
    this.userId = this.route.snapshot.params.id
    this.getName(this.userId)
    console.log(this.userId)
    if(sessionStorage.getItem('user_id')! !=this.userId.toString())
    {
      this.router.navigate(['login'])
    }
    this.lastLogin = sessionStorage.getItem('lastlogin')!
  }

  nameOnDashboard:string
  obj:any
  // name on dashboard
  getName(userId:number){
    this.serviceUser.getName(userId).subscribe(
      data=>{
        console.log(data)
        this.obj = data
        console.log(this.obj.fname)
        this.nameOnDashboard=this.obj
      }
    )
  }

  logoutTime = new Date()
  //logout
  logout()
  {
    sessionStorage.clear()
    this.router.navigate(['login'])
    sessionStorage.setItem('lastLogin', this.logoutTime.toString())

  }
}
