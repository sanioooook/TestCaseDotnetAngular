import {Component, OnInit, OnDestroy, Inject, EventEmitter} from '@angular/core';
import {AutoUnsubscribe} from 'ngx-auto-unsubscribe';
import {Transaction} from '../../models/interfaces/transaction';
import {StatusTransaction} from '../../models/enums/status-transaction.enum';
import {TypeTransaction} from '../../models/enums/type-transaction.enum';
import {MAT_DIALOG_DATA, MatDialogRef} from '@angular/material/dialog';

@AutoUnsubscribe()
@Component({
  selector: 'app-edit-transaction',
  templateUrl: './edit-transaction.component.html',
  styleUrls: ['./edit-transaction.component.scss']
})
export class EditTransactionComponent implements OnInit, OnDestroy {
  saveTransaction: EventEmitter<Transaction> = new EventEmitter<Transaction>();

  constructor(
    public dialogRef: MatDialogRef<EditTransactionComponent>,
    @Inject(MAT_DIALOG_DATA) public transaction: Transaction) {
  }

  transactionStatus = StatusTransaction;
  transactionTypes = TypeTransaction;

  ngOnDestroy(): void {
  }

  ngOnInit(): void {
  }

  onSaveTransaction(): void {
    this.saveTransaction.emit(this.transaction);
  }

  close(result = false): void {
    this.dialogRef.close(result);
  }
}
