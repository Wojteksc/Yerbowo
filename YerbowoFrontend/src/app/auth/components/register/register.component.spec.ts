import { ComponentFixture, TestBed, fakeAsync, tick } from '@angular/core/testing';
import { ReactiveFormsModule } from '@angular/forms';
import { Router } from '@angular/router';
import { of, throwError } from 'rxjs';

import { RegisterComponent } from './register.component';
import { AuthService } from 'src/app/auth/services/auth.service';
import { AlertifyService } from 'src/app/core/services/alertify.service';

describe('RegisterComponent', () => {
  let component: RegisterComponent;
  let fixture: ComponentFixture<RegisterComponent>;
  let authServiceSpy: jasmine.SpyObj<AuthService>;
  let alertifySpy: jasmine.SpyObj<AlertifyService>;
  let routerSpy: jasmine.SpyObj<Router>;

  beforeEach(async () => {
    authServiceSpy = jasmine.createSpyObj('AuthService', ['register']);
    alertifySpy = jasmine.createSpyObj('AlertifyService', ['success', 'error']);
    routerSpy = jasmine.createSpyObj('Router', ['navigate']);

    await TestBed.configureTestingModule({
      declarations: [RegisterComponent],
      imports: [ReactiveFormsModule],
      providers: [
        { provide: AuthService, useValue: authServiceSpy },
        { provide: AlertifyService, useValue: alertifySpy },
        { provide: Router, useValue: routerSpy }
      ]
    }).compileComponents();

    fixture = TestBed.createComponent(RegisterComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create the component and initialize form controls', () => {
    expect(component).toBeTruthy();
    const form = component.registerForm;
    expect(form).toBeTruthy();
    expect(form.get('firstName')).toBeTruthy();
    expect(form.get('lastName')).toBeTruthy();
    expect(form.get('email')).toBeTruthy();
    expect(form.get('confirmEmail')).toBeTruthy();
    expect(form.get('password')).toBeTruthy();
    expect(form.get('confirmPassword')).toBeTruthy();
  });

  it('should mark required fields as invalid when empty', () => {
    const form = component.registerForm;
    form.patchValue({
      firstName: '',
      lastName: '',
      email: '',
      confirmEmail: '',
      password: '',
      confirmPassword: '',
    });

    expect(form.get('firstName').valid).toBeFalse();
    expect(form.get('lastName').valid).toBeFalse();
    expect(form.get('email').valid).toBeFalse();
    expect(form.get('confirmEmail').valid).toBeFalse();
    expect(form.get('password').valid).toBeFalse();
    expect(form.get('confirmPassword').valid).toBeFalse();
  });

  it('should validate that email and confirmEmail match', () => {
    component.registerForm.patchValue({
      email: 'test@example.com',
      confirmEmail: 'mismatch@example.com'
    });
    const errors = component.registerForm.errors || {};
    expect(errors['mismatchEmail']).toBeTrue();

    component.registerForm.patchValue({
      confirmEmail: 'test@example.com'
    });
    expect(component.registerForm.errors).toBeNull();
  });

  it('should validate that password and confirmPassword match', () => {
    component.registerForm.patchValue({
      password: '123456',
      confirmPassword: '654321'
    });
    const errors = component.registerForm.errors || {};
    expect(errors['mismatchPassword']).toBeTrue();

    component.registerForm.patchValue({
      confirmPassword: '123456'
    });
    expect(component.registerForm.errors).toBeNull();
  });

  it('should call authService.register and navigate on success', fakeAsync(() => {
    const formValue = {
      firstName: 'Jan',
      lastName: 'Kowalski',
      companyName: 'Firma',
      email: 'jan@kowalski.pl',
      confirmEmail: 'jan@kowalski.pl',
      password: 'pass123',
      confirmPassword: 'pass123'
    };
    component.registerForm.setValue(formValue);

    authServiceSpy.register.and.returnValue(of({}));
    component.register();

    expect(component.submitted).toBeTrue();
    expect(authServiceSpy.register).toHaveBeenCalledWith(jasmine.objectContaining(formValue));
    tick();

    expect(alertifySpy.success).toHaveBeenCalled();
    expect(routerSpy.navigate).toHaveBeenCalledWith(['']);
  }));

  it('should show error alert on registration failure', fakeAsync(() => {
    const formValue = {
      firstName: 'Jan',
      lastName: 'Kowalski',
      companyName: 'Firma',
      email: 'jan@kowalski.pl',
      confirmEmail: 'jan@kowalski.pl',
      password: 'pass123',
      confirmPassword: 'pass123'
    };
    component.registerForm.setValue(formValue);

    const errorMsg = 'Error during registration';
    authServiceSpy.register.and.returnValue(throwError(() => errorMsg));
    component.register();

    tick();

    expect(alertifySpy.error).toHaveBeenCalledWith(errorMsg);
    expect(routerSpy.navigate).not.toHaveBeenCalled();
  }));

  it('should not submit if form is invalid', () => {
    component.registerForm.patchValue({
      firstName: '',
      lastName: '',
      email: '',
      confirmEmail: '',
      password: '',
      confirmPassword: ''
    });

    component.register();
    expect(authServiceSpy.register).not.toHaveBeenCalled();
  });
});
