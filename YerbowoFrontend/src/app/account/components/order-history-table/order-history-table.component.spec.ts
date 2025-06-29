import { ComponentFixture, TestBed } from '@angular/core/testing';
import { OrderHistoryTableComponent } from './order-history-table.component';
import { ActivatedRoute } from '@angular/router';
import { of } from 'rxjs';
import { By } from '@angular/platform-browser';
import { RouterTestingModule } from '@angular/router/testing';
import { Component } from '@angular/core';

@Component({ template: '' })
class DummyComponent {}

describe('OrderHistoryTableComponent', () => {
  let component: OrderHistoryTableComponent;
  let fixture: ComponentFixture<OrderHistoryTableComponent>;

  const mockOrders = [
    {
      id: 1,
      productImages: [
        { name: 'yerba1.png', quantity: 2 },
        { name: 'yerba2.png', quantity: 1 }
      ],
      date: '2025-06-27',
      total: 120,
      status: 'Zrealizowane'
    },
    {
      id: 2,
      productImages: [
        { name: 'yerba3.png', quantity: 3 }
      ],
      date: '2025-06-20',
      total: 90,
      status: 'W trakcie'
    }
  ];

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [
        OrderHistoryTableComponent,
        DummyComponent
      ],
      imports: [
        RouterTestingModule.withRoutes([
          { path: 'moje-konto/zamowienia/:id', component: DummyComponent }
        ])
      ],
      providers: [
        {
          provide: ActivatedRoute,
          useValue: {
            data: of({ orderHistory: mockOrders })
          }
        }
      ]
    }).compileComponents();

    fixture = TestBed.createComponent(OrderHistoryTableComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create component and load orders', () => {
    expect(component).toBeTruthy();
    expect(component.orders.length).toBe(2);
    expect(component.orders).toEqual(mockOrders);
  });

  it('should render order ids in the template', () => {
    const compiled = fixture.nativeElement as HTMLElement;
    const orderNumbers = compiled.querySelectorAll('.grid-item-number-data');
    expect(orderNumbers.length).toBe(mockOrders.length);
    expect(orderNumbers[0].textContent).toContain('1');
    expect(orderNumbers[1].textContent).toContain('2');
  });

  it('should render product images with quantities', () => {
    const compiled = fixture.nativeElement as HTMLElement;
    const imageContainers = compiled.querySelectorAll('.grid-item-image-container');
    expect(imageContainers.length).toBe(3);

    const firstImage = imageContainers[0].querySelector('img')!;
    expect(firstImage.getAttribute('src')).toContain('yerba1.png');

    const firstQuantity = imageContainers[0].querySelector('.circle-quantity')!;
    expect(firstQuantity.textContent).toContain('2 szt.');
  });

  it('should render order dates', () => {
    const compiled = fixture.nativeElement as HTMLElement;
    const rows = compiled.querySelectorAll('.ignore-subgrid');

    expect(rows.length).toBe(2);
    const firstDate = rows[0].querySelector('.grid-item-date')!.textContent!.trim();
    const secondDate = rows[1].querySelector('.grid-item-date')!.textContent!.trim();

    expect(firstDate).toContain('2025-06-27');
    expect(secondDate).toContain('2025-06-20');
  });

  it('should render total order values', () => {
    const compiled = fixture.nativeElement as HTMLElement;
    const valueElements = compiled.querySelectorAll('.grid-item-value');
    const rows = compiled.querySelectorAll('.ignore-subgrid');

    const firstValue = rows[0].querySelector('.grid-item-value')!.textContent!;
    const secondValue = rows[1].querySelector('.grid-item-value')!.textContent!;

    expect(firstValue).toContain('120');
    expect(secondValue).toContain('90');
  });

  it('should render order statuses', () => {
    const compiled = fixture.nativeElement as HTMLElement;
    const rows = compiled.querySelectorAll('.ignore-subgrid');

    const firstStatus = rows[0].querySelector('.grid-item-status b')!.textContent!.trim();
    const secondStatus = rows[1].querySelector('.grid-item-status b')!.textContent!.trim();

    expect(firstStatus).toBe('Zrealizowane');
    expect(secondStatus).toBe('W trakcie');
  });
});
