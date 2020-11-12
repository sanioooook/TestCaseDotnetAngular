import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { AppRoutingModule } from './app-routing.module';
import { PaginationModule } from 'ngx-bootstrap/pagination';
import { ModalModule } from 'ngx-bootstrap/modal';

import { AppComponent } from './app.component';
import { TransactionComponent } from './transaction/transaction.component';
import { EditTransactionComponent } from './edit-transaction/edit-transaction.component';
import { CreateTransactionComponent } from './create-transaction/create-transaction.component';
import { EnumToArrayPipe } from './pipes/enum-to-array.pipe';
import { TableTransactionsComponent } from './table-transactions/table-transactions.component';

@NgModule({
  declarations: [
    AppComponent,
    TransactionComponent,
    EditTransactionComponent,
    CreateTransactionComponent,
    EnumToArrayPipe,
    TableTransactionsComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    FormsModule,
    HttpClientModule,
    BrowserAnimationsModule,
    PaginationModule.forRoot(),
    ModalModule.forRoot()
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
