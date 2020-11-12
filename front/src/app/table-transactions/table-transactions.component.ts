import { Component, Input, EventEmitter, OnInit, Output } from '@angular/core';
import { Transaction } from '../classes/transaction';
import { StatusTransaction } from '../enums/status-transaction.enum';
import { TypeTransaction } from '../enums/type-transaction.enum';

@Component({
  selector: 'app-table-transactions',
  templateUrl: './table-transactions.component.html',
  styleUrls: ['./table-transactions.component.css']
})
export class TableTransactionsComponent implements OnInit {

  constructor() { }
  @Input() transactions: Array<Transaction>;
  @Output() delete: EventEmitter<number> = new EventEmitter();
  @Output() edit: EventEmitter<Transaction> = new EventEmitter();
  transactionStatus = StatusTransaction;
  transactionTypes = TypeTransaction;

  ngOnInit(): void {
  }

  editTransaction(transaction: Transaction): void {
    this.edit.emit(transaction);
  }

  deleteTransactionById(id: number): void {
    this.delete.emit(id);
  }

}
