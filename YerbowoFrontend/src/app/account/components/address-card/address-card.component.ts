import { Component, OnInit, Input, ViewEncapsulation, Output, EventEmitter } from '@angular/core';
import { AddressCard } from 'src/app/account/models/addressCard';

@Component({
  selector: 'app-address-card',
  templateUrl: './address-card.component.html',
  styleUrls: ['./address-card.component.css'],
  encapsulation: ViewEncapsulation.None,
})
export class AddressCardComponent implements OnInit {
  @Output() onDeleteAddress = new EventEmitter<void>();
  @Input() address!: AddressCard;

  constructor() {}

  ngOnInit(): void {}

  getAddressDelivery(): string {
    return this.address.apartmentNumber
      ? `${this.address.buildingNumber}/${this.address.apartmentNumber}`
      : this.address.buildingNumber;
  }

  delete(): void {
    this.onDeleteAddress.emit();
  }
}
