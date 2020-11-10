import { Component, OnInit } from '@angular/core';
import { BsModalRef } from 'ngx-bootstrap/modal';
import { Transaction } from '../classes/transaction';
import { StatusTransaction } from '../enums/status-transaction.enum';
import { TypeTransaction } from '../enums/type-transaction.enum';
import { TransactionService } from '../services/transaction.service';

@Component({
  selector: 'app-create-transaction',
  templateUrl: './create-transaction.component.html',
  styleUrls: ['./create-transaction.component.css']
})
export class CreateTransactionComponent implements OnInit {

  constructor(private transactionService: TransactionService,
              public bsModalRef: BsModalRef) { }

  transaction = new Transaction();
  public statusesTransaction =
    [
      StatusTransaction.Pending,
      StatusTransaction.Cancelled,
      StatusTransaction.Completed
    ];

  public typesTransaction =
    [
      TypeTransaction.Withdrawal,
      TypeTransaction.Refill
    ];

  ngOnInit(): void {
  }

  getStringStatus(status: number): string {
    return StatusTransaction[status];
  }

  getStringType(type: number): string {
    return TypeTransaction[type];
  }

  public setTypeTransaction(e: any): void {
    this.transaction.type = +e.target.value;
  }

  public setStatusTransaction(e: any): void {
    this.transaction.status = +e.target.value;
  }

  createTransaction(): void {
    if (this.transaction.amount &&
      this.transaction.clientName &&
      this.getStringStatus(this.transaction.status) &&
      this.getStringType(this.transaction.type))
    {
      this.transactionService.createTransaction(this.transaction)
        .subscribe(() => this.bsModalRef.hide());
    }
  }
}
