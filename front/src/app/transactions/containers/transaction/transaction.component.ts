import {Component, OnInit, OnDestroy} from '@angular/core';
import {Pagination} from '../../models/interfaces/pagination';
import {Transaction} from '../../models/interfaces/transaction';
import {
  CreateTransactionComponent,
  EditTransactionComponent,
} from '../../components';
import {StatusTransaction} from '../../models/enums/status-transaction.enum';
import {TypeTransaction} from '../../models/enums/type-transaction.enum';
import {SortBy} from '../../models/interfaces/sort-by';
import {TransactionService} from '../../services/transaction.service';
import {AutoUnsubscribe} from 'ngx-auto-unsubscribe';
import {Subscription} from 'rxjs';
import {MatDialog, MatDialogRef} from '@angular/material/dialog';
import {Overlay} from '@angular/cdk/overlay';
import {dialogConfigHelper} from '../../../helpers';
import {catchError, tap} from 'rxjs/operators';
import {PageEvent} from "@angular/material/paginator";

@AutoUnsubscribe()
@Component({
  selector: 'app-transaction',
  templateUrl: './transaction.component.html',
  styleUrls: ['./transaction.component.scss']
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
  private updateSubscriber: Subscription;
  private createSubsriber: Subscription;

  constructor(
    private transactionService: TransactionService,
    private dialog: MatDialog,
    private overlay: Overlay,
  ) {
    this.paginator = {
      pageNumber: 0,
      pageSize: 10,
      data: [],
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
    } else {
      this.sortBy.sortTypeBy = +e.target.value;
    }
    this.getAllTransactions();
  }

  setSortByStatusTransaction(e: any): void {
    if (e.target.value === 'Status') {
      this.sortBy.sortStatusBy = null;
    } else {
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
    const updateModalRef = this.dialog
      .open(EditTransactionComponent, dialogConfigHelper(this.overlay, transaction));
    this.editSubscriber = updateModalRef.componentInstance
      .saveTransaction.subscribe((model) =>
        this.updateTransaction(model, updateModalRef));
  }

  showModalCreateTransaction(): void {
    const createModalRef = this.dialog
      .open(CreateTransactionComponent, dialogConfigHelper(this.overlay));
    this.createSubscriber = createModalRef.componentInstance
      .createTransaction.subscribe((transaction) =>
        this.createTransaction(transaction, createModalRef));
  }

  pageChanged(event: PageEvent): void {
    this.paginator.pageNumber = event.pageIndex;
    this.getAllTransactions();
  }

  updateTransaction(transaction: Transaction, updateModalRef: MatDialogRef<EditTransactionComponent>): void {
    this.updateSubscriber = this.transactionService.updateTransaction(transaction)
      .pipe(tap(() => {
          this.getAllTransactions();
          updateModalRef.close();
        }),
        catchError((err, caught) => caught))
      .subscribe();
  }

  createTransaction(transaction: Transaction, createModalRef: MatDialogRef<CreateTransactionComponent>): void {
    if (transaction.amount &&
      transaction.clientName &&
      this.transactionStatus[transaction.status] &&
      this.transactionTypes[transaction.type]) {
      this.createSubsriber = this.transactionService.createTransaction(transaction)
        .pipe(tap(() => {
            this.getAllTransactions();
            createModalRef.close();
          }),
          catchError((err, caught) => caught))
        .subscribe();
    }
  }
}
