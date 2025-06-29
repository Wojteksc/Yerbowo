import { ComponentFixture, TestBed, fakeAsync, tick } from '@angular/core/testing';
import { AddressEditComponent } from './address-edit.component';
import { UntypedFormBuilder, ReactiveFormsModule } from '@angular/forms';
import { AddressService } from 'src/app/account/services/address.service';
import { ActivatedRoute, Router } from '@angular/router';
import { AuthService } from 'src/app/auth/services/auth.service';
import { AlertifyService } from 'src/app/core/services/alertify.service';
import { of, throwError } from 'rxjs';

describe('AddressEditComponent', () => {
  let component: AddressEditComponent;
  let fixture: ComponentFixture<AddressEditComponent>;
  let addressServiceSpy: jasmine.SpyObj<AddressService>;
  let alertifySpy: jasmine.SpyObj<AlertifyService>;
  let routerSpy: jasmine.SpyObj<Router>;
  let authServiceStub: Partial<AuthService>;

  const mockAddress = {
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
  };

  beforeEach(async () => {
    addressServiceSpy = jasmine.createSpyObj('AddressService', ['updateAddress']);
    alertifySpy = jasmine.createSpyObj('AlertifyService', ['success', 'error']);
    routerSpy = jasmine.createSpyObj('Router', ['navigate']);
    authServiceStub = {
      decodedToken: { sub: 123 }
    };

    await TestBed.configureTestingModule({
      declarations: [AddressEditComponent],
      imports: [ReactiveFormsModule],
      providers: [
        UntypedFormBuilder,
        { provide: AddressService, useValue: addressServiceSpy },
        { provide: AlertifyService, useValue: alertifySpy },
        { provide: Router, useValue: routerSpy },
        { 
          provide: ActivatedRoute, 
          useValue: {
            data: of({ address: mockAddress }),
            snapshot: { params: { id: mockAddress.id } }
          }
        },
        { provide: AuthService, useValue: authServiceStub }
      ]
    }).compileComponents();

    fixture = TestBed.createComponent(AddressEditComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should initialize form with address data', () => {
    const form = component.addressEditForm;
    expect(form.value.alias).toBe(mockAddress.alias);
    expect(form.value.firstName).toBe(mockAddress.firstName);
    expect(form.value.lastName).toBe(mockAddress.lastName);
    expect(form.value.buildingNumber).toBe(mockAddress.buildingNumber);
  });

  it('should mark form invalid if required fields are empty', () => {
    component.addressEditForm.controls['alias'].setValue('');
    component.addressEditForm.controls['firstName'].setValue('');
    expect(component.addressEditForm.invalid).toBeTrue();
  });

  it('should not call updateAddress if form is invalid', () => {
    component.submitted = false;
    component.addressEditForm.controls['alias'].setValue('');
    component.updateAddress();
    expect(addressServiceSpy.updateAddress).not.toHaveBeenCalled();
  });

  it('should call updateAddress and navigate on success', fakeAsync(() => {
    addressServiceSpy.updateAddress.and.returnValue(of(null));

    component.updateAddress();
    expect(component.submitted).toBeTrue();
    expect(addressServiceSpy.updateAddress).toHaveBeenCalledWith(
      authServiceStub.decodedToken.sub,
      mockAddress.id,
      jasmine.objectContaining({ alias: mockAddress.alias })
    );

    tick();

    expect(alertifySpy.success).toHaveBeenCalledWith('Pomyślnie zapisano zmiany.');
    expect(routerSpy.navigate).toHaveBeenCalledWith(['/moje-konto/adresy']);
  }));

  it('should show error message if updateAddress fails', fakeAsync(() => {
    const errorMsg = 'Błąd zapisu';
    addressServiceSpy.updateAddress.and.returnValue(throwError(() => errorMsg));

    component.updateAddress();
    tick();

    expect(alertifySpy.error).toHaveBeenCalledWith(errorMsg);
    expect(routerSpy.navigate).not.toHaveBeenCalled();
  }));
});
