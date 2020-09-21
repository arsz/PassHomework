import { HttpClient } from "@angular/common/http";
import { Observable } from "rxjs";
import { Injectable } from "@angular/core";
import { ExchangeRates, ExchangeRatesDTO } from "../models/exchange-rates";
import { map } from "rxjs/operators";

@Injectable()
export class ExchangeRatesService {
  constructor(private http: HttpClient) { }

  getLatestExchangeRates(): Observable<ExchangeRates> {
    return this.http.get<ExchangeRatesDTO>('exchangerates').pipe(map(dto => new ExchangeRates(dto)));
  }


  getExchangeRatesBySelectedCurrency(currency: string): Observable<ExchangeRates> {
    return this.http.get<ExchangeRatesDTO>(`/ExchangeRates/${currency}`).pipe(map(dto => new ExchangeRates(dto)));
  }
}
