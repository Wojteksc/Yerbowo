import { Component, OnInit, ViewEncapsulation } from '@angular/core';
import { AddressCard } from 'src/app/_models/addressCard';
import { ActivatedRoute } from '@angular/router';
import { AlertifyService } from 'src/app/_services/alertify.service';
import { AddressService } from 'src/app/_services/address.service';
import { AuthService } from 'src/app/_services/auth.service';

@Component({
  selector: 'app-address-list',
  templateUrl: './address-list.component.html',
  styleUrls: ['./address-list.component.css'],
  encapsulation: ViewEncapsulation.None,
})
export class AddressListComponent implements OnInit {
  addresses: AddressCard[]
  
  constructor(private activatedRoute: ActivatedRoute, 
    private alertify: AlertifyService,
    private adressService: AddressService,
    private authService: AuthService) {}

  ngOnInit() {
    this.activatedRoute.data.subscribe(data => {
      this.addresses = data['addresses'];
    })
  }

  isAnyAddress(): boolean {
    return this.addresses.length > 0;
  }

  removeAddress(id: number) {
    this.alertify.confirm('Czy chcesz usunąć ten adres?', "Pytanie", () => {
      this.adressService.deleteAddress(this.authService.decodedToken.sub, id).subscribe(() => {
        this.addresses.splice(this.addresses.findIndex(a => a.id === id), 1);
        this.alertify.success("Adres został usunięty.")
      }, error => {
        this.alertify.error("Wystąpił błąd podczas usuwania adresu");
      });
    });
  }
}
