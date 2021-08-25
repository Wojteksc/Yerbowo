import { Component, OnInit, Input, ViewEncapsulation, Output, EventEmitter } from '@angular/core';
import { AddressCard } from 'src/app/_models/addressCard';
import { AlertifyService } from 'src/app/_services/alertify.service';

@Component({
  selector: 'app-address-card',
  templateUrl: './address-card.component.html',
  styleUrls: ['./address-card.component.css'],
  encapsulation: ViewEncapsulation.None,
})
export class AddressCardComponent implements OnInit {
  @Output() onDeleteAddress = new EventEmitter();
  @Input() address: AddressCard;
  constructor(private alertify: AlertifyService) { }

  ngOnInit() {
  }

  getAddressDelivery(): string {
    if(this.address.apartmentNumber) {
      return `${this.address.buildingNumber}/${this.address.apartmentNumber}`;
    } else {
      return this.address.buildingNumber;
    }
  }

  delete() {
    this.onDeleteAddress.emit();
  }

}
