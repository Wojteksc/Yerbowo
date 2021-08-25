import { Injectable } from '@angular/core';
import { OrderHistoryItem } from '../_models/orderHistoryItem';
import { Resolve, ActivatedRouteSnapshot, Router } from '@angular/router';
import { Observable, of } from 'rxjs';
import { OrderService } from '../_services/order.service';
import { AuthService } from '../_services/auth.service';
import { catchError } from 'rxjs/operators';
import { AlertifyService } from '../_services/alertify.service';

@Injectable()
export class OrderHistoryTableResolver implements Resolve<OrderHistoryItem[]> {
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
