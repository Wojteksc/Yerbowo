import { Component, OnInit, ViewEncapsulation } from '@angular/core';
import { AddressCard } from 'src/app/account/models/addressCard';
import { ActivatedRoute } from '@angular/router';
import { AlertifyService } from 'src/app/core/services/alertify.service';
import { AddressService } from 'src/app/account/services/address.service';
import { AuthService } from 'src/app/auth/services/auth.service';
import { Observable, map, of } from 'rxjs';

@Component({
  selector: 'app-address-list',
  templateUrl: './address-list.component.html',
  styleUrls: ['./address-list.component.css'],
  encapsulation: ViewEncapsulation.None,
})
export class AddressListComponent implements OnInit {
  addresses$: Observable<AddressCard[]> = of([]);
  
  constructor(
    private route: ActivatedRoute, 
    private alertify: AlertifyService,
    private addressService: AddressService,
    private authService: AuthService) {}

  ngOnInit(): void {
    this.addresses$ = this.route.data.pipe(
      map((data: { addresses: AddressCard[] }) => data.addresses));
  }

  reloadAddresses(): void {
    const userId = this.authService.decodedToken.sub;
    this.addresses$ = this.addressService.getAddresses(userId);
  }

  removeAddress(id: number): void {
    this.alertify.confirm('Czy chcesz usunąć ten adres?', 'Pytanie', () => {
      const userId = this.authService.decodedToken.sub;
      this.addressService.deleteAddress(userId, id).subscribe({
        next: () => {
          this.alertify.success('Adres został usunięty.');
          this.reloadAddresses();
        },
        error: () => {
          this.alertify.error('Wystąpił błąd podczas usuwania adresu.');
        }
      });
    });
  }
}
