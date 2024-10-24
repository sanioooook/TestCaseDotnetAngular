import {Injectable} from '@angular/core';
import {HttpClient, HttpHeaders, HttpParams} from '@angular/common/http';
import {Transaction} from '../models/interfaces/transaction';
import {Pagination} from '../models/interfaces/pagination';
import {Observable, throwError} from 'rxjs';
import {catchError, map} from 'rxjs/operators';
import {SortBy} from '../models/interfaces/sort-by';
import {environment} from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class TransactionService {

  constructor(private http: HttpClient) {
  }

  url = `${environment.api_url}/Transaction`;

  getTransactions(pagination: Pagination<any>, sortBy: SortBy)
    : Observable<Pagination<Transaction>> {
    let httpParams = new HttpParams()
      .set('pagination.pageNumber', pagination.pageNumber.toString())
      .set('pagination.pageSize', pagination.pageSize.toString());
    if (sortBy.sortStatusBy !== null) {
      httpParams = httpParams.set('sortBy.sortStatusBy', sortBy.sortStatusBy.toString());
    }
    if (sortBy.sortTypeBy !== null) {
      httpParams = httpParams.set('sortBy.sortTypeBy', sortBy.sortTypeBy.toString());
    }
    return this.http.get<Pagination<Transaction>>(this.url, {
      params: httpParams,
      headers: new HttpHeaders()
    })
      .pipe(map(data => data), catchError(err => this.handleError(err)));
  }

  export(sortBy: SortBy): Observable<any> {
    const url = `${this.url}/Export`;
    let httpParams = new HttpParams();
    if (sortBy.sortStatusBy != null) {
      httpParams = httpParams.set('sortBy.sortStatusBy', sortBy.sortStatusBy.toString());
    }
    if (sortBy.sortTypeBy != null) {
      httpParams = httpParams.set('sortBy.sortTypeBy', sortBy.sortTypeBy.toString());
    }
    return this.http.get(url,
      {
        params: httpParams,
        responseType: 'blob'
      })
      .pipe(map(response => response), catchError(err => this.handleError(err)));
  }

  getTransactionById(id: number): Observable<Transaction[]> {
    return this.http.get<Transaction[]>(`${this.url}/${id}`)
      .pipe(map(data => data), catchError(err => this.handleError(err)));
  }

  createTransaction(transaction: Transaction): Observable<number> {
    return this.http.post<number>(this.url, transaction, {headers: new HttpHeaders()})
      .pipe(map(data => data), catchError(err => this.handleError(err)));
  }

  updateTransaction(transaction: Transaction): Observable<any> {
    return this.http.put<any>(
      `${this.url}/${transaction.id}`, transaction, {headers: new HttpHeaders()});
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
