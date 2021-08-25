import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { environment } from 'src/environments/environment';
import { Cart } from '../_models/cart';
import { CartComponent } from '../components/cart/cart.component';
import { map } from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class CartService {
baseUrl: string = environment.apiUrl;

constructor(private http: HttpClient) { }

get() {
  return this.http.get<Cart>(`${this.baseUrl}cart`);
}

getTotalCartProducts() {
  return this.http.get<number>(`${this.baseUrl}cart/totalCartProducts`)
}

add(id: number, quantity: number = 1) {
  return this.http.post(`${this.baseUrl}cart`, { id, quantity })
  .pipe(
    map(response => {
      return <Cart>response;
    })
  );
}

update(id: number, quantity: number = 1) {
  return this.http.put(`${this.baseUrl}cart/${id}`, { id, quantity })
  .pipe(
    map(response => {
      return <Cart>response;
    })
  );
}

remove(id: number) {
  return this.http.delete(`${this.baseUrl}cart/${id}`)
  .pipe(
    map(response => {
      return <Cart>response;
    })
  );
}

}
