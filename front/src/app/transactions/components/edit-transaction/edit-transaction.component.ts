import { Component, OnInit, OnDestroy } from '@angular/core';
import { AutoUnsubscribe } from 'ngx-auto-unsubscribe';
import { BsModalRef } from 'ngx-bootstrap/modal';
import { Subscription } from 'rxjs';
import { Transaction } from '../../models/interfaces/transaction';
import { StatusTransaction } from '../../models/enums/status-transaction.enum';
import { TypeTransaction } from '../../models/enums/type-transaction.enum';
import { TransactionService } from '../../services/transaction.service';

@AutoUnsubscribe()
@Component({
  selector: 'app-edit-transaction',
  templateUrl: './edit-transaction.component.html',
  styleUrls: ['./edit-transaction.component.css']
})
export class EditTransactionComponent implements OnInit, OnDestroy {

  constructor(private transactionService: TransactionService,
              public bsModalRef: BsModalRef) { }

  transaction: Transaction;
  transactionStatus = StatusTransaction;
  transactionTypes = TypeTransaction;

  ngOnDestroy(): void {
  }

  ngOnInit(): void {
  }

  public setTypeTransaction(e: any): void {
    this.transaction.type = +e.target.value;
  }

  public setStatusTransaction(e: any): void {
    this.transaction.status = +e.target.value;
  }

  saveTransaction(): void {
    this.bsModalRef.hide();
  }

}