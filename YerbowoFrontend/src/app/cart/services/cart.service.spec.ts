import { TestBed } from '@angular/core/testing';
import { HttpClientTestingModule, HttpTestingController } from '@angular/common/http/testing';
import { CartService } from './cart.service';
import { ProductState } from '../../shared/enums/productState.enum';
import { environment } from 'src/environments/environment';
import { CartProductItem } from '../models/cartProductItem';
import { CartItem } from '../models/cartItem';
import { Cart } from '../models/cart';

describe('CartService', () => {
  let service: CartService;
  let httpMock: HttpTestingController;
  const baseUrl = environment.apiUrl + 'cart';

  const exampleProduct: CartProductItem = {
    id: 1,
    code: 'P001',
    name: 'Test product',
    description: 'Test description',
    price: 100,
    oldPrice: 120,
    stock: 10,
    state: ProductState.Bestseller,
    image: 'image-url',
    categorySlug: 'category',
    subcategorySlug: 'subcategory',
    slug: 'test-product'
  };

  const exampleCartItem: CartItem = {
    product: exampleProduct,
    quantity: 2
  };

  const exampleCart: Cart = {
    items: [exampleCartItem],
    sum: exampleProduct.price * exampleCartItem.quantity,
    totalItems: exampleCartItem.quantity
  };

  beforeEach(() => {
    TestBed.configureTestingModule({
      imports: [HttpClientTestingModule],
      providers: [CartService]
    });
    service = TestBed.inject(CartService);
    httpMock = TestBed.inject(HttpTestingController);
  });

  afterEach(() => {
    httpMock.verify();
  });

  it('should retrieve the cart', () => {
    service.get().subscribe(cart => {
      expect(cart).toEqual(exampleCart);
    });

    const req = httpMock.expectOne(baseUrl);
    expect(req.request.method).toBe('GET');
    req.flush(exampleCart);
  });

  it('should retrieve total cart products count', () => {
    const totalCount = exampleCart.totalItems;

    service.getTotalCartProducts().subscribe(count => {
      expect(count).toBe(totalCount);
    });

    const req = httpMock.expectOne(`${baseUrl}/totalCartProducts`);
    expect(req.request.method).toBe('GET');
    req.flush(totalCount);
  });

  it('should add product to cart', () => {
    const productId = exampleProduct.id;
    const quantity = 3;

    service.add(productId, quantity).subscribe(cart => {
      expect(cart).toEqual(exampleCart);
    });

    const req = httpMock.expectOne(baseUrl);
    expect(req.request.method).toBe('POST');
    expect(req.request.body).toEqual({ id: productId, quantity });
    req.flush(exampleCart);
  });

  it('should update product quantity in cart', () => {
    const productId = exampleProduct.id;
    const quantity = 5;

    service.update(productId, quantity).subscribe(cart => {
      expect(cart).toEqual(exampleCart);
    });

    const req = httpMock.expectOne(`${baseUrl}/${productId}`);
    expect(req.request.method).toBe('PUT');
    expect(req.request.body).toEqual({ id: productId, quantity });
    req.flush(exampleCart);
  });

  it('should remove product from cart', () => {
    const productId = exampleProduct.id;

    service.remove(productId).subscribe(cart => {
      expect(cart).toEqual(exampleCart);
    });

    const req = httpMock.expectOne(`${baseUrl}/${productId}`);
    expect(req.request.method).toBe('DELETE');
    req.flush(exampleCart);
  });
});
