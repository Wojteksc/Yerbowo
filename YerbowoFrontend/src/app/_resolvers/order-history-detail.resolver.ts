import { Injectable } from '@angular/core';
import { Resolve, Router, ActivatedRouteSnapshot } from '@angular/router';
import { OrderService } from '../_services/order.service';
import { Observable, of } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { AlertifyService } from '../_services/alertify.service';
import { OrderHistoryDetail } from '../_models/orderHistoryDetail';
import { AuthService } from '../_services/auth.service';

@Injectable()
export class OrderHistoryDetailResolver implements Resolve<OrderHistoryDetail> {
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
