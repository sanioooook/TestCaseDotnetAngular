import { Component, OnInit, OnDestroy } from '@angular/core';
import { BsModalService } from 'ngx-bootstrap/modal';
import { Pagination } from '../interfaces/pagination';
import { Transaction } from '../classes/transaction';
import { CreateTransactionComponent } from '../create-transaction/create-transaction.component';
import { EditTransactionComponent } from '../edit-transaction/edit-transaction.component';
import { StatusTransaction } from '../enums/status-transaction.enum';
import { TypeTransaction } from '../enums/type-transaction.enum';
import { SortBy } from '../interfaces/sort-by';
import { TransactionService } from '../services/transaction.service';
import { AutoUnsubscribe } from 'ngx-auto-unsubscribe';
import { Subscription } from 'rxjs';

@AutoUnsubscribe()
@Component({
  selector: 'app-transaction',
  templateUrl: './transaction.component.html',
  styleUrls: ['./transaction.component.css']
})
export class TransactionComponent implements OnInit, OnDestroy {

  transactionStatus = StatusTransaction;
  transactionTypes = TypeTransaction;
  paginator: Pagination<Transaction>;
  sortBy: SortBy;
  private getAllTransactionsSubscriber: Subscription;
  private exportSubscriber: Subscription;
  private importSubscriber: Subscription;
  private deleteSubscriber: Subscription;
  private editSubscriber: Subscription;
  private createSubscriber: Subscription;

  constructor(private transactionService: TransactionService,
              private modalService: BsModalService) {
    this.paginator = {
      pageNumber: 0,
      pageSize: 10,
      data: null,
      pageCount: 0,
      totalCount: 0
    };
    this.sortBy = {
      sortStatusBy: null,
      sortTypeBy: null
    };
  }
  ngOnDestroy(): void {

  }

  ngOnInit(): void {
    this.getAllTransactions();
  }

  getAllTransactions(): void {
    this.getAllTransactionsSubscriber =
      this.transactionService.getTransactions(this.paginator, this.sortBy).subscribe(
      data => {
        this.paginator.data = data.data;
        this.paginator.pageCount = data.pageCount;
        this.paginator.pageNumber = data.pageNumber;
        this.paginator.pageSize = data.pageSize;
        this.paginator.totalCount = data.totalCount;
      }
    );
  }

  setSortByTypeTransaction(e: any): void {
    if (e.target.value === 'Type') {
      this.sortBy.sortTypeBy = null;
    }
    else{
      this.sortBy.sortTypeBy = +e.target.value;
    }
    this.getAllTransactions();
  }

  setSortByStatusTransaction(e: any): void {
    if (e.target.value === 'Status') {
      this.sortBy.sortStatusBy = null;
    }
    else{
      this.sortBy.sortStatusBy = +e.target.value;
    }
    this.getAllTransactions();
  }

  exportToCSV(): void {
    this.exportSubscriber = this.transactionService.export(this.sortBy)
      .subscribe((data: Blob) => {
        if (data) {
          const link = document.createElement('a');
          link.download = 'data.xlsx';
          link.href = window.URL.createObjectURL(new Blob([data]));
          link.click();
          link.remove();
        }
      });
  }

  importCSV(): void {
    const input = document.createElement('input');
    input.type = 'file';
    input.accept = '.csv'; // supported file types
    input.onchange = (ev: Event) => {
      const file = (ev.target as HTMLInputElement).files[0];
      (ev.target as HTMLInputElement).remove();
      const formData = new FormData();
      formData.append('file', file);
      this.importSubscriber = this.transactionService.import(formData)
        .subscribe(() => this.getAllTransactions());
    };
    input.click();
    input.remove();
  }

  deleteTransactionById(transactionId: number): void {
    if (confirm(`Do you really want to delete this entry? (Id=${transactionId})`)) {
      this.deleteSubscriber = this.transactionService.deleteTransactionById(transactionId)
        .subscribe(() => this.getAllTransactions());
    }
  }

  showModalEditTransaction(transaction: Transaction): void {
    this.editSubscriber = this.modalService.show(EditTransactionComponent,
      { initialState: { transaction } }).onHide.subscribe(() => this.getAllTransactions());
  }

  showModalCreateTransaction(): void {
    this.createSubscriber = this.modalService.show(CreateTransactionComponent)
      .onHide.subscribe(() => this.getAllTransactions());
  }

  pageChanged(event: any): void {
    this.paginator.pageNumber = event.page - 1;
    this.getAllTransactions();
  }
}
