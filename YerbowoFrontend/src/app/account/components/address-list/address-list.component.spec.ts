import { ComponentFixture, TestBed, fakeAsync, tick } from '@angular/core/testing';
import { AddressListComponent } from './address-list.component';
import { ActivatedRoute } from '@angular/router';
import { AlertifyService } from 'src/app/core/services/alertify.service';
import { AddressService } from 'src/app/account/services/address.service';
import { AuthService } from 'src/app/auth/services/auth.service';
import { of, throwError } from 'rxjs';
import { Component, Input, Output, EventEmitter } from '@angular/core';

@Component({
  selector: 'app-address-card',
  template: ''
})
class MockAddressCardComponent {
  @Input() address: any;
  @Output() onDeleteAddress = new EventEmitter<number>();
}

describe('AddressListComponent', () => {
  let component: AddressListComponent;
  let fixture: ComponentFixture<AddressListComponent>;
  let alertifySpy: jasmine.SpyObj<AlertifyService>;
  let addressServiceSpy: jasmine.SpyObj<AddressService>;
  let authServiceStub: Partial<AuthService>;
  let activatedRouteStub: Partial<ActivatedRoute>;

  const mockAddresses = [
    {
      id: 1,
      userId: 123,
      alias: 'Dom',
      firstName: 'Jan',
      lastName: 'Kowalski',
      street: 'Mickiewicza',
      buildingNumber: '10',
      apartmentNumber: '5',
      place: 'Warszawa',
      postCode: '00-001',
      phone: '123456789',
      email: 'jan.kowalski@example.com',
      nip: '123-456-78-90',
      company: 'FirmaX'
    }
  ];

  beforeEach(async () => {
    alertifySpy = jasmine.createSpyObj('AlertifyService', ['confirm', 'success', 'error']);
    addressServiceSpy = jasmine.createSpyObj('AddressService', ['getAddresses', 'deleteAddress']);
    authServiceStub = { decodedToken: { sub: 123 } };
    activatedRouteStub = { data: of({ addresses: mockAddresses }) };

    await TestBed.configureTestingModule({
      declarations: [AddressListComponent, MockAddressCardComponent],
      providers: [
        { provide: AlertifyService, useValue: alertifySpy },
        { provide: AddressService, useValue: addressServiceSpy },
        { provide: AuthService, useValue: authServiceStub },
        { provide: ActivatedRoute, useValue: activatedRouteStub }
      ]
    }).compileComponents();

    fixture = TestBed.createComponent(AddressListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create and load addresses from route data', (done) => {
    component.addresses$.subscribe(addresses => {
      expect(addresses).toEqual(mockAddresses);
      done();
    });
  });

  it('reloadAddresses should fetch addresses for current user', (done) => {
    addressServiceSpy.getAddresses.and.returnValue(of(mockAddresses));
    component.reloadAddresses();
    component.addresses$.subscribe(addresses => {
      expect(addresses).toEqual(mockAddresses);
      done();
    });
    expect(addressServiceSpy.getAddresses).toHaveBeenCalledWith(authServiceStub.decodedToken.sub);
  });

  it('removeAddress should call alertify.confirm and delete address on confirm', () => {
    alertifySpy.confirm.and.callFake((msg, title, okCallback) => okCallback());
    addressServiceSpy.deleteAddress.and.returnValue(of(null));
    addressServiceSpy.getAddresses.and.returnValue(of(mockAddresses));
    spyOn(component, 'reloadAddresses');

    component.removeAddress(1);

    expect(alertifySpy.confirm).toHaveBeenCalledWith('Czy chcesz usunąć ten adres?', 'Pytanie', jasmine.any(Function));
    expect(addressServiceSpy.deleteAddress).toHaveBeenCalledWith(authServiceStub.decodedToken.sub, 1);
    expect(alertifySpy.success).toHaveBeenCalledWith('Adres został usunięty.');
    expect(component.reloadAddresses).toHaveBeenCalled();
  });

  it('removeAddress should show error alert if delete fails', () => {
    alertifySpy.confirm.and.callFake((msg, title, okCallback) => okCallback());
    addressServiceSpy.deleteAddress.and.returnValue(throwError(() => new Error('Delete failed')));

    component.removeAddress(1);

    expect(alertifySpy.error).toHaveBeenCalledWith('Wystąpił błąd podczas usuwania adresu.');
  });
});
