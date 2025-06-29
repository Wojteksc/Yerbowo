import { ComponentFixture, TestBed } from '@angular/core/testing';
import { OrderHistoryDetailComponent } from './order-history-detail.component';
import { ActivatedRoute } from '@angular/router';
import { of } from 'rxjs';
import { DebugElement } from '@angular/core';
import { By } from '@angular/platform-browser';

describe('OrderHistoryDetailComponent', () => {
  let component: OrderHistoryDetailComponent;
  let fixture: ComponentFixture<OrderHistoryDetailComponent>;
  let activatedRouteStub: any;

  const mockOrder = {
    id: 12345,
    totalCost: 150.75,
    address: {
      id: 1,
      userId: 1,
      alias: 'Home',
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
    },
    orderItems: [
      {
        productName: 'Yerba Mate',
        productImage: 'yerba.png',
        productCategorySlug: 'napoje',
        productSubcategorySlug: 'yerba',
        productSlug: 'yerba-mate',
        quantity: 2,
        price: 50.25,
        sum: 100.50
      },
      {
        productName: 'Bombilla',
        productImage: 'bombilla.png',
        productCategorySlug: 'akcesoria',
        productSubcategorySlug: 'bombille',
        productSlug: 'bombilla',
        quantity: 1,
        price: 50.25,
        sum: 50.25
      }
    ]
  };

  beforeEach(async () => {
    activatedRouteStub = {
      data: of({ orderDetail: mockOrder })
    };

    await TestBed.configureTestingModule({
      declarations: [OrderHistoryDetailComponent],
      providers: [
        { provide: ActivatedRoute, useValue: activatedRouteStub }
      ]
    }).compileComponents();

    fixture = TestBed.createComponent(OrderHistoryDetailComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create the component and load order data', () => {
    expect(component).toBeTruthy();
    expect(component.order).toEqual(mockOrder);
  });

  it('should render order ID in template', () => {
    const compiled = fixture.nativeElement as HTMLElement;
    expect(compiled.querySelector('p.text-big b')?.textContent).toContain(mockOrder.id.toString());
  });

  it('should render correct number of order items', () => {
    const productElements = fixture.nativeElement.querySelectorAll('.grid-item-product div.cursor-pointer');
    expect(productElements.length).toBe(mockOrder.orderItems.length);
    expect(productElements[0].textContent).toContain('Yerba Mate');
    expect(productElements[1].textContent).toContain('Bombilla');
  });

  it('should render total cost formatted with 2 decimals', () => {
    const totalCostEl = fixture.nativeElement.querySelector('.grid-item-total-value');
    expect(totalCostEl.textContent).toContain(mockOrder.totalCost.toFixed(2));
  });

  it('should render delivery address phone', () => {
    const phoneEl = fixture.nativeElement.querySelector('.grid-item-contact-details p:nth-child(2)');
    expect(phoneEl.textContent).toContain(mockOrder.address.phone);
  });

  it('should render delivery address full info', () => {
    const deliveryAddressEl = fixture.nativeElement.querySelector('.grid-item-delivery-address');
    expect(deliveryAddressEl.textContent).toContain(mockOrder.address.firstName);
    expect(deliveryAddressEl.textContent).toContain(mockOrder.address.lastName);
    expect(deliveryAddressEl.textContent).toContain(mockOrder.address.street);
    expect(deliveryAddressEl.textContent).toContain(mockOrder.address.buildingNumber);
    expect(deliveryAddressEl.textContent).toContain(mockOrder.address.apartmentNumber);
    expect(deliveryAddressEl.textContent).toContain(mockOrder.address.postCode);
    expect(deliveryAddressEl.textContent).toContain(mockOrder.address.place);
  });
});
