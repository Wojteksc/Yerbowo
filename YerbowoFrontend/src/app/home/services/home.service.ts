import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from 'src/environments/environment';
import { Observable } from 'rxjs';
import { HomeProducts } from '../../shared/models/homeProducts';

@Injectable({
  providedIn: 'root',
})
export class HomeService {
  private readonly baseUrl = `${environment.apiUrl}home`;

  constructor(private http: HttpClient) {}

  getProducts(): Observable<HomeProducts> {
    return this.http.get<HomeProducts>(this.baseUrl);
  }
}
