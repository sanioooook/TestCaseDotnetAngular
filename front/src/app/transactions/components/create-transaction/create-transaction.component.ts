import { Component, OnInit, OnDestroy } from '@angular/core';
import { BsModalRef } from 'ngx-bootstrap/modal';
import { Transaction } from '../../models/interfaces/transaction';
import { StatusTransaction } from '../../models/enums/status-transaction.enum';
import { TypeTransaction } from '../../models/enums/type-transaction.enum';
import { TransactionService } from '../../services/transaction.service';

@Component({
  selector: 'app-create-transaction',
  templateUrl: './create-transaction.component.html',
  styleUrls: ['./create-transaction.component.css']
})
export class CreateTransactionComponent implements OnInit, OnDestroy {

  constructor(private transactionService: TransactionService,
              public bsModalRef: BsModalRef) { }

  transaction = { amount: 0.0, clientName: '', id: 0, status: null, type: null } as Transaction;
  transactionStatus = StatusTransaction;
  transactionTypes = TypeTransaction;

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
    this.bsModalRef.hide();
  }
}
