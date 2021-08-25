import { Injectable } from '@angular/core';
import { User } from '../_models/user';
import { Resolve, ActivatedRouteSnapshot, Router } from '@angular/router';
import { Observable, of } from 'rxjs';
import { UserService } from '../_services/user.service';
import { AuthService } from '../_services/auth.service';
import { catchError } from 'rxjs/operators';
import { AlertifyService } from '../_services/alertify.service';

@Injectable()
export class AccountEditResolver implements Resolve<User> {
  constructor(private userService: UserService,  private authService: AuthService,
    private router: Router, private alertify: AlertifyService) {}

  resolve(route: ActivatedRouteSnapshot): Observable<User> {
    return this.userService.getUser(this.authService.decodedToken.sub).pipe(
      catchError(error => {
        this.alertify.error('Błąd podczas pobrania danych');
        this.router.navigate(['/moje-konto']);
        return of(null);
      })
    );
  }
}
