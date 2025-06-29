import { TestBed } from '@angular/core/testing';
import { HttpClientTestingModule, HttpTestingController } from '@angular/common/http/testing';
import { ProductService } from './product.service';
import { ProductDetail } from '../models/productDetail';
import { ProductCard } from '../models/productCard';
import { ProductState } from '../../shared/enums/productState.enum';
import { environment } from 'src/environments/environment';

describe('ProductService', () => {
  let service: ProductService;
  let httpMock: HttpTestingController;

  const dummyProduct: ProductDetail = {
    id: 1,
    code: 'ABC123',
    name: 'Produkt A',
    description: 'Opis produktu A',
    price: 100,
    oldPrice: 120,
    stock: 10,
    state: ProductState.Bestseller,
    image: 'img.jpg',
    category: 'elektronika',
    subcategory: 'smartfony'
  };

  const dummyProductCards: ProductCard[] = [
    {
      id: 1,
      name: 'Produkt A',
      categorySlug: 'elektronika',
      subcategorySlug: 'smartfony',
      price: 100,
      oldPrice: 120,
      state: ProductState.New,
      image: 'img.jpg',
      slug: 'produkt-a',
      createdAt: new Date('2024-01-01')
    }
  ];

  beforeEach(() => {
    TestBed.configureTestingModule({
      imports: [HttpClientTestingModule],
      providers: [ProductService]
    });

    service = TestBed.inject(ProductService);
    httpMock = TestBed.inject(HttpTestingController);
  });

  afterEach(() => {
    httpMock.verify();
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });

  it('should fetch a single product by slug', () => {
    service.getProduct('produkt-a').subscribe(product => {
      expect(product).toEqual(dummyProduct);
      expect(product.name).toBe('Produkt A');
    });

    const req = httpMock.expectOne(`${environment.apiUrl}products/produkt-a`);
    expect(req.request.method).toBe('GET');
    req.flush(dummyProduct);
  });

  it('should fetch paginated products', () => {
    const mockPagination = { currentPage: 1, totalItems: 1, itemsPerPage: 10, totalPages: 1 };

    service.getProducts(1, 10, 'elektronika', 'smartfony').subscribe(res => {
      expect(res.result.length).toBe(1);
      expect(res.result[0].name).toBe('Produkt A');
      expect(res.pagination).toEqual(mockPagination);
    });

    const req = httpMock.expectOne(req => req.method === 'GET' && req.url === `${environment.apiUrl}products`);
    expect(req.request.params.get('pageNumber')).toBe('1');
    expect(req.request.params.get('pageSize')).toBe('10');
    expect(req.request.params.get('category')).toBe('elektronika');
    expect(req.request.params.get('subcategory')).toBe('smartfony');

    req.flush(dummyProductCards, {
      headers: {
        Pagination: JSON.stringify(mockPagination)
      }
    });
  });
});
