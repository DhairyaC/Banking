import { UserService } from './../../shared/services/user.service';
import { ActivatedRoute, Router } from '@angular/router';
import { setPassword } from './../../shared/models/set-password.model';
import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-set-password',
  templateUrl: './set-password.component.html',
  styleUrls: ['./set-password.component.css']
})
export class SetPasswordComponent implements OnInit {


  // object to be binded with set new password form
  set:setPassword = new setPassword()


  constructor(private route:ActivatedRoute, private serviceUser:UserService, private router:Router) { }

  userId:number
  ngOnInit(): void {
    this.userId = this.route.snapshot.params.id
    let value = sessionStorage.getItem('setPassword')
    if(Number(value) != this.userId)
    {
      this.router.navigate(['home'])
    }
  }


  // set password
  setPassword(p1:string, p2:string){
    this.serviceUser.setNewLoginPassword(this.userId, p1, p2).subscribe((data)=>{
      console.log(data)
      if(data == "set")
      {
        alert("Password updated")
        this.router.navigate(['login'])
      } 
      else 
      {
        alert("Please try again")
      }
    }, (err)=>{
      console.log(err)
  
    })
  }

}
