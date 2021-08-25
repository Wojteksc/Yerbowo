import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class DataService {

private totalCartProductsSource = new BehaviorSubject("puste");
currentTotalMessage = this.totalCartProductsSource.asObservable();

constructor() { }

changeTotalCartProducts(totalCartProducts: any) {
  if(totalCartProducts == 0) {
    totalCartProducts = 'puste';
  }
  this.totalCartProductsSource.next(totalCartProducts);
}

}
