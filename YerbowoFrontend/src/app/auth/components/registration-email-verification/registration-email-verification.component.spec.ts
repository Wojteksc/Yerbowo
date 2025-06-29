import { ComponentFixture, TestBed } from '@angular/core/testing';
import { RegistrationEmailVerificationComponent } from './registration-email-verification.component';
import { ActivatedRoute, Router } from '@angular/router';
import { AuthService } from '../../services/auth.service';
import { AlertifyService } from 'src/app/core/services/alertify.service';
import { of } from 'rxjs';

describe('RegistrationEmailVerificationComponent', () => {
  let component: RegistrationEmailVerificationComponent;
  let fixture: ComponentFixture<RegistrationEmailVerificationComponent>;

  const navigateSpy = jasmine.createSpy('navigate');

  const authServiceMock = {
    confirmEmail: jasmine.createSpy('confirmEmail')
  };

  const alertifyMock = {
    success: jasmine.createSpy('success'),
    error: jasmine.createSpy('error')
  };

  const routerMock = {
    navigate: navigateSpy
  };

  let activatedRouteMock = {
    queryParams: of({ email: 'test@example.com', token: 'abc123' })
  };

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [RegistrationEmailVerificationComponent],
      providers: [
        { provide: AuthService, useValue: authServiceMock },
        { provide: AlertifyService, useValue: alertifyMock },
        { provide: Router, useValue: routerMock },
        { provide: ActivatedRoute, useValue: activatedRouteMock }
      ]
    }).compileComponents();
  });

  beforeEach(() => {
    authServiceMock.confirmEmail.calls.reset();
    alertifyMock.success.calls.reset();
    alertifyMock.error.calls.reset();
    routerMock.navigate.calls.reset();

    fixture = TestBed.createComponent(RegistrationEmailVerificationComponent);
    component = fixture.componentInstance;
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should show error if email or token is missing', async () => {
    activatedRouteMock = { queryParams: of({ email: null, token: null }) };


    await TestBed.resetTestingModule();

    await TestBed.configureTestingModule({
      declarations: [RegistrationEmailVerificationComponent],
      providers: [
        { provide: AuthService, useValue: authServiceMock },
        { provide: AlertifyService, useValue: alertifyMock },
        { provide: Router, useValue: routerMock },
        { provide: ActivatedRoute, useValue: activatedRouteMock }
      ]
    }).compileComponents();

    fixture = TestBed.createComponent(RegistrationEmailVerificationComponent);
    component = fixture.componentInstance;

    fixture.detectChanges();

    expect(alertifyMock.error).toHaveBeenCalledWith('Nieprawidłowy link aktywacyjny.');
    expect(routerMock.navigate).toHaveBeenCalledWith(['/']);
  });
});