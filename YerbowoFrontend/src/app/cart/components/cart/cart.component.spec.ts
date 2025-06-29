import { ComponentFixture, TestBed } from '@angular/core/testing';
import { CartComponent } from './cart.component';
import { ActivatedRoute } from '@angular/router';
import { of, throwError, BehaviorSubject } from 'rxjs';
import { CartService } from 'src/app/cart/services/cart.service';
import { AlertifyService } from 'src/app/core/services/alertify.service';
import { DataService } from 'src/app/core/services/data.service';
import { Cart } from 'src/app/cart/models/cart';

describe('CartComponent', () => {
  let component: CartComponent;
  let fixture: ComponentFixture<CartComponent>;
  let mockCartService: jasmine.SpyObj<CartService>;
  let mockAlertifyService: jasmine.SpyObj<AlertifyService>;
  let dataService: DataService;

  const mockCart: Cart = {
    items: [
      {
        product: { id: 1, name: 'Yerba', price: 20, stock: 10, code: '', description: '', oldPrice: 0, state: 0, image: '', categorySlug: '', subcategorySlug: '', slug: '' },
        quantity: 2
      }
    ],
    sum: 40,
    totalItems: 2
  };

  beforeEach(async () => {
    mockCartService = jasmine.createSpyObj('CartService', ['update', 'remove']);
    mockAlertifyService = jasmine.createSpyObj('AlertifyService', ['error', 'success']);
    const dataServiceMock = {
      cartTotal$: new BehaviorSubject<number>(2),
      changeTotalCartProducts: jasmine.createSpy('changeTotalCartProducts')
    };

    await TestBed.configureTestingModule({
      declarations: [CartComponent],
      providers: [
        { provide: CartService, useValue: mockCartService },
        { provide: AlertifyService, useValue: mockAlertifyService },
        { provide: ActivatedRoute, useValue: { data: of({ cart: mockCart }) } },
        { provide: DataService, useValue: dataServiceMock }
      ]
    }).compileComponents();

    fixture = TestBed.createComponent(CartComponent);
    component = fixture.componentInstance;
    dataService = TestBed.inject(DataService);
    fixture.detectChanges();
  });

  it('should initialize cart data', () => {
    expect(component.cart).toEqual(mockCart);
  });

  it('should return true if cart has items', () => {
    component.cart = {
      items: [{
        product: { id: 1, name: 'Yerba', price: 20, stock: 10, code: '', description: '', oldPrice: 0, state: 0, image: '', categorySlug: '', subcategorySlug: '', slug: '' },
        quantity: 2
      }],
      sum: 40,
      totalItems: 2
    };
    expect(component.isAnyCartItem()).toBeTrue();
  });

  it('should return false if cart is empty', () => {
    component.cart = { items: [], sum: 0, totalItems: 0 };
    expect(component.isAnyCartItem()).toBeFalse();
  });

  it('should not update cart if new quantity < 1 in saveRange', () => {
    const spy = spyOn(component as any, 'updateCart');
    component.saveRange(1, 0);
    expect(spy).not.toHaveBeenCalled();
  });

  it('should call updateCart in saveRange with valid value', () => {
    const spy = spyOn(component as any, 'updateCart');
    component.saveRange(1, 2);
    expect(spy).toHaveBeenCalledWith(1, 2);
  });

  it('should call updateCart in handleMinus', () => {
    const spy = spyOn(component as any, 'updateCart');
    component.handleMinus(1, 1);
    expect(spy).toHaveBeenCalledWith(1, 1);
  });

  it('should not call updateCart if stock exceeded in handlePlus', () => {
    component.cart = mockCart;
    const spy = spyOn(component as any, 'updateCart');
    component.handlePlus(1, 11);
    expect(spy).not.toHaveBeenCalled();
  });

  it('should call updateCart in handlePlus if stock not exceeded', () => {
    component.cart = mockCart;
    const spy = spyOn(component as any, 'updateCart');
    component.handlePlus(1, 2);
    expect(spy).toHaveBeenCalledWith(1, 2);
  });

  it('should update cart and notify on updateCart', () => {
    const updatedCart = { ...mockCart, totalItems: 3 };
    mockCartService.update.and.returnValue(of(updatedCart));

    component['updateCart'](1, 3);
    expect(component.cart).toEqual(updatedCart);
    expect(dataService.changeTotalCartProducts).toHaveBeenCalledWith(3);
  });

  it('should handle error on updateCart', () => {
    mockCartService.update.and.returnValue(throwError(() => 'error'));
    component['updateCart'](1, 2);
    expect(mockAlertifyService.error).toHaveBeenCalledWith('error');
  });

  it('should delete product and update cart', () => {
    const newCart = { ...mockCart, items: [], totalItems: 0 };
    mockCartService.remove.and.returnValue(of(newCart));

    component.deleteProduct(1);
    expect(component.cart).toEqual(newCart);
    expect(mockAlertifyService.success).toHaveBeenCalled();
    expect(dataService.changeTotalCartProducts).toHaveBeenCalledWith(0);
  });

  it('should show error on failed delete', () => {
    mockCartService.remove.and.returnValue(throwError(() => 'fail'));
    component.deleteProduct(1);
    expect(mockAlertifyService.error).toHaveBeenCalled();
  });
});
