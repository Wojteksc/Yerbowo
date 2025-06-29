import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class DataService {
  private readonly EMPTY_VALUE = 'puste';

  private totalCartProductsSource = new BehaviorSubject<number | string>(this.EMPTY_VALUE);
  cartTotal$: Observable<number | string> = this.totalCartProductsSource.asObservable();

  constructor() {}

  changeTotalCartProducts(total: number): void {
    this.totalCartProductsSource.next(total === 0 ? this.EMPTY_VALUE : total);
  }
}
