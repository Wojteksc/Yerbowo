import { Injectable } from '@angular/core';
import { HttpClient, HttpParams, HttpResponse } from '@angular/common/http';
import { environment } from 'src/environments/environment';
import { Observable } from 'rxjs';
import { ProductCard } from '../models/productCard';
import { ProductDetail } from '../models/productDetail';
import { PaginatedResult } from '../../shared/models/pagination';
import { map } from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class ProductService {
  private readonly baseUrl = `${environment.apiUrl}products`;

  constructor(private http: HttpClient) {}

  getProducts(
    pageNumber?: number,
    pageSize?: number,
    category?: string,
    subcategory?: string
  ): Observable<PaginatedResult<ProductCard[]>> {
    const paginatedResult: PaginatedResult<ProductCard[]> = new PaginatedResult<ProductCard[]>();
    let params = new HttpParams();

    if (pageNumber != null && pageSize != null) {
      params = params.set('pageNumber', pageNumber.toString());
      params = params.set('pageSize', pageSize.toString());
    }

    if (category) {
      params = params.set('category', category);
    }

    if (subcategory) {
      params = params.set('subcategory', subcategory);
    }

    return this.http.get<ProductCard[]>(this.baseUrl, { observe: 'response', params })
      .pipe(
        map((response: HttpResponse<ProductCard[]>) => {
          paginatedResult.result = response.body ?? [];
          const paginationHeader = response.headers.get('Pagination');
          if (paginationHeader) {
            paginatedResult.pagination = JSON.parse(paginationHeader);
          }
          return paginatedResult;
        })
      );
  }

  getProduct(productSlug: string): Observable<ProductDetail> {
    return this.http.get<ProductDetail>(`${this.baseUrl}/${productSlug}`);
  }
}