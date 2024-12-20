import {NgModule} from '@angular/core';
import {FormsModule} from '@angular/forms';
import {provideHttpClient, withInterceptorsFromDi} from '@angular/common/http';
import {CommonModule} from '@angular/common';
import {
  EditTransactionComponent,
  CreateTransactionComponent,
  TableTransactionsComponent,
} from './components';
import {TransactionComponent} from './containers/transaction/transaction.component';
import {EnumToArrayPipe} from './models/pipes/enum-to-array.pipe';
import {TransactionService} from './services/transaction.service';
import {TransactionRoutingModule} from './transaction-routing.module';
import {MtxGridModule} from '@ng-matero/extensions/grid';
import {MatIconModule} from '@angular/material/icon';
import {MatDialogModule} from '@angular/material/dialog';
import {MatButtonModule} from '@angular/material/button';
import {MatFormFieldModule} from '@angular/material/form-field';
import {MatSelectModule} from '@angular/material/select';
import {MatInputModule} from '@angular/material/input';
import {MatPaginatorModule} from '@angular/material/paginator';
import {MatTooltipModule} from '@angular/material/tooltip';

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
    CommonModule,
    MatDialogModule,
    MatButtonModule,
    MatFormFieldModule,
    MatSelectModule,
    MatInputModule,
    MtxGridModule,
    MatPaginatorModule,
    MatIconModule,
    MatTooltipModule
  ],
  providers: [
    TransactionService,
    provideHttpClient(withInterceptorsFromDi())
  ],
  bootstrap: [],
  exports: [
    TransactionComponent,
    EnumToArrayPipe
  ]
})
export class TransactionsModule {
}
