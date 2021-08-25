import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { Observable } from 'rxjs';
import { HomeProducts } from '../_models/homeProducts';
import { HttpClient } from '@angular/common/http';

@Injectable({
  providedIn: 'root'
})
export class HomeService {
  baseUrl: string = environment.apiUrl;

  constructor(private http: HttpClient) { }

  getProducts(): Observable<HomeProducts> {
    return this.http.get<HomeProducts>(this.baseUrl + 'home');
  }

}
