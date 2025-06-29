import { Injectable } from '@angular/core';
import { Router, ActivatedRouteSnapshot } from '@angular/router';
import { Observable, of } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { Cart } from '../models/cart';
import { CartService } from '../services/cart.service';
import { AlertifyService } from 'src/app/core/services/alertify.service';

@Injectable()
export class CartResolver  {

  constructor(private cartService: CartService, private router: Router,
    private alertify: AlertifyService) {}

  resolve(route: ActivatedRouteSnapshot): Observable<Cart>  {
    return this.cartService.get().pipe(
      catchError(error => {
        this.alertify.error('Błąd podczas pobrania danych');
        this.router.navigate(['/']);
        return of(null);
      })
    );
  }
}
