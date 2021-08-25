import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { HttpClient } from '@angular/common/http';
import { AddressCard } from '../_models/addressCard';
import { AddressCreate } from '../_models/addressCreate';
import { AddressDetail } from '../_models/addressDetail';
import { AddressEdit } from '../_models/addressEdit';

@Injectable({
  providedIn: 'root'
})
export class AddressService {
  baseUrl: string = environment.apiUrl;

  constructor(private http: HttpClient) { }

  getAddress(userId: number, addressId: number) {
    return this.http.get<AddressDetail>(this.baseUrl + `users/${userId}/addresses/${addressId}`);
  }

  getAddresses(userId: number) {
    return this.http.get<AddressCard[]>(this.baseUrl + `users/${userId}/addresses`);
  }

  createAddress(userId: number, address: AddressCreate) {
    return this.http.post(`${this.baseUrl}users/${userId}/addresses`, address);
  }

  updateAddress(userId: number, addressId: number, address: AddressEdit) {
    return this.http.put(`${this.baseUrl}users/${userId}/addresses/${addressId}`, address);
  }

  deleteAddress(userId: number, addressId: number) {
    return this.http.delete(`${this.baseUrl}users/${userId}/addresses/${addressId}`);
  }
}
