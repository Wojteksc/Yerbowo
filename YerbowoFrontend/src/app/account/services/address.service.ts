import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from 'src/environments/environment';
import { AddressCard } from '../models/addressCard';
import { AddressCreate } from '../models/addressCreate';
import { AddressDetail } from '../models/addressDetail';
import { AddressEdit } from '../models/addressEdit';

@Injectable({
  providedIn: 'root'
})
export class AddressService {
  private readonly baseUrl = environment.apiUrl;

  constructor(private http: HttpClient) {}

  private getAddressUrl(userId: number, addressId?: number): string {
    return `${this.baseUrl}users/${userId}/addresses${addressId ? `/${addressId}` : ''}`;
  }

  getAddress(userId: number, addressId: number) {
    return this.http.get<AddressDetail>(this.getAddressUrl(userId, addressId));
  }

  getAddresses(userId: number) {
    return this.http.get<AddressCard[]>(this.getAddressUrl(userId));
  }

  createAddress(userId: number, address: AddressCreate) {
    return this.http.post(this.getAddressUrl(userId), address);
  }

  updateAddress(userId: number, addressId: number, address: AddressEdit) {
    return this.http.put(this.getAddressUrl(userId, addressId), address);
  }

  deleteAddress(userId: number, addressId: number) {
    return this.http.delete(this.getAddressUrl(userId, addressId));
  }
}
