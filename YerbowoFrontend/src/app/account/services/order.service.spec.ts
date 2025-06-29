import { TestBed } from '@angular/core/testing';
import { HttpClientTestingModule, HttpTestingController } from '@angular/common/http/testing';
import { OrderService } from './order.service';
import { OrderHistoryItem } from '../models/orderHistoryItem';
import { OrderHistoryDetail } from '../models/orderHistoryDetail';
import { environment } from 'src/environments/environment';

describe('OrderService', () => {
  let service: OrderService;
  let httpMock: HttpTestingController;
  const baseUrl = environment.apiUrl;
  const userId = 1;
  const orderId = 42;

  beforeEach(() => {
    TestBed.configureTestingModule({
      imports: [HttpClientTestingModule],
      providers: [OrderService]
    });

    service = TestBed.inject(OrderService);
    httpMock = TestBed.inject(HttpTestingController);
  });

  afterEach(() => {
    httpMock.verify();
  });

  it('should retrieve order history items', () => {
    const mockOrders: OrderHistoryItem[] = [
      {
        id: 1,
        productImages: [
          { name: 'image1.jpg', quantity: 1 },
          { name: 'image2.jpg', quantity: 2 }
        ],
        date: '2023-05-10',
        total: 199.99,
        status: 'Delivered'
      }
    ];

    service.getOrders(userId).subscribe(data => {
      expect(data.length).toBe(1);
      expect(data).toEqual(mockOrders);
    });

    const req = httpMock.expectOne(`${baseUrl}users/${userId}/orders`);
    expect(req.request.method).toBe('GET');
    req.flush(mockOrders);
  });

  it('should retrieve order history detail', () => {
    const mockOrderDetail: OrderHistoryDetail = {
      id: orderId,
      totalCost: 299.99,
      address: {
        id: 1,
        userId: userId,
        alias: 'Home',
        firstName: 'John',
        lastName: 'Doe',
        street: 'Main St',
        buildingNumber: '12A',
        apartmentNumber: '34',
        place: 'Cityville',
        postCode: '00-000',
        phone: '123456789',
        email: 'john@example.com',
        nip: '1234567890',
        company: 'JD Ltd'
      },
      orderItems: [
        {
          productName: 'Product A',
          productImage: 'img-a.jpg',
          productCategorySlug: 'electronics',
          productSubcategorySlug: 'phones',
          productSlug: 'product-a',
          quantity: 2,
          price: 150,
          sum: 300
        }
      ]
    };

    service.getOrderHistoryDetail(userId, orderId).subscribe(data => {
      expect(data).toEqual(mockOrderDetail);
      expect(data.orderItems.length).toBe(1);
    });

    const req = httpMock.expectOne(`${baseUrl}users/${userId}/orders/${orderId}`);
    expect(req.request.method).toBe('GET');
    req.flush(mockOrderDetail);
  });
});
