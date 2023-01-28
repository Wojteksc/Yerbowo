import { Injectable } from '@angular/core';
import { Resolve, Router, ActivatedRouteSnapshot } from '@angular/router';
import { ProductService } from '../_services/product.service';
import { Observable, of } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { AlertifyService } from '../_services/alertify.service';
import { ProductDetail } from '../_models/productDetail';

@Injectable()
export class ProductDetailResolver implements Resolve<ProductDetail> {
  constructor(private productService: ProductService, private router: Router,
    private alertify: AlertifyService) {}

  resolve(route: ActivatedRouteSnapshot): Observable<ProductDetail>  {
    return this.productService.getProduct(route.params['product']).pipe(
      catchError(error => {
        this.alertify.error('Błąd podczas pobrania danych');
        this.router.navigate(['/home']);
        return of(null);
      })
    );
  }
}
