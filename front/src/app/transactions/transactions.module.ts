import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { PaginationModule } from 'ngx-bootstrap/pagination';
import { ModalModule } from 'ngx-bootstrap/modal';
import { CommonModule } from '@angular/common';

import { TransactionComponent } from './containers/transaction/transaction.component';
import { EditTransactionComponent } from './components/edit-transaction/edit-transaction.component';
import { CreateTransactionComponent } from './components/create-transaction/create-transaction.component';
import { EnumToArrayPipe } from './models/pipes/enum-to-array.pipe';
import { TableTransactionsComponent } from './components/table-transactions/table-transactions.component';
import { TransactionService } from './services/transaction.service';
import { TransactionRoutingModule } from './transaction-routing.module';

@NgModule({
  declarations: [
    TransactionComponent,
    EditTransactionComponent,
    CreateTransactionComponent,
    TableTransactionsComponent,
    EnumToArrayPipe
  ],
  imports: [
    TransactionRoutingModule,
    FormsModule,
    HttpClientModule,
    PaginationModule.forRoot(),
    ModalModule.forRoot(),
    CommonModule
  ],
  providers: [TransactionService],
  bootstrap: [],
  exports: [
    TransactionComponent,
    EnumToArrayPipe
  ]
})
export class TransactionsModule { }
