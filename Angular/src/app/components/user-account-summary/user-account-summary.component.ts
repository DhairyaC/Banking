import { UserService } from './../../shared/services/user.service';
import { ActivatedRoute } from '@angular/router';
import { Component, OnInit } from '@angular/core';
import { Transaction } from 'src/app/shared/models/transaction.model';

@Component({
  selector: 'app-user-account-summary',
  templateUrl: './user-account-summary.component.html',
  styleUrls: ['./user-account-summary.component.css']
})
export class UserAccountSummaryComponent implements OnInit {


  // list to hold recent transaction and loop over
  recent:Transaction[] = []

  constructor(private route:ActivatedRoute, private serviceUser:UserService) { }

  userId:number
  ngOnInit(): void {
    // this.recent = this.getTransactionList()
    this.userId = this.route.snapshot.params.id
    this.getAccountSummary()
    this.serviceUser.userId = this.userId
    console.log(this.serviceUser.userId)
  }

  accNo:number
  bal:number
  trList:any
  obj:any
  
  // get list of top 5 transaction
  
  getAccountSummary(){
    this.serviceUser.accountSummary(this.userId).subscribe((data)=>{
      console.log(data)
      this.obj = data
      this.accNo = this.obj.accountnumber
      this.bal = this.obj.balance
      this.trList = this.obj.transactions
    }, (err)=>{
      console.log(err)
    })
  }

}
