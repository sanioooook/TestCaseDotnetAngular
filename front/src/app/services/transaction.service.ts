import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders, HttpParams, HttpRequest } from '@angular/common/http';
import { Transaction } from '../classes/transaction';
import { Pagination } from '../classes/pagination';
import { Observable, throwError } from 'rxjs';
import { catchError, map } from 'rxjs/operators';
import { TypeTransaction } from '../enums/type-transaction.enum';
import { StatusTransaction } from '../enums/status-transaction.enum';


@Injectable({
  providedIn: 'root'
})
export class TransactionService {

  constructor(private http: HttpClient) {
  }

  url = 'https://localhost:5001/api/Transaction';

  getTransactions(pagination: Pagination<any>): Observable<Pagination<Transaction>> {
    let httpParams = new HttpParams()
      .set('pageNumber', pagination.pageNumber.toString())
      .set('pageSize', pagination.pageSize.toString());
    if (pagination.sortStatusBy !== null) {
      httpParams = httpParams.set('sortStatusBy', pagination.sortStatusBy.toString());
    }
    if (pagination.sortTypeBy !== null) {
      httpParams = httpParams.set('sortTypeBy', pagination.sortTypeBy.toString());
    }
    return this.http.get<Pagination<Transaction>>(this.url, {
      params: httpParams
    })
      .pipe(map(data => data), catchError(err => this.handleError(err)));
  }

  getTransactionsFile(
    sortTypeTransaction: TypeTransaction,
    sortStatusTransaction: StatusTransaction): Observable<any> {
    let httpParams = new HttpParams();
    const url = `${this.url}/Export`;
    if (sortStatusTransaction !== null) {
      httpParams = httpParams.set('sortStatusBy', sortStatusTransaction.toString());
    }
    if (sortTypeTransaction !== null) {
      httpParams = httpParams.set('sortTypeBy', sortTypeTransaction.toString());
    }
    const httpRequest = new HttpRequest<string>('GET', url, {
      responseType: 'text',
      params: httpParams
    });
    return this.http.request<string>(httpRequest)
      .pipe(map(response => {
        if (response.status === 200) {
          return response.body;
        }
      }), catchError(err => this.handleError(err)));
  }

  getTransactionById(id: number): Observable<Transaction[]> {
    return this.http.get<Transaction[]>(`${this.url}/${id}`)
      .pipe(map(data => data), catchError(err => this.handleError(err)));
  }

  createTransaction(transaction: Transaction): Observable<number> {
    return this.http.post<number>(this.url, transaction)
      .pipe(map(data => data), catchError(err => this.handleError(err)));
  }

  updateTransaction(transaction: Transaction): Observable<any> {
    return this.http.put<any>(`${this.url}/${transaction.id}`, transaction);
  }

  deleteTransactionById(transactionId: number): Observable<any> {
    return this.http.delete<any>(`${this.url}/${transactionId}`);
  }

  import(fileImport: FormData): Observable<any> {
    // const httpHeaders = new HttpHeaders().set('content-type', 'multipart/form-data');
    return this.http.post<any>(`${this.url}/Import`, fileImport);
  }

  private handleError(error: Response): Observable<never> {
    console.error(error);
    return throwError(error || 'Server error');
  }

}
