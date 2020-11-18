import { StatusTransaction } from '../enums/status-transaction.enum';
import { TypeTransaction } from '../enums/type-transaction.enum';

export interface SortBy {
  sortStatusBy: StatusTransaction;
  sortTypeBy: TypeTransaction;
}
