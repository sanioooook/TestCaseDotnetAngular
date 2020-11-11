import { Component, OnInit } from '@angular/core';
import { BsModalService } from 'ngx-bootstrap/modal';
import { Pagination } from '../classes/pagination';
import { Transaction } from '../classes/transaction';
import { CreateTransactionComponent } from '../create-transaction/create-transaction.component';
import { EditTransactionComponent } from '../edit-transaction/edit-transaction.component';
import { StatusTransaction } from '../enums/status-transaction.enum';
import { TypeTransaction } from '../enums/type-transaction.enum';
import { TransactionService } from '../services/transaction.service';

@Component({
  selector: 'app-transaction',
  templateUrl: './transaction.component.html',
  styleUrls: ['./transaction.component.css']
})
export class TransactionComponent implements OnInit {

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
  public paginator = new Pagination<Transaction>();
  constructor(private transactionService: TransactionService,
              private modalService: BsModalService) { }

  ngOnInit(): void {
    this.paginator.pageNumber = 0;
    this.paginator.pageSize = 10;
    this.paginator.sortStatusBy = null;
    this.paginator.sortTypeBy = null;
    this.getAllTransactions();
  }

  public getAllTransactions(): void {
    this.transactionService.getTransactions(this.paginator).subscribe(
      data => {
        this.paginator.data = data.data;
        this.paginator.pageCount = data.pageCount;
        this.paginator.pageNumber = data.pageNumber;
        this.paginator.pageSize = data.pageSize;
        this.paginator.totalCount = data.totalCount;
      }
    );
  }

  public getStringStatus(status: number): string {
    return StatusTransaction[status];
  }

  public getStringType(type: number): string {
    return TypeTransaction[type];
  }

  public setSortByTypeTransaction(e: any): void {
    if (e.target.value === 'Type') {
      this.paginator.sortTypeBy = null;
    }
    else{
      this.paginator.sortTypeBy = +e.target.value;
    }
    this.getAllTransactions();
  }

  public setSortByStatusTransaction(e: any): void {
    if (e.target.value === 'Status') {
      this.paginator.sortStatusBy = null;
    }
    else{
      this.paginator.sortStatusBy = +e.target.value;
    }
    this.getAllTransactions();
  }

  public exportToCSV(): void {
    this.transactionService.getTransactionsFile(
      this.paginator.sortTypeBy, this.paginator.sortStatusBy)
      .subscribe((data: string) => {
        if (data) {
          const link = document.createElement('a');
          link.download = 'data.csv';
          const blob = new Blob([data], { type: 'text/csv' });
          link.href = window.URL.createObjectURL(blob);
          link.click();
        }
      });
  }

  public importCSV(): void {
    const input = document.createElement('input');
    input.type = 'file';
    input.accept = '.csv'; // supported file types
    input.onchange = (ev: Event) => {
      const file = (ev.target as HTMLInputElement).files[0];
      const formData = new FormData();
      formData.append('file', file);
      this.transactionService.import(formData)
        .subscribe(() => this.getAllTransactions());
    };
    input.click();
  }

  public deleteTransactionById(transactionId: number): void {
    if (confirm(`Do you really want to delete this entry? (Id=${transactionId})`)) {
      this.transactionService.deleteTransactionById(transactionId)
        .subscribe(() => this.getAllTransactions());
    }
  }

  public showModalEditTransaction(transaction: Transaction): void {
    this.modalService.show(EditTransactionComponent,
      { initialState: { transaction } }).onHide.subscribe(() => this.getAllTransactions());
  }

  public showModalCreateTransaction(): void {
    this.modalService.show(CreateTransactionComponent)
      .onHide.subscribe(() => this.getAllTransactions());
  }

  pageChanged(event: any): void {
    this.paginator.pageNumber = event.page - 1;
    this.getAllTransactions();
  }
}
