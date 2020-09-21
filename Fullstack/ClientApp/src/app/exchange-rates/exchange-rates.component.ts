import { Component, OnInit, ViewChild } from '@angular/core';
import { ExchangeRatesService } from '../services/exchange-rates.service';
import { ExchangeRates, Rate } from '../models/exchange-rates';
import { finalize } from 'rxjs/operators';
import { MatTableDataSource } from '@angular/material/table';
import { MatSort } from '@angular/material/sort';
import { MatPaginator } from '@angular/material/paginator';

@Component({
  selector: 'app-exchange-rates',
  templateUrl: './exchange-rates.component.html',
  styleUrls: ['./exchange-rates.component.css']
})
export class ExchangeRatesComponent implements OnInit {

  exchangeRates: ExchangeRates = undefined;
  tableDataSource: MatTableDataSource<Rate> = undefined;

  @ViewChild(MatSort, { static: true }) sort: MatSort;
  @ViewChild(MatPaginator, { static: true }) paginator: MatPaginator;

  constructor(private exchangeRatesService: ExchangeRatesService) { }

  ngOnInit() {
    const subscription = this.exchangeRatesService.getLatestExchangeRates()
      .pipe(finalize(() => subscription.unsubscribe()))
      .subscribe((rates: ExchangeRates) => {
        this.exchangeRates = rates;
        this.tableDataSource = new MatTableDataSource(rates.rates);
        this.tableDataSource.sort = this.sort;
        this.tableDataSource.paginator = this.paginator;
      });
  }

  cellClicked(currency) {
    const subscription = this.exchangeRatesService.getExchangeRatesBySelectedCurrency(currency)
      .pipe(finalize(() => subscription.unsubscribe()))
      .subscribe((rates: ExchangeRates) => {
        this.exchangeRates = rates;
        this.tableDataSource = new MatTableDataSource(rates.rates);
        this.tableDataSource.sort = this.sort;
        this.tableDataSource.paginator = this.paginator;
      });
  }
}
