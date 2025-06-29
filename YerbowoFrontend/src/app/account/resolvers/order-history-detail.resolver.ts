import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot } from '@angular/router';
import { OrderService } from '../services/order.service';
import { Observable, of } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { AlertifyService } from '../../core/services/alertify.service';
import { OrderHistoryDetail } from '../models/orderHistoryDetail';
import { AuthService } from '../../auth/services/auth.service';

@Injectable()
export class OrderHistoryDetailResolver  {
  constructor(private orderService: OrderService, private alertify: AlertifyService,
    private authService: AuthService) {}

  resolve(route: ActivatedRouteSnapshot): Observable<OrderHistoryDetail> {
    return this.orderService.getOrderHistoryDetail(this.authService.decodedToken.sub, route.params['id']).pipe(
      catchError(error => {
        this.alertify.error('Błąd podczas pobrania danych');
        return of(null);
      })
    );
  }
}
