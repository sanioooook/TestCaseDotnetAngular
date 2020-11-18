import { StatusTransaction } from '../enums/status-transaction.enum';
import { TypeTransaction } from '../enums/type-transaction.enum';

export interface Transaction {
  id: number;

  status: StatusTransaction;

  type: TypeTransaction;

  clientName: string;

  amount: number;
}

