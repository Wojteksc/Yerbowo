import { Injectable } from '@angular/core';
import { Router, ActivatedRouteSnapshot } from '@angular/router';
import { Observable, of } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { AlertifyService } from '../../core/services/alertify.service';
import { AddressCard } from '../models/addressCard';
import { AddressService } from '../services/address.service';
import { AuthService } from '../../auth/services/auth.service';

@Injectable()
export class AddressListResolver  {
  constructor(private addressService: AddressService, private router: Router,
    private authService: AuthService, private alertify: AlertifyService) {}

  resolve(): Observable<AddressCard[]>  {
    return this.addressService.getAddresses(this.authService.decodedToken.sub).pipe(
      catchError(error => {
        this.alertify.error('Błąd podczas pobrania danych');
        return of(null);
      })
    );
  }
}
