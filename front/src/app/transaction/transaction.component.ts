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
    input.accept = 'text/csv'; // supported file types
    input.onchange = (ev: Event) => {
      const file = (ev.target as HTMLInputElement).files[0];
      const reader = new FileReader();
      reader.readAsText(file);
      reader.onload = () => {
        const arrayString = this.CSVToArray(reader.result as string, ',');
        const arrayTransaction = this.arrayStringToArrayTransaction(arrayString);
        if (arrayTransaction && arrayTransaction.length > 0) {
          this.transactionService.import(arrayTransaction)
            .subscribe(() => this.getAllTransactions());
        }
      };

      reader.onerror = () => {
        console.error(reader.error);
      };
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

  public showModalCreateTransaction(transaction: Transaction): void {
    this.modalService.show(CreateTransactionComponent)
      .onHide.subscribe(() => this.getAllTransactions());
  }

  private CSVToArray(strData: string, strDelimiter: string): any[][] {
    // Check to see if the delimiter is defined. If not,
    // then default to comma.
    strDelimiter = (strDelimiter || ',');

    // Create a regular expression to parse the CSV values.
    const objPattern = new RegExp(
      (
        // Delimiters.
        '(\\' + strDelimiter + '|\\r?\\n|\\r|^)' +
        // Quoted fields.
        '(?:"([^"]*(?:""[^"]*)*)"|' +
        // Standard fields.
        '([^"\\' + strDelimiter + '\\r\\n]*))'
      ),
      'gi'
    );

    // Create an array to hold our data. Give the array
    // a default empty first row.
    const arrData = [[]];

    // Create an array to hold our individual pattern
    // matching groups.
    let arrMatches = null;

    // Keep looping over the regular expression matches
    // until we can no longer find a match.
    // tslint:disable-next-line: no-conditional-assignment
    while (arrMatches = objPattern.exec(strData)) {

      // Get the delimiter that was found.
      const strMatchedDelimiter = arrMatches[1];

      // Check to see if the given delimiter has a length
      // (is not the start of string) and if it matches
      // field delimiter. If id does not, then we know
      // that this delimiter is a row delimiter.
      if (
        strMatchedDelimiter.length &&
        strMatchedDelimiter !== strDelimiter
      ) {

        // Since we have reached a new row of data,
        // add an empty row to our data array.
        arrData.push([]);

      }

      let strMatchedValue: any;

      // Now that we have our delimiter out of the way,
      // let's check to see which kind of value we
      // captured (quoted or unquoted).
      if (arrMatches[2]) {

        // We found a quoted value. When we capture
        // this value, unescape any double quotes.
        strMatchedValue = arrMatches[2].replace(
          new RegExp('""', 'g'),
          '"'
        );

      } else {

        // We found a non-quoted value.
        strMatchedValue = arrMatches[3];

      }


      // Now that we have our value string, let's add
      // it to the data array.
      arrData[arrData.length - 1].push(strMatchedValue);
    }

    // Return the parsed data.
    return (arrData);
  }

  private arrayStringToArrayTransaction(arayString: any[][]): Array<Transaction> {
    const arrayTransaction = new Array<Transaction>();
    const headers = arayString[0];
    arayString.splice(0, 1);
    arayString.forEach((arr: Array<string>) => {
      const transaction = new Transaction();
      for (let i = 0; i < arr.length; i++) {
        switch (headers[i]) {
          case 'Status': {
            transaction.status = StatusTransaction[arr[i]];
            break;
          }
          case 'Type': {
            transaction.type = TypeTransaction[arr[i]];
            break;
          }
          case 'ClientName': {
            transaction.clientName = arr[i];
            break;
          }
          case 'Amount': {
            transaction.amount = +arr[i].replace('$', '');
            break;
          }
          default:
            break;
        }
      }
      if (transaction.amount &&
        transaction.clientName &&
        this.getStringStatus(transaction.status) &&
        this.getStringType(transaction.type)) {
        arrayTransaction.push(transaction);
      }
    });
    return arrayTransaction;
  }

  pageChanged(event: any): void {
    this.paginator.pageNumber = event.page - 1;
    this.getAllTransactions();
  }
}
