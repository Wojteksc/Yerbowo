import { Injectable } from '@angular/core';
import { Router, ActivatedRouteSnapshot } from '@angular/router';
import { ProductCard } from '../models/productCard';
import { ProductService } from '../services/product.service';
import { Observable, of } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { AlertifyService } from '../../core/services/alertify.service';
import { PaginatedResult } from '../../shared/models/pagination';

@Injectable()
export class ProductListResolver  {
  pageNumber = 1;
  pageSize = 20;

  constructor(private productService: ProductService, private router: Router,
    private alertify: AlertifyService) {}

  resolve(route: ActivatedRouteSnapshot): Observable<PaginatedResult<ProductCard[]>>  {
    return this.productService.getProducts(this.pageNumber, this.pageSize, route.params['category'], route.params['subcategory']).pipe(
      catchError(error => {
        this.alertify.error('Błąd podczas pobrania danych');
        this.router.navigate(['/']);
        return of(null);
      })
    );
  }
}
