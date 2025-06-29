import { ComponentFixture, TestBed, fakeAsync, tick } from '@angular/core/testing';
import { AccountEditComponent } from './account-edit.component';
import { ActivatedRoute } from '@angular/router';
import { AuthService } from 'src/app/auth/services/auth.service';
import { UserService } from 'src/app/account/services/user.service';
import { AlertifyService } from 'src/app/core/services/alertify.service';
import { UntypedFormBuilder, ReactiveFormsModule } from '@angular/forms';
import { of, throwError } from 'rxjs';
import { By } from '@angular/platform-browser';

describe('AccountEditComponent', () => {
  let component: AccountEditComponent;
  let fixture: ComponentFixture<AccountEditComponent>;
  let mockUserService, mockAuthService, mockAlertifyService;

  const userMock = {
    id: 1,
    firstName: 'Jan',
    lastName: 'Kowalski',
    companyName: 'FirmaX',
    email: 'jan@example.com',
    confirmEmail: 'jan@example.com',
    newPassword: '',
    confirmPassword: '',
    currentPassword: ''
  };

  beforeEach(async () => {
    mockUserService = jasmine.createSpyObj('UserService', ['updateUser']);
    mockAuthService = { decodedToken: { sub: '123' } };
    mockAlertifyService = jasmine.createSpyObj('AlertifyService', ['success', 'error']);

    await TestBed.configureTestingModule({
      declarations: [AccountEditComponent],
      imports: [ReactiveFormsModule],
      providers: [
        UntypedFormBuilder,
        { provide: UserService, useValue: mockUserService },
        { provide: AuthService, useValue: mockAuthService },
        { provide: AlertifyService, useValue: mockAlertifyService },
        {
          provide: ActivatedRoute,
          useValue: {
            data: of({ user: userMock })
          }
        }
      ]
    }).compileComponents();

    fixture = TestBed.createComponent(AccountEditComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create component and init form with user data', () => {
    expect(component).toBeTruthy();
    expect(component.accountForm).toBeDefined();
    expect(component.accountForm.value.firstName).toBe(userMock.firstName);
    expect(component.accountForm.value.email).toBe(userMock.email);
  });

  it('should invalidate form if required fields are missing', () => {
    component.accountForm.controls['firstName'].setValue('');
    component.accountForm.controls['lastName'].setValue('');
    component.accountForm.controls['email'].setValue('');
    component.accountForm.controls['confirmEmail'].setValue('');
    component.accountForm.controls['currentPassword'].setValue('');
    expect(component.accountForm.valid).toBeFalse();
  });

  it('should set submitted flag to true on updateUser', () => {
    spyOn(component, 'updateUser').and.callThrough();

    component.updateUser();
    expect(component.submitted).toBeTrue();
  });

  it('should call userService.updateUser on valid form submission and show success alert', fakeAsync(() => {
    mockUserService.updateUser.and.returnValue(of({}));
    component.accountForm.controls['currentPassword'].setValue('currentPass');
    component.updateUser();
    tick();

    expect(mockUserService.updateUser).toHaveBeenCalledWith('123', jasmine.objectContaining({
      firstName: userMock.firstName
    }));
    expect(mockAlertifyService.success).toHaveBeenCalledWith('Pomyślnie zapisano zmiany');
  }));

  it('should call alertify.error on updateUser error', fakeAsync(() => {
    const errorMessage = 'Błąd serwera';
    mockUserService.updateUser.and.returnValue(throwError(() => errorMessage));
    component.accountForm.controls['currentPassword'].setValue('currentPass');
    component.updateUser();
    tick();

    expect(mockAlertifyService.error).toHaveBeenCalledWith(errorMessage);
  }));

  it('should not call userService.updateUser if form is invalid', () => {
    component.accountForm.controls['currentPassword'].setValue('');
    component.accountForm.controls['firstName'].setValue('');
    component.updateUser();

    expect(mockUserService.updateUser).not.toHaveBeenCalled();
  });

  it('should display validation errors in template after submit', () => {
    component.submitted = true;
    component.accountForm.controls['firstName'].setValue('');
    fixture.detectChanges();

    const errorDiv = fixture.debugElement.query(By.css('.text-danger'));
    expect(errorDiv.nativeElement.textContent).toContain("Pole 'Imię' jest wymagane.");
  });
});
