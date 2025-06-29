import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from 'src/environments/environment';
import { Cart } from '../models/cart';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class CartService {
  private readonly baseUrl = environment.apiUrl + 'cart';

  constructor(private http: HttpClient) { }

  get(): Observable<Cart> {
    return this.http.get<Cart>(this.baseUrl);
  }

  getTotalCartProducts(): Observable<number> {
    return this.http.get<number>(`${this.baseUrl}/totalCartProducts`);
  }

  add(id: number, quantity: number = 1): Observable<Cart> {
    return this.http.post<Cart>(this.baseUrl, { id, quantity });
  }

  update(id: number, quantity: number = 1): Observable<Cart> {
    return this.http.put<Cart>(`${this.baseUrl}/${id}`, { id, quantity });
  }

  remove(id: number): Observable<Cart> {
    return this.http.delete<Cart>(`${this.baseUrl}/${id}`);
  }
}
