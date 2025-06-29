import { ComponentFixture, TestBed, fakeAsync, tick } from '@angular/core/testing';
import { LoginComponent } from './login.component';
import { AuthService } from 'src/app/auth/services/auth.service';
import { AlertifyService } from 'src/app/core/services/alertify.service';
import { SocialAuthService } from '@abacritt/angularx-social-login';
import { Router } from '@angular/router';
import { of, throwError, Subject } from 'rxjs';
import { FormsModule } from '@angular/forms';

describe('LoginComponent', () => {
  let component: LoginComponent;
  let fixture: ComponentFixture<LoginComponent>;
  let authServiceMock: any;
  let alertifyMock: any;
  let routerMock: any;
  let socialAuthServiceMock: any;

  beforeEach(async () => {
    authServiceMock = {
      login: jasmine.createSpy('login').and.returnValue(of(void 0)),
      loginWithSocial: jasmine.createSpy('loginWithSocial').and.returnValue(of(void 0))
    };

    alertifyMock = {
      success: jasmine.createSpy('success'),
      error: jasmine.createSpy('error')
    };

    routerMock = {
      navigate: jasmine.createSpy('navigate')
    };

    socialAuthServiceMock = {
      authState: new Subject(),
      signIn: jasmine.createSpy('signIn').and.resolveTo({ provider: 'FACEBOOK', email: 'fb@example.com' })
    };

    await TestBed.configureTestingModule({
      declarations: [LoginComponent],
      imports: [FormsModule],
      providers: [
        { provide: AuthService, useValue: authServiceMock },
        { provide: AlertifyService, useValue: alertifyMock },
        { provide: Router, useValue: routerMock },
        { provide: SocialAuthService, useValue: socialAuthServiceMock }
      ]
    }).compileComponents();

    fixture = TestBed.createComponent(LoginComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should call AuthService.login and navigate on success', () => {
    component.model = { email: 'test@example.com', password: '1234' };
    component.signIn();

    expect(authServiceMock.login).toHaveBeenCalledWith(component.model);
    expect(alertifyMock.success).toHaveBeenCalledWith('Pomyślnie zalogowano');
    expect(routerMock.navigate).toHaveBeenCalledWith(['']);
  });

  it('should handle login error', () => {
    authServiceMock.login.and.returnValue(throwError(() => 'Błąd logowania'));
    component.model = { email: 'test@example.com', password: '1234' };
    component.signIn();

    expect(alertifyMock.error).toHaveBeenCalledWith('Błąd logowania');
  });

  it('should call Google sign in', () => {
    component.signInWithGoogle();
    expect(socialAuthServiceMock.signIn).toHaveBeenCalledWith('GOOGLE');
  });

  it('should call loginWithSocial on Facebook login success', fakeAsync(() => {
    component.signInWithFB();
    tick();
    expect(authServiceMock.loginWithSocial).toHaveBeenCalled();
    expect(alertifyMock.success).toHaveBeenCalledWith('Pomyślnie zalogowano');
    expect(routerMock.navigate).toHaveBeenCalledWith(['']);
  }));

  it('should handle Facebook login failure', fakeAsync(() => {
    socialAuthServiceMock.signIn.and.rejectWith('FB Error');
    component.signInWithFB();
    tick();
    expect(alertifyMock.error).toHaveBeenCalledWith('Błąd podczas logowania przez Facebook');
  }));

  it('should listen to Google auth state and login', () => {
    socialAuthServiceMock.authState.next({ provider: 'GOOGLE', email: 'test@google.com' });
    expect(authServiceMock.loginWithSocial).toHaveBeenCalled();
  });
});
