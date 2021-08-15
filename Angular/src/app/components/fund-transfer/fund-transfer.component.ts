import { UserService } from './../../shared/services/user.service';
import { Component, OnInit } from '@angular/core';
import { NgForm } from '@angular/forms';
import { FundTransfer } from 'src/app/shared/models/fund-transfer.model';
import {MatDialog, MatDialogRef, MAT_DIALOG_DATA} from '@angular/material/dialog';
import { TransactionReceiptComponent } from './transaction-receipt/transaction-receipt.component';
import { Transaction } from 'src/app/shared/models/transaction.model';

@Component({
  selector: 'app-fund-transfer',
  templateUrl: './fund-transfer.component.html',
  styleUrls: ['./fund-transfer.component.css']
})
export class FundTransferComponent implements OnInit {


  // object to be binded with form of fund transfer
  fund:FundTransfer = new FundTransfer()

  constructor(private serviceUser:UserService, public dialog:MatDialog) { }

  // store list of beneficiaries
  bList:any

  ngOnInit(): void {
    this.getTrList()
  }

  transactionReceipt:any = {}


  // submit transfer form
  submitTransferForm(){
    // console.log(amount)
    // console.log(from)
    // console.log(mode)
    // console.log(remarks)
    this.serviceUser.fundTransfer(this.fund.Amount, this.fund.ToAccount, this.fund.Mode, this.fund.Remark).subscribe(
      data=>{
        console.log(data)
        this.transactionReceipt = data

        localStorage.setItem("transactionR",JSON.stringify(this.transactionReceipt));

        // if(data == "done")
        // {
        //   alert("Transferred successfully")
        // } 
        // else 
        if(data == "insufficient funds")
        {
          alert("Insufficient funds")
        }
        else {
          this.dialog.open(TransactionReceiptComponent,{panelClass: 'add-record-password-container'});
        }
        
      },
      err=>{
        console.log(err)
        // if(err.error.text == "done"){
        //   alert("Transferred successfully")
        // } else if(err.error.text == "insufficient funds"){
        //   alert("Insufficient funds")
        // }  
      }
    )
  } 


  resetFields(){
    
  }

  // get list of transaction
  getTrList(){
    this.serviceUser.getTransactionList().subscribe((data)=>{
      console.log(data)
      this.bList = data
      console.log(this.bList)
    }, (err)=>{
      console.log(err)
    })
  }
}
