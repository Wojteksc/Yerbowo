import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, Router } from '@angular/router';
import { Observable, of } from 'rxjs';
import { AuthService } from '../../auth/services/auth.service';
import { catchError } from 'rxjs/operators';
import { AlertifyService } from '../../core/services/alertify.service';
import { AddressService } from '../services/address.service';
import { AddressDetail } from '../models/addressDetail';

@Injectable()
export class AddressEditResolver  {
  constructor(private addressService: AddressService,  
    private authService: AuthService,
    private router: Router, 
    private alertify: AlertifyService) {}

  resolve(route: ActivatedRouteSnapshot): Observable<AddressDetail> {
    return this.addressService.getAddress(this.authService.decodedToken.sub, route.params['id']).pipe(
      catchError(error => {
        this.alertify.error('Błąd podczas pobrania danych');
        this.router.navigate(['/moje-konto/adresy']);
        return of(null);
      })
    );
  }
}
