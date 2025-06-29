import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { User } from '../../shared/models/user';

@Injectable({
  providedIn: 'root'
})
export class UserService {
  private readonly baseUrl = environment.apiUrl;

  constructor(private http: HttpClient) {}

  private getUserUrl(id: number): string {
    return `${this.baseUrl}users/${id}`;
  }

  getUser(id: number): Observable<User> {
    return this.http.get<User>(this.getUserUrl(id));
  }

  updateUser(id: number, user: User): Observable<any> {
    return this.http.put(this.getUserUrl(id), user);
  }
}
