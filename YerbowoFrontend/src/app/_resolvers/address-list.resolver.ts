import { Injectable } from '@angular/core';
import { Resolve, Router, ActivatedRouteSnapshot } from '@angular/router';
import { Observable, of } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { AlertifyService } from '../_services/alertify.service';
import { AddressCard } from '../_models/addressCard';
import { AddressService } from '../_services/address.service';
import { AuthService } from '../_services/auth.service';

@Injectable()
export class AddressListResolver implements Resolve<AddressCard[]> {
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
