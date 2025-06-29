import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { HttpClient } from '@angular/common/http';
import { map } from 'rxjs/operators';
import { JwtHelperService } from '@auth0/angular-jwt';
import { Observable } from 'rxjs';
import { User } from '../../shared/models/user';

interface AuthResponse {
  token: { token: string };
  photoUrl?: string;
}

@Injectable({
  providedIn: 'root',
})
export class AuthService {
  private readonly baseUrl = environment.apiUrl + 'auth/';
  private jwtHelper = new JwtHelperService();
  decodedToken: any = null;
  photoUrl: string | null = null;

  constructor(private http: HttpClient) {}

  login(model: any): Observable<void> {
    return this.http.post<AuthResponse>(`${this.baseUrl}login`, model).pipe(
      map(response => {
        this.handleAuthResponse(response);
      })
    );
  }

  loginWithSocial(model: any): Observable<void> {
    return this.http.post<AuthResponse>(`${this.baseUrl}socialLogin`, model).pipe(
      map(response => {
        this.handleAuthResponse(response);
      })
    );
  }

  signOut(): void {
    localStorage.removeItem('token');
    localStorage.removeItem('photoUrl');
    this.decodedToken = null;
    this.photoUrl = null;
  }

  isLoggedIn(): boolean {
    const token = localStorage.getItem('token');
    return token != null && !this.jwtHelper.isTokenExpired(token);
  }

  register(user: User): Observable<any> {
    return this.http.post(`${this.baseUrl}register`, user);
  }

  confirmEmail(data: any): Observable<any> {
    return this.http.post(`${this.baseUrl}confirmEmail`, data);
  }

  restoreSession(): void {
    const token = localStorage.getItem('token');
    const photoUrl = localStorage.getItem('photoUrl');

    if (token) {
      this.decodedToken = this.jwtHelper.decodeToken(token);
    }

    if (photoUrl) {
      this.photoUrl = photoUrl;
    }
  }

  private handleAuthResponse(response: AuthResponse): void {
    if (response.token?.token) {
      localStorage.setItem('token', response.token.token);
      this.decodedToken = this.jwtHelper.decodeToken(response.token.token);
    }
    if (response.photoUrl) {
      localStorage.setItem('photoUrl', response.photoUrl);
      this.photoUrl = response.photoUrl;
    }
  }
}
