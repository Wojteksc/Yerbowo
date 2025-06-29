import { ComponentFixture, TestBed } from '@angular/core/testing';
import { AddressCardComponent } from './address-card.component';
import { AlertifyService } from 'src/app/core/services/alertify.service';
import { AddressCard } from 'src/app/account/models/addressCard';
import { RouterTestingModule } from '@angular/router/testing';
import { By } from '@angular/platform-browser';

describe('AddressCardComponent', () => {
  let component: AddressCardComponent;
  let fixture: ComponentFixture<AddressCardComponent>;
  let alertifySpy: jasmine.SpyObj<AlertifyService>;
  
  const mockAddress: AddressCard = {
    id: 1,
    userId: 100,
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
    const alertifyMock = jasmine.createSpyObj('AlertifyService', ['success', 'error']);

    await TestBed.configureTestingModule({
      declarations: [AddressCardComponent],
      imports: [RouterTestingModule],
      providers: [{ provide: AlertifyService, useValue: alertifyMock }]
    }).compileComponents();

    alertifySpy = TestBed.inject(AlertifyService) as jasmine.SpyObj<AlertifyService>;
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(AddressCardComponent);
    component = fixture.componentInstance;
    component.address = { ...mockAddress };
    fixture.detectChanges();
  });

  it('should create the component', () => {
    expect(component).toBeTruthy();
  });

  it('should return "buildingNumber/apartmentNumber" when apartmentNumber exists', () => {
    expect(component.getAddressDelivery()).toBe('10/5');
  });

  it('should return only "buildingNumber" when apartmentNumber is empty', () => {
    component.address.apartmentNumber = '';
    expect(component.getAddressDelivery()).toBe('10');
  });

  it('should emit onDeleteAddress event when delete() is called', () => {
    spyOn(component.onDeleteAddress, 'emit');
    component.delete();
    expect(component.onDeleteAddress.emit).toHaveBeenCalled();
  });

  it('should display correct data in template', () => {
    const compiled = fixture.nativeElement as HTMLElement;

    expect(compiled.querySelector('h4')?.textContent).toContain(mockAddress.alias);
    expect(compiled.querySelector('address')?.textContent).toContain(mockAddress.firstName);
    expect(compiled.querySelector('address')?.textContent).toContain(mockAddress.lastName);
    expect(compiled.querySelector('address')?.textContent).toContain(mockAddress.street);
    expect(compiled.querySelector('address')?.textContent).toContain('10/5');
    expect(compiled.querySelector('address')?.textContent).toContain(mockAddress.postCode);
    expect(compiled.querySelector('address')?.textContent).toContain(mockAddress.place);
    expect(compiled.querySelector('address')?.textContent).toContain(mockAddress.phone);
  });

  it('should call delete() method on clicking "Usuń"', () => {
    spyOn(component, 'delete');

    const removeButton = fixture.debugElement.query(By.css('.address-card-footer-text-remove'));
    removeButton.triggerEventHandler('click', null);

    expect(component.delete).toHaveBeenCalled();
  });
});
