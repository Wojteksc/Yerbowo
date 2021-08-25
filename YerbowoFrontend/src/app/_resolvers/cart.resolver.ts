import { Injectable } from '@angular/core';
import { Resolve, Router, ActivatedRouteSnapshot } from '@angular/router';
import { Observable, of } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { AlertifyService } from '../_services/alertify.service';
import { CartService } from '../_services/cart.service';
import { Cart } from '../_models/cart';

@Injectable()
export class CartResolver implements Resolve<Cart> {

  constructor(private cartService: CartService, private router: Router,
    private alertify: AlertifyService) {}

  resolve(route: ActivatedRouteSnapshot): Observable<Cart>  {
    return this.cartService.get().pipe(
      catchError(error => {
        this.alertify.error('Błąd podczas pobrania danych');
        this.router.navigate(['/home']);
        return of(null);
      })
    );
  }
}
