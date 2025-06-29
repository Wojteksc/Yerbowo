import { ComponentFixture, TestBed } from '@angular/core/testing';
import { ProductCardComponent } from './product-card.component';
import { UntypedFormBuilder } from '@angular/forms';
import { of, BehaviorSubject } from 'rxjs';
import { CartService } from 'src/app/cart/services/cart.service';
import { AlertifyService } from 'src/app/core/services/alertify.service';
import { DataService } from 'src/app/core/services/data.service';
import { ProductState } from 'src/app/shared/enums/productState.enum';

describe('ProductCardComponent', () => {
  let component: ProductCardComponent;
  let fixture: ComponentFixture<ProductCardComponent>;

  const cartServiceSpy = jasmine.createSpyObj('CartService', ['add']);
  const alertifySpy = jasmine.createSpyObj('AlertifyService', ['success', 'error']);
  const cartTotalSubject = new BehaviorSubject<number>(10);
  const dataServiceSpy = jasmine.createSpyObj('DataService', ['changeTotalCartProducts']);
  dataServiceSpy.cartTotal$ = cartTotalSubject.asObservable();

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ProductCardComponent],
      providers: [
        UntypedFormBuilder,
        { provide: CartService, useValue: cartServiceSpy },
        { provide: AlertifyService, useValue: alertifySpy },
        { provide: DataService, useValue: dataServiceSpy },
      ],
    }).compileComponents();

    fixture = TestBed.createComponent(ProductCardComponent);
    component = fixture.componentInstance;

    component.product = {
      id: 1,
      name: 'Test Product',
      categorySlug: 'cat',
      subcategorySlug: 'subcat',
      price: 100,
      oldPrice: 150,
      state: ProductState.Promotion,
      image: 'image.jpg',
      slug: 'test-product',
      createdAt: new Date(),
    };

    cartServiceSpy.add.and.returnValue(of({ items: [], totalItems: 5, sum: 0 }));

    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should subscribe to cartTotal$ and update totalCartProducts', () => {
    expect(component.totalCartProducts).toBe(10);

    cartTotalSubject.next(20);
    expect(component.totalCartProducts).toBe(20);
  });

  it('should render promotion box when product state is Promotion', () => {
    const promoBox = fixture.nativeElement.querySelector('.promotion-box');
    expect(promoBox).toBeTruthy();
  });

  it('should show old price if product is on promotion and oldPrice differs', () => {
    const oldPriceEl = fixture.nativeElement.querySelector('.product-old-price');
    expect(oldPriceEl).toBeTruthy();
    expect(oldPriceEl.textContent).toContain('150.00 zł');
  });

  it('should call cartService.add, show success alert and update total cart on add()', () => {
    component.add();

    expect(cartServiceSpy.add).toHaveBeenCalledWith(component.product.id);
    expect(alertifySpy.success).toHaveBeenCalledWith('Produkt został dodany do koszyka.');
    expect(dataServiceSpy.changeTotalCartProducts).toHaveBeenCalledWith(5);
  });
});
