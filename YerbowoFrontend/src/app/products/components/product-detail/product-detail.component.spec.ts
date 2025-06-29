import { ComponentFixture, TestBed, fakeAsync, tick } from '@angular/core/testing';
import { ProductDetailComponent } from './product-detail.component';
import { ActivatedRoute } from '@angular/router';
import { CartService } from 'src/app/cart/services/cart.service';
import { AlertifyService } from 'src/app/core/services/alertify.service';
import { DataService } from 'src/app/core/services/data.service';
import { of, throwError, Subject } from 'rxjs';
import { ReactiveFormsModule } from '@angular/forms';

describe('ProductDetailComponent', () => {
  let component: ProductDetailComponent;
  let fixture: ComponentFixture<ProductDetailComponent>;

  const productMock = {
    id: 1,
    code: 'P001',
    name: 'Yerba Mate',
    description: '<p>Description</p>',
    price: 12.34,
    oldPrice: 15.00,
    stock: 5,
    state: 0,
    image: 'yerba.jpg',
    category: 'Herbata',
    subcategory: 'Zielona'
  };

  const activatedRouteMock = {
    data: of({ product: productMock })
  };

  const cartServiceMock = {
    add: jasmine.createSpy('add').and.returnValue(of({ totalItems: 3 }))
  };

  const alertifyServiceMock = {
    success: jasmine.createSpy('success'),
    error: jasmine.createSpy('error')
  };

  const cartTotalSubject = new Subject<number>();
  const dataServiceMock = {
    cartTotal$: cartTotalSubject.asObservable(),
    changeTotalCartProducts: jasmine.createSpy('changeTotalCartProducts')
  };

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ProductDetailComponent],
      imports: [ReactiveFormsModule],
      providers: [
        { provide: ActivatedRoute, useValue: activatedRouteMock },
        { provide: CartService, useValue: cartServiceMock },
        { provide: AlertifyService, useValue: alertifyServiceMock },
        { provide: DataService, useValue: dataServiceMock }
      ]
    }).compileComponents();

    fixture = TestBed.createComponent(ProductDetailComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create and initialize form with product', () => {
    expect(component).toBeTruthy();
    expect(component.product).toEqual(productMock);
    expect(component.productForm).toBeDefined();
    expect(component.productForm.controls['id'].value).toBe(productMock.id);
    expect(component.productForm.controls['quantity'].value).toBe(1);
  });

  it('should disable submit button if form invalid', () => {
    component.productForm.controls['quantity'].setValue(null);
    fixture.detectChanges();
    const btn: HTMLButtonElement = fixture.nativeElement.querySelector('button[type="submit"]');
    expect(btn.disabled).toBeTrue();
  });

  it('should show alertify error on submit failure', fakeAsync(() => {
    cartServiceMock.add.and.returnValue(throwError(() => 'error'));
    component.productForm.controls['quantity'].setValue(1);

    component.onSubmit();

    tick();

    expect(alertifyServiceMock.error).toHaveBeenCalledWith('error');
  }));

  it('should update totalCartProducts when dataService emits new value', () => {
    cartTotalSubject.next(10);
    expect(component.totalCartProducts).toBe(10);
  });
});
