import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { HttpClient } from '@angular/common/http';
import { map } from 'rxjs/operators';
import { JwtHelperService } from '@auth0/angular-jwt';
import { User } from '../_models/user';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  baseUrl: string = environment.apiUrl + 'auth/';
  jwtHelper = new JwtHelperService();
  decodedToken: any;
  photoUrl: string;

constructor(private http: HttpClient) { }

  login(model: any) {
    return this.http.post(this.baseUrl + 'login', model)
    .pipe(
      map((response: any) => {
        this.setToken(response);
        this.setPhotoUrl(response);
      })
    );
  }

  loginWithSocial(model: any) {
    return this.http.post<any>(this.baseUrl + 'socialLogin', model)
    .pipe(
      map((response: any) => {
        this.setToken(response);
        this.setPhotoUrl(response);
      })
    );
  }

  loggedIn() {
    const token = localStorage.getItem('token');
    return !this.jwtHelper.isTokenExpired(token);
  }

  register(user: User) {
    return this.http.post(this.baseUrl + 'register', user);
  }

  setToken(response: any) {
    const user = response.token;
    
    if (user) {
      localStorage.setItem('token', user.token);
      this.decodedToken = this.jwtHelper.decodeToken(user.token);
    }
  }

  setPhotoUrl(response: any) {
    const photoUrl = response.photoUrl;
    
    if (photoUrl) {
      localStorage.setItem('photoUrl', photoUrl);
      this.photoUrl = photoUrl;
    }
  }
}
