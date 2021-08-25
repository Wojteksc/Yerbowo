import { Injectable } from '@angular/core';
import { Resolve, Router, ActivatedRouteSnapshot } from '@angular/router';
import { ProductCard } from '../_models/productCard';
import { ProductService } from '../_services/product.service';
import { Observable, of } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { AlertifyService } from '../_services/alertify.service';

@Injectable()
export class ProductDetailResolver implements Resolve<ProductCard> {
  constructor(private productService: ProductService, private router: Router,
    private alertify: AlertifyService) {}

  resolve(route: ActivatedRouteSnapshot): Observable<ProductCard>  {
    return this.productService.getProduct(route.params['product']).pipe(
      catchError(error => {
        this.alertify.error('Błąd podczas pobrania danych');
        this.router.navigate(['/home']);
        return of(null);
      })
    );
  }
}
