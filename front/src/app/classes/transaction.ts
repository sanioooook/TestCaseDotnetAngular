import { StatusTransaction } from '../enums/status-transaction.enum';
import { TypeTransaction } from '../enums/type-transaction.enum';

export class Transaction {
  public id: number;

  public status: StatusTransaction;

  public type: TypeTransaction;

  public clientName: string;

  public amount: number;
}

