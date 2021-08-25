import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { Observable } from 'rxjs';
import { ProductCard } from '../_models/productCard';
import { HttpClient, HttpParams } from '@angular/common/http';
import { ProductDetail } from '../_models/productDetail';
import { PaginatedResult } from '../_models/pagination';
import { map } from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class ProductService {
  baseUrl: string = environment.apiUrl;


  constructor(private http: HttpClient) { }

  getProducts(pageNumber?, pageSize?, category?, subcategory?): Observable<PaginatedResult<ProductCard[]>> {
    const paginatedResult: PaginatedResult<ProductCard[]> = new PaginatedResult<ProductCard[]>();

    let params = new HttpParams();

    if(pageNumber != null && pageSize) {
      params = params.append('pageNumber', pageNumber);
      params = params.append('pageSize', pageSize);
    }

    if(category != null) {
      params = params.append('category', category);
    }

    if(subcategory) {
      params = params.append('subcategory', subcategory);
    }

    return this.http.get<ProductCard[]>(this.baseUrl + 'products', { observe: 'response', params })
    .pipe(
      map(response => {
        paginatedResult.result = response.body;
        if(response.headers.get('Pagination') != null) {
          paginatedResult.pagination = JSON.parse(response.headers.get('Pagination'));
        }
        return paginatedResult;
      })
    );
  }

  getProduct(productSlug): Observable<ProductDetail> {
    return this.http.get<ProductDetail>(`${this.baseUrl}products/${productSlug}`);
  }

}
