import {Component, Input, EventEmitter, OnInit, Output, TemplateRef} from '@angular/core';
import {Transaction} from '../../models/interfaces/transaction';
import {StatusTransaction} from '../../models/enums/status-transaction.enum';
import {TypeTransaction} from '../../models/enums/type-transaction.enum';
import {MtxGridColumn} from '@ng-matero/extensions/grid';

@Component({
  selector: 'app-table-transactions',
  templateUrl: './table-transactions.component.html',
  styleUrls: ['./table-transactions.component.scss']
})
export class TableTransactionsComponent implements OnInit {

  constructor() {
  }

  @Input() transactions: Array<Transaction> = [];
  @Input() paginatorTpl: TemplateRef<any>;
  @Input() toolbarTpl: TemplateRef<any>;
  @Output() delete: EventEmitter<number> = new EventEmitter();
  @Output() edit: EventEmitter<Transaction> = new EventEmitter();
  transactionStatus = StatusTransaction;
  transactionTypes = TypeTransaction;

  /*tableHeaders: MtxGridColumn[] = [
    {header: this.translateService.stream('Id'), field: 'id'},
    {
      header: this.translateService.stream('Status'),
      field: 'status',
      formatter: ((row: Transaction) => StatusTransaction[row.status])
    },
    {
      header: this.translateService.stream('Type'),
      field: 'type',
      formatter: ((row: Transaction) => TypeTransaction[row.type])
    },
    {header: this.translateService.stream('Client name'), field: 'clientName'},
    {header: this.translateService.stream('Amount'), field: 'amount'},
    {
      header: this.translateService.stream('Actions'),
      field: 'actions',
      type: 'button',
      buttons:
        [
          {
            type: 'icon',
            icon: 'edit',
            color: 'accent',
            click: ((record: Transaction) => this.editTransaction(record)),
            tooltip: this.translateService.stream('Edit transaction'),
          },
          {
            type: 'icon',
            icon: 'delete',
            color: 'warn',
            click: ((record: Transaction) => this.deleteTransactionById(record.id)),
            tooltip: this.translateService.stream('Delete transaction'),
          },
        ]
    },
  ];*/
  tableHeaders: MtxGridColumn[] = [
    {header: 'Id', field: 'id'},
    {
      header: 'Status',
      field: 'status',
      formatter: ((row: Transaction) => StatusTransaction[row.status])
    },
    {
      header: 'Type',
      field: 'type',
      formatter: ((row: Transaction) => TypeTransaction[row.type])
    },
    {header: 'Client name', field: 'clientName'},
    {header: 'Amount', field: 'amount', type: 'currency', formatter: ((row: Transaction) => `${row.amount}$`)},
    {
      header: 'Actions',
      field: 'actions',
      type: 'button',
      buttons:
        [
          {
            type: 'icon',
            icon: 'edit',
            color: 'accent',
            click: ((record: Transaction) => this.editTransaction(record)),
            tooltip: 'Edit transaction',
          },
          {
            type: 'icon',
            icon: 'delete',
            color: 'warn',
            click: ((record: Transaction) => this.deleteTransactionById(record.id)),
            tooltip: 'Delete transaction',
          },
        ]
    },
  ];

  ngOnInit(): void {
  }

  editTransaction(transaction: Transaction): void {
    this.edit.emit(transaction);
  }

  deleteTransactionById(id: number): void {
    this.delete.emit(id);
  }

}
