import { StatusTransaction } from '../enums/status-transaction.enum';
import { TypeTransaction } from '../enums/type-transaction.enum';

export interface Pagination<T> {
  data: Array<T>;

  pageNumber: number;

  pageSize: number;

  totalCount: number;

  pageCount: number;
}
