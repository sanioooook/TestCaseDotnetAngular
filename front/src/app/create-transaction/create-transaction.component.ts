import { Component, OnInit, OnDestroy } from '@angular/core';
import { AutoUnsubscribe } from 'ngx-auto-unsubscribe';
import { BsModalRef } from 'ngx-bootstrap/modal';
import { Subscription } from 'rxjs';
import { Transaction } from '../classes/transaction';
import { StatusTransaction } from '../enums/status-transaction.enum';
import { TypeTransaction } from '../enums/type-transaction.enum';
import { TransactionService } from '../services/transaction.service';

@AutoUnsubscribe()
@Component({
  selector: 'app-create-transaction',
  templateUrl: './create-transaction.component.html',
  styleUrls: ['./create-transaction.component.css']
})
export class CreateTransactionComponent implements OnInit, OnDestroy {

  constructor(private transactionService: TransactionService,
              public bsModalRef: BsModalRef) { }

  transaction = new Transaction();
  transactionStatus = StatusTransaction;
  transactionTypes = TypeTransaction;
  private createSubsriber: Subscription;

  ngOnDestroy(): void {

  }

  ngOnInit(): void {
  }
  setTypeTransaction(e: any): void {
    this.transaction.type = +e.target.value;
  }

  setStatusTransaction(e: any): void {
    this.transaction.status = +e.target.value;
  }

  createTransaction(): void {
    if (this.transaction.amount &&
      this.transaction.clientName &&
      this.transactionStatus[this.transaction.status] &&
      this.transactionTypes[this.transaction.type])
    {
      this.createSubsriber = this.transactionService.createTransaction(this.transaction)
        .subscribe(() => this.bsModalRef.hide());
    }
  }
}
