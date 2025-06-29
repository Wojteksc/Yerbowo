import { Injectable } from '@angular/core';
import { Router, ActivatedRouteSnapshot } from '@angular/router';
import { ProductService } from '../services/product.service';
import { Observable, of } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { AlertifyService } from '../../core/services/alertify.service';
import { ProductDetail } from '../models/productDetail';

@Injectable()
export class ProductDetailResolver  {
  constructor(private productService: ProductService, private router: Router,
    private alertify: AlertifyService) {}

  resolve(route: ActivatedRouteSnapshot): Observable<ProductDetail>  {
    return this.productService.getProduct(route.params['product']).pipe(
      catchError(error => {
        this.alertify.error('Błąd podczas pobrania danych');
        this.router.navigate(['/']);
        return of(null);
      })
    );
  }
}
