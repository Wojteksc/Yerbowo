import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from 'src/environments/environment';
import { OrderHistoryItem } from '../models/orderHistoryItem';
import { OrderHistoryDetail } from '../models/orderHistoryDetail';

@Injectable({
  providedIn: 'root'
})
export class OrderService {
  private readonly baseUrl = environment.apiUrl;

  constructor(private http: HttpClient) {}

  private getOrderUrl(userId: number, orderId?: number): string {
    return `${this.baseUrl}users/${userId}/orders${orderId ? `/${orderId}` : ''}`;
  }

  getOrders(userId: number) {
    return this.http.get<OrderHistoryItem[]>(this.getOrderUrl(userId));
  }

  getOrderHistoryDetail(userId: number, orderId: number) {
    return this.http.get<OrderHistoryDetail>(this.getOrderUrl(userId, orderId));
  }
}
