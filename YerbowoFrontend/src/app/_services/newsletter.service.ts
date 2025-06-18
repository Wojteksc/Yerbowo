import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { HttpClient } from '@angular/common/http';
import { map } from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class NewsletterService {
  baseUrl: string = environment.apiUrl + 'newsletter/';

constructor(private http: HttpClient) { }

invite(email: string) {
  return this.http.post(`${this.baseUrl}invite`, email)
  .pipe(
    map(response => {
      return <string>response;
    })
  );
}

subscribe(response: any) {
  return this.http.post(`${this.baseUrl}subscribe`, response);
}

unsubscribe(response: any) {
  return this.http.post(`${this.baseUrl}unsubscribe`, response);
}
}