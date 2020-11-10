import { StatusTransaction } from '../enums/status-transaction.enum';
import { TypeTransaction } from '../enums/type-transaction.enum';

export class Pagination<T> {
  public data: Array<T>;

  public pageNumber: number;

  public pageSize: number;

  public totalCount: number;

  public pageCount: number;

  public sortStatusBy: StatusTransaction;

  public sortTypeBy: TypeTransaction;
}
