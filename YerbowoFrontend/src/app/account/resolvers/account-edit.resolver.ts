import { Injectable } from '@angular/core';
import { User } from '../../shared/models/user';
import { ActivatedRouteSnapshot, Router } from '@angular/router';
import { Observable, of } from 'rxjs';
import { UserService } from '../services/user.service';
import { AuthService } from '../../auth/services/auth.service';
import { catchError } from 'rxjs/operators';
import { AlertifyService } from '../../core/services/alertify.service';

@Injectable()
export class AccountEditResolver  {
  constructor(
    private userService: UserService,
    private authService: AuthService,
    private router: Router,
    private alertify: AlertifyService
  ) {}

  resolve(route: ActivatedRouteSnapshot): Observable<User> {
    const decodedToken = this.authService.decodedToken;
    if (!decodedToken || !decodedToken.sub) {
      this.router.navigate(['/login']); 
      return of(null);
    }

    return this.userService.getUser(decodedToken.sub).pipe(
      catchError(error => {
        this.alertify.error('Błąd podczas pobrania danych');
        this.router.navigate(['/moje-konto']);
        return of(null);
      })
    );
  }
}
