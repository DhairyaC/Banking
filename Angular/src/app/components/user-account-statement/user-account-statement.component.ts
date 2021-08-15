import { TrList } from './../../../../trList.model';
import { Statement } from './../../shared/models/statement.model';
import { UserService } from './../../shared/services/user.service';
import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-user-account-statement',
  templateUrl: './user-account-statement.component.html',
  styleUrls: ['./user-account-statement.component.css']
})
export class UserAccountStatementComponent implements OnInit {

  constructor(private serviceUser:UserService) { }

  userId:number

  formStatement:Statement = new Statement()

  ngOnInit(): void {
    this.userId = this.serviceUser.userId
    this.getInfo()
  }

  info:any
  getInfo(){
    this.serviceUser.getInfo().subscribe(
      data=>{
        console.log(data)
        this.info=data
      }
    )
  }

  ac:number
  obj:any
  trList:TrList[]=[]
  fetchTransactions(from:string, to:string){
    console.log(from);
    this.ac = this.info.acno
    this.serviceUser.getTrRange(this.ac,from,to).subscribe(
      data=>{
        console.log(data)
        this.obj=data
        this.trList=this.obj
      }
    )
  }
}
