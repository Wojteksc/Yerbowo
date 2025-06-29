import { ComponentFixture, fakeAsync, TestBed, tick } from '@angular/core/testing';
import { HomeComponent } from './home.component';
import { AuthService } from 'src/app/auth/services/auth.service';
import { CartService } from 'src/app/cart/services/cart.service';
import { DataService } from 'src/app/core/services/data.service';
import { of, Subject } from 'rxjs';
import { Router, NavigationStart } from '@angular/router';

describe('HomeComponent', () => {
  let component: HomeComponent;
  let fixture: ComponentFixture<HomeComponent>;

  let mockCartTotal$: Subject<number>;
  let mockRouterEvents$: Subject<any>;

  const authServiceMock = {
    restoreSession: jasmine.createSpy('restoreSession')
  };

  const cartServiceMock = {
    getTotalCartProducts: jasmine.createSpy('getTotalCartProducts').and.returnValue(of(5))
  };

  const dataServiceMock = {
    changeTotalCartProducts: jasmine.createSpy('changeTotalCartProducts'),
    cartTotal$: new Subject<number>()
  };

  const routerMock = {
    events: new Subject<any>()
  };

  beforeEach(async () => {
    mockCartTotal$ = dataServiceMock.cartTotal$;
    mockRouterEvents$ = routerMock.events;

    await TestBed.configureTestingModule({
      declarations: [HomeComponent],
      providers: [
        { provide: AuthService, useValue: authServiceMock },
        { provide: CartService, useValue: cartServiceMock },
        { provide: DataService, useValue: dataServiceMock },
        { provide: Router, useValue: routerMock }
      ]
    }).compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(HomeComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  afterEach(() => {
    mockCartTotal$.complete();
    mockRouterEvents$.complete();
  });

  it('should create the component', () => {
    expect(component).toBeTruthy();
  });

  it('should call restoreSession on init', () => {
    expect(authServiceMock.restoreSession).toHaveBeenCalled();
  });
});
