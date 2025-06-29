import { ComponentFixture, TestBed, fakeAsync, tick } from '@angular/core/testing';
import { FooterComponent } from './footer.component';
import { ReactiveFormsModule } from '@angular/forms';
import { NewsletterService } from 'src/app/newsletter/services/newsletter.service';
import { AlertifyService } from 'src/app/core/services/alertify.service';
import { of, throwError } from 'rxjs';

describe('FooterComponent', () => {
  let component: FooterComponent;
  let fixture: ComponentFixture<FooterComponent>;
  let newsletterServiceSpy: jasmine.SpyObj<NewsletterService>;
  let alertifyServiceSpy: jasmine.SpyObj<AlertifyService>;

  beforeEach(async () => {
    const newsletterSpy = jasmine.createSpyObj('NewsletterService', ['invite']);
    const alertifySpy = jasmine.createSpyObj('AlertifyService', ['success', 'error']);

    await TestBed.configureTestingModule({
      declarations: [FooterComponent],
      imports: [ReactiveFormsModule],
      providers: [
        { provide: NewsletterService, useValue: newsletterSpy },
        { provide: AlertifyService, useValue: alertifySpy },
      ],
    }).compileComponents();

    newsletterServiceSpy = TestBed.inject(NewsletterService) as jasmine.SpyObj<NewsletterService>;
    alertifyServiceSpy = TestBed.inject(AlertifyService) as jasmine.SpyObj<AlertifyService>;
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(FooterComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create the FooterComponent', () => {
    expect(component).toBeTruthy();
  });

  it('should initialize newsletterForm with email control', () => {
    expect(component.newsletterForm.contains('email')).toBeTrue();
    expect(component.f.email.valid).toBeFalse();
  });

  it('should mark email invalid when empty', () => {
    component.f.email.setValue('');
    expect(component.f.email.invalid).toBeTrue();
    expect(component.f.email.errors?.['required']).toBeTruthy();
  });

  it('should mark email invalid with wrong format', () => {
    component.f.email.setValue('invalidemail');
    expect(component.f.email.invalid).toBeTrue();
    expect(component.f.email.errors?.['email']).toBeTruthy();
  });

  it('should mark email valid with correct format', () => {
    component.f.email.setValue('test@example.com');
    expect(component.f.email.valid).toBeTrue();
  });

  it('should call invite and alert success on valid form submission', () => {
    const testEmail = 'test@example.com';
    component.f.email.setValue(testEmail);
    newsletterServiceSpy.invite.and.returnValue(of('Zaproszenie wysłane'));
    
    component.invite();

    expect(newsletterServiceSpy.invite).toHaveBeenCalledWith(testEmail);
    expect(alertifyServiceSpy.success).toHaveBeenCalledWith('Zaproszenie wysłane');
  });

  it('should not call invite if form invalid', () => {
    component.f.email.setValue('');
    component.invite();
    expect(newsletterServiceSpy.invite).not.toHaveBeenCalled();
  });

  it('should alert error when invite fails', () => {
    const testEmail = 'test@example.com';
    component.f.email.setValue(testEmail);
    const errorMsg = 'Błąd zaproszenia';
    newsletterServiceSpy.invite.and.returnValue(throwError(() => errorMsg));

    component.invite();

    expect(alertifyServiceSpy.error).toHaveBeenCalledWith(errorMsg);
  });
});
