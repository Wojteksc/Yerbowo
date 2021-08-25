import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from 'src/environments/environment';
import { OrderHistoryItem } from '../_models/orderHistoryItem';
import { OrderHistoryDetail } from '../_models/orderHistoryDetail';

@Injectable({
  providedIn: 'root'
})
export class OrderService {

  baseUrl: string = environment.apiUrl;

  constructor(private http: HttpClient) { }

  getOrders(userId: number) {
    return this.http.get<OrderHistoryItem[]>(this.baseUrl + `users/${userId}/orders`);
  }

  getOrderHistoryDetail(userId: number, orderId: number) {
    return this.http.get<OrderHistoryDetail>(this.baseUrl + `users/${userId}/orders/${orderId}`);
  }

}
