import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class NewsletterService {
  private readonly baseUrl = `${environment.apiUrl}newsletter/`;

  constructor(private http: HttpClient) {}

  invite(email: string): Observable<string> {
    return this.http.post<string>(`${this.baseUrl}invite`, email);
  }

  subscribe(payload: any): Observable<any> {
    return this.http.post(`${this.baseUrl}subscribe`, payload);
  }

  unsubscribe(payload: any): Observable<any> {
    return this.http.post(`${this.baseUrl}unsubscribe`, payload);
  }
}