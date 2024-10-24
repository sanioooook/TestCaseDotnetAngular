import {NgModule} from '@angular/core';
import {BrowserAnimationsModule} from '@angular/platform-browser/animations';

import {AppComponent} from './app.component';
import {TransactionsModule} from './transactions/transactions.module';
import {AppRoutingModule} from './app-routing.module';

// import { PipesModuleModule } from './transactions/models/pipes-module.module';

@NgModule({
  declarations: [
    AppComponent,
  ],
  imports: [
    AppRoutingModule,
    BrowserAnimationsModule,
    TransactionsModule,
    // PipesModuleModule
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule {
}
