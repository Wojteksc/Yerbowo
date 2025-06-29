import { Injectable } from '@angular/core';
import { OrderHistoryItem } from '../models/orderHistoryItem';
import { Router } from '@angular/router';
import { Observable, of } from 'rxjs';
import { OrderService } from '../services/order.service';
import { AuthService } from '../../auth/services/auth.service';
import { catchError } from 'rxjs/operators';
import { AlertifyService } from '../../core/services/alertify.service';

@Injectable()
export class OrderHistoryTableResolver  {
  constructor(private orderService: OrderService, private router: Router,
    private authService: AuthService, private alertify: AlertifyService) {}

  resolve(): Observable<OrderHistoryItem[]> {
    return this.orderService.getOrders(this.authService.decodedToken.sub).pipe(
      catchError(error => {
        this.alertify.error('Błąd podczas pobrania danych');
        return of(null);
      })
    );
  }
}
