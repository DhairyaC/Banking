import { RegisterAccount } from './../../shared/models/register-account.model';
import { Component, OnInit } from '@angular/core';
import { AdminService } from 'src/app/shared/services/admin.service';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-account-list',
  templateUrl: './account-list.component.html',
  styleUrls: ['./account-list.component.css']
})
export class AccountListComponent implements OnInit {

  accountList:RegisterAccount = new RegisterAccount()

  constructor(private service:AdminService, private route:ActivatedRoute) { }

  adminId:number 

  ngOnInit(): void {
    this.adminId = this.route.snapshot.params.id
    this.service.adminId = this.adminId
    this.getPendingList(this.adminId)
    
  }

  pendingList:any
  getPendingList(adminId:number){
    this.service.getUnapprovedAccountList(adminId).subscribe((data)=>{
      this.pendingList = data
      console.log(data)
      if(data == "something went wrong")
      {
        alert("Something went wrong.")
      }
    }, (err)=>{
      console.log(err)
    })
  }
}
