import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { TableTransactionsComponent } from './table-transactions.component';

describe('TableTransactionsComponent', () => {
  let component: TableTransactionsComponent;
  let fixture: ComponentFixture<TableTransactionsComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ TableTransactionsComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(TableTransactionsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
