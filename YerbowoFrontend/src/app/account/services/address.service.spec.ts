import { TestBed } from '@angular/core/testing';
import { HttpClientTestingModule, HttpTestingController } from '@angular/common/http/testing';
import { AddressService } from './address.service';
import { AddressCard } from '../models/addressCard';
import { AddressCreate } from '../models/addressCreate';
import { AddressDetail } from '../models/addressDetail';
import { AddressEdit } from '../models/addressEdit';
import { environment } from 'src/environments/environment';

describe('AddressService', () => {
  let service: AddressService;
  let httpMock: HttpTestingController;
  const baseUrl = environment.apiUrl;

  const userId = 1;
  const addressId = 10;

  beforeEach(() => {
    TestBed.configureTestingModule({
      imports: [HttpClientTestingModule],
      providers: [AddressService]
    });
    service = TestBed.inject(AddressService);
    httpMock = TestBed.inject(HttpTestingController);
  });

  afterEach(() => {
    httpMock.verify();
  });

  it('should retrieve address detail', () => {
    const mockAddress: AddressDetail = {
      id: addressId,
      userId,
      alias: 'Home',
      firstName: 'John',
      lastName: 'Doe',
      street: 'Main St',
      buildingNumber: '1',
      apartmentNumber: '2',
      place: 'City',
      postCode: '00-000',
      phone: '123456789',
      email: 'john@example.com',
      nip: '1234563218',
      company: 'Company'
    };

    service.getAddress(userId, addressId).subscribe((data) => {
      expect(data).toEqual(mockAddress);
    });

    const req = httpMock.expectOne(`${baseUrl}users/${userId}/addresses/${addressId}`);
    expect(req.request.method).toBe('GET');
    req.flush(mockAddress);
  });

  it('should retrieve address cards', () => {
    const mockAddresses: AddressCard[] = [{
      id: 1,
      userId,
      alias: 'Work',
      firstName: 'Jane',
      lastName: 'Smith',
      street: 'Second St',
      buildingNumber: '12',
      apartmentNumber: '5',
      place: 'Town',
      postCode: '11-111',
      phone: '987654321',
      email: 'jane@example.com',
      nip: '8765432190',
      company: 'Biz Ltd'
    }];

    service.getAddresses(userId).subscribe((data) => {
      expect(data.length).toBe(1);
      expect(data).toEqual(mockAddresses);
    });

    const req = httpMock.expectOne(`${baseUrl}users/${userId}/addresses`);
    expect(req.request.method).toBe('GET');
    req.flush(mockAddresses);
  });

  it('should create a new address', () => {
    const newAddress: AddressCreate = {
      userId,
      alias: 'Home',
      firstName: 'Anna',
      lastName: 'Nowak',
      street: 'New St',
      buildingNumber: '3',
      apartmentNumber: '7',
      place: 'Village',
      postCode: '22-222',
      phone: '111222333',
      email: 'anna@example.com',
      nip: '1112223344',
      company: 'NewCo'
    };

    service.createAddress(userId, newAddress).subscribe((res) => {
      expect(res).toBeTruthy();
    });

    const req = httpMock.expectOne(`${baseUrl}users/${userId}/addresses`);
    expect(req.request.method).toBe('POST');
    expect(req.request.body).toEqual(newAddress);
    req.flush({ success: true });
  });

  it('should update an address', () => {
    const updatedAddress: AddressEdit = {
      id: addressId,
      userId,
      alias: 'Office',
      firstName: 'Tom',
      lastName: 'White',
      street: 'Update St',
      buildingNumber: '99',
      apartmentNumber: '9',
      place: 'BigCity',
      postCode: '33-333',
      phone: '444555666',
      email: 'tom@example.com',
      nip: '9998887776',
      company: 'UpdateCorp'
    };

    service.updateAddress(userId, addressId, updatedAddress).subscribe((res) => {
      expect(res).toBeTruthy();
    });

    const req = httpMock.expectOne(`${baseUrl}users/${userId}/addresses/${addressId}`);
    expect(req.request.method).toBe('PUT');
    expect(req.request.body).toEqual(updatedAddress);
    req.flush({ success: true });
  });

  it('should delete an address', () => {
    service.deleteAddress(userId, addressId).subscribe((res) => {
      expect(res).toBeTruthy();
    });

    const req = httpMock.expectOne(`${baseUrl}users/${userId}/addresses/${addressId}`);
    expect(req.request.method).toBe('DELETE');
    req.flush({ success: true });
  });
});
