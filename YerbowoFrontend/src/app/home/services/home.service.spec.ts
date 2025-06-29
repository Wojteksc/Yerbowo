import { TestBed } from '@angular/core/testing';
import { HttpClientTestingModule, HttpTestingController } from '@angular/common/http/testing';
import { HomeService } from './home.service';
import { environment } from 'src/environments/environment';
import { HomeProducts } from '../../shared/models/homeProducts';
import { ProductState } from '../../shared/enums/productState.enum';

describe('HomeService', () => {
  let service: HomeService;
  let httpMock: HttpTestingController;

  const mockHomeProducts: HomeProducts = {
    bestsellers: [
      {
        id: 1,
        name: 'Produkt A',
        categorySlug: 'elektronika',
        subcategorySlug: 'smartfony',
        price: 1000,
        oldPrice: 1200,
        state: ProductState.Bestseller,
        image: 'image-a.jpg',
        slug: 'produkt-a',
        createdAt: new Date('2024-01-01'),
      }
    ],
    news: [],
    recommended: [],
    promotions: [],
  };

  beforeEach(() => {
    TestBed.configureTestingModule({
      imports: [HttpClientTestingModule],
      providers: [HomeService],
    });

    service = TestBed.inject(HomeService);
    httpMock = TestBed.inject(HttpTestingController);
  });

  afterEach(() => {
    httpMock.verify();
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });

  it('should return HomeProducts from the API', () => {
    service.getProducts().subscribe((res) => {
      expect(res).toEqual(mockHomeProducts);
      expect(res.bestsellers.length).toBe(1);
      expect(res.bestsellers[0].name).toBe('Produkt A');
    });

    const req = httpMock.expectOne(`${environment.apiUrl}home`);
    expect(req.request.method).toBe('GET');

    req.flush(mockHomeProducts);
  });
});
