import { ComponentFixture, TestBed, fakeAsync, tick } from '@angular/core/testing';
import { AddressAddComponent } from './address-add.component';
import { UntypedFormBuilder, ReactiveFormsModule } from '@angular/forms';
import { AddressService } from 'src/app/account/services/address.service';
import { AuthService } from 'src/app/auth/services/auth.service';
import { AlertifyService } from 'src/app/core/services/alertify.service';
import { Router } from '@angular/router';
import { of, throwError } from 'rxjs';

describe('AddressAddComponent', () => {
  let component: AddressAddComponent;
  let fixture: ComponentFixture<AddressAddComponent>;
  let addressServiceSpy: jasmine.SpyObj<AddressService>;
  let alertifySpy: jasmine.SpyObj<AlertifyService>;
  let routerSpy: jasmine.SpyObj<Router>;

  const authServiceMock = { decodedToken: { sub: 123 } } as AuthService;

  beforeEach(async () => {
    const addressServiceMock = jasmine.createSpyObj('AddressService', ['createAddress']);
    const alertifyServiceMock = jasmine.createSpyObj('AlertifyService', ['success', 'error']);
    const routerMock = jasmine.createSpyObj('Router', ['navigate']);

    await TestBed.configureTestingModule({
      declarations: [AddressAddComponent],
      imports: [ReactiveFormsModule],
      providers: [
        UntypedFormBuilder,
        { provide: AddressService, useValue: addressServiceMock },
        { provide: AuthService, useValue: authServiceMock },
        { provide: AlertifyService, useValue: alertifyServiceMock },
        { provide: Router, useValue: routerMock }
      ]
    }).compileComponents();

    fixture = TestBed.createComponent(AddressAddComponent);
    component = fixture.componentInstance;
    addressServiceSpy = TestBed.inject(AddressService) as jasmine.SpyObj<AddressService>;
    alertifySpy = TestBed.inject(AlertifyService) as jasmine.SpyObj<AlertifyService>;
    routerSpy = TestBed.inject(Router) as jasmine.SpyObj<Router>;

    fixture.detectChanges();
  });

  it('should create form with default values', () => {
    expect(component.addressAddForm).toBeTruthy();
    expect(component.f.alias.value).toBe('');
    expect(component.f.firstName.value).toBe('');
    expect(component.f.userId.value).toBe(123);
  });

  it('should mark form as invalid when required fields are empty', () => {
    component.addressAddForm.patchValue({
      alias: '',
      firstName: '',
      lastName: '',
      street: '',
      buildingNumber: '',
      place: '',
      postCode: '',
      phone: '',
      email: ''
    });
    expect(component.addressAddForm.valid).toBeFalse();
  });

  it('should call createAddress on valid form submission and navigate', fakeAsync(() => {
    const addressData = {
      userId: 123,
      alias: 'dom',
      firstName: 'Jan',
      lastName: 'Kowalski',
      street: 'Mickiewicza',
      buildingNumber: '12',
      apartmentNumber: '5',
      place: 'Warszawa',
      postCode: '00-001',
      phone: '123456789',
      email: 'jan@example.com',
      nip: '1234567890',
      company: 'Firma'
    };

    component.addressAddForm.setValue(addressData);

    addressServiceSpy.createAddress.and.returnValue(of(null));

    component.createAddress();
    tick();

    expect(component.submitted).toBeTrue();
    expect(addressServiceSpy.createAddress).toHaveBeenCalledWith(123, jasmine.objectContaining({
      alias: 'dom',
      firstName: 'Jan',
      lastName: 'Kowalski',
      street: 'Mickiewicza',
      buildingNumber: '12',
      apartmentNumber: '5',
      place: 'Warszawa',
      postCode: '00-001',
      phone: '123456789',
      email: 'jan@example.com',
      nip: '1234567890',
      company: 'Firma'
    }));
    expect(alertifySpy.success).toHaveBeenCalledWith('Pomyślnie utworzono adres.');
    expect(routerSpy.navigate).toHaveBeenCalledWith(['/moje-konto/adresy']);
  }));

  it('should call alertify.error on service error', fakeAsync(() => {
    component.addressAddForm.setValue({
      userId: 123,
      alias: 'dom',
      firstName: 'Jan',
      lastName: 'Kowalski',
      street: 'Mickiewicza',
      buildingNumber: '12',
      apartmentNumber: '5',
      place: 'Warszawa',
      postCode: '00-001',
      phone: '123456789',
      email: 'jan@example.com',
      nip: '1234567890',
      company: 'Firma'
    });

    const errorResponse = 'Błąd serwera';

    addressServiceSpy.createAddress.and.returnValue(throwError(() => errorResponse));

    component.createAddress();
    tick();

    expect(alertifySpy.error).toHaveBeenCalledWith(errorResponse);
    expect(routerSpy.navigate).not.toHaveBeenCalled();
  }));

  it('should not submit if form is invalid', () => {
    component.addressAddForm.patchValue({
      alias: '',
      firstName: '',
      lastName: '',
      street: '',
      buildingNumber: '',
      place: '',
      postCode: '',
      phone: '',
      email: ''
    });

    component.createAddress();

    expect(component.submitted).toBeTrue();
    expect(addressServiceSpy.createAddress).not.toHaveBeenCalled();
    expect(alertifySpy.success).not.toHaveBeenCalled();
    expect(routerSpy.navigate).not.toHaveBeenCalled();
  });
});
