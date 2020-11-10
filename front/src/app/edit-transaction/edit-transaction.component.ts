import { Component, OnInit } from '@angular/core';
import { BsModalRef } from 'ngx-bootstrap/modal';
import { Transaction } from '../classes/transaction';
import { StatusTransaction } from '../enums/status-transaction.enum';
import { TypeTransaction } from '../enums/type-transaction.enum';
import { TransactionService } from '../services/transaction.service';

@Component({
  selector: 'app-edit-transaction',
  templateUrl: './edit-transaction.component.html',
  styleUrls: ['./edit-transaction.component.css']
})
export class EditTransactionComponent implements OnInit {

  constructor(private transactionService: TransactionService,
              public bsModalRef: BsModalRef) { }

  transaction: Transaction;
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

  saveTransaction(): void {
    this.transactionService.updateTransaction(this.transaction)
      .subscribe(() => this.bsModalRef.hide());
  }

}
