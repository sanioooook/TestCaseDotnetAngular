import {Component, OnInit, OnDestroy, EventEmitter} from '@angular/core';
import {Transaction} from '../../models/interfaces/transaction';
import {StatusTransaction} from '../../models/enums/status-transaction.enum';
import {TypeTransaction} from '../../models/enums/type-transaction.enum';
import {MatDialogRef} from '@angular/material/dialog';

@Component({
  selector: 'app-create-transaction',
  templateUrl: './create-transaction.component.html',
  styleUrls: ['./create-transaction.component.scss']
})
export class CreateTransactionComponent implements OnInit, OnDestroy {
  createTransaction: EventEmitter<Transaction> = new EventEmitter<Transaction>();

  constructor(public dialogRef: MatDialogRef<CreateTransactionComponent>) {
  }

  transaction: Transaction;
  transactionStatus = StatusTransaction;
  transactionTypes = TypeTransaction;

  ngOnDestroy(): void {
  }

  ngOnInit(): void {
    this.transaction = {amount: 0.0, clientName: '', id: 0, status: null, type: null};
  }

  onCreateTransaction(): void {
    this.createTransaction.emit(this.transaction);
  }

  close(result = false): void {
    this.dialogRef.close(result);
  }
}
