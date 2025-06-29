import { ComponentFixture, TestBed } from '@angular/core/testing';
import { Router } from '@angular/router';
import { NavComponent } from './nav.component';
import { AuthService } from 'src/app/auth/services/auth.service';
import { SocialAuthService } from '@abacritt/angularx-social-login';
import { of } from 'rxjs';

describe('NavComponent', () => {
  let component: NavComponent;
  let fixture: ComponentFixture<NavComponent>;

  let authServiceMock: any;
  let routerMock: any;
  let socialAuthServiceMock: any;

  beforeEach(async () => {
    authServiceMock = {
      isLoggedIn: jasmine.createSpy('isLoggedIn'),
      signOut: jasmine.createSpy('signOut'),
      photoUrl: 'test-photo.jpg',
      decodedToken: { unique_name: 'testuser@example.com' }
    };

    routerMock = {
      navigate: jasmine.createSpy('navigate')
    };

    socialAuthServiceMock = {
      signOut: jasmine.createSpy('signOut')
    };

    await TestBed.configureTestingModule({
      declarations: [NavComponent],
      providers: [
        { provide: AuthService, useValue: authServiceMock },
        { provide: Router, useValue: routerMock },
        { provide: SocialAuthService, useValue: socialAuthServiceMock },
      ],
    }).compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(NavComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create the nav component', () => {
    expect(component).toBeTruthy();
  });

  describe('isLoggedIn', () => {
    it('should return true if authService.isLoggedIn returns true', () => {
      authServiceMock.isLoggedIn.and.returnValue(true);
      expect(component.isLoggedIn()).toBeTrue();
      expect(authServiceMock.isLoggedIn).toHaveBeenCalled();
    });

    it('should return false if authService.isLoggedIn returns false', () => {
      authServiceMock.isLoggedIn.and.returnValue(false);
      expect(component.isLoggedIn()).toBeFalse();
      expect(authServiceMock.isLoggedIn).toHaveBeenCalled();
    });
  });

  describe('logout', () => {
    it('should call signOut on authService and socialAuthService and navigate to root', () => {
      component.logout();

      expect(authServiceMock.signOut).toHaveBeenCalled();
      expect(socialAuthServiceMock.signOut).toHaveBeenCalled();
      expect(routerMock.navigate).toHaveBeenCalledWith(['/']);
    });
  });
});
