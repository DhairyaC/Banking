import { Component, OnInit } from '@angular/core';
import { MatDialog, MatDialogRef } from '@angular/material/dialog';

@Component({
  selector: 'app-transaction-receipt',
  templateUrl: './transaction-receipt.component.html',
  styleUrls: ['./transaction-receipt.component.css']
})
export class TransactionReceiptComponent implements OnInit {

  constructor(public dialog: MatDialog, public dialogRef: MatDialogRef<TransactionReceiptComponent>) { }

  ngOnInit(): void {
    console.log(JSON.parse(localStorage.getItem("transactionR")!));
  }

  Fund:any=JSON.parse(localStorage.getItem("transactionR")!);
  
  close(): void {
    this.dialogRef.close();
  }
}
