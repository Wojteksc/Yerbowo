import { ComponentFixture, TestBed, fakeAsync, tick } from '@angular/core/testing';
import { NewsletterEmailSubscribeComponent } from './newsletter-email-subscribe.component';
import { NewsletterService } from 'src/app/newsletter/services/newsletter.service';
import { AlertifyService } from 'src/app/core/services/alertify.service';
import { ActivatedRoute, Router } from '@angular/router';
import { of, throwError, Subject } from 'rxjs';

describe('NewsletterEmailSubscribeComponent', () => {
  let component: NewsletterEmailSubscribeComponent;
  let fixture: ComponentFixture<NewsletterEmailSubscribeComponent>;
  let newsletterServiceSpy: jasmine.SpyObj<NewsletterService>;
  let alertifyServiceSpy: jasmine.SpyObj<AlertifyService>;
  let routerSpy: jasmine.SpyObj<Router>;
  let queryParamsSubject: Subject<any>;

  beforeEach(async () => {
    newsletterServiceSpy = jasmine.createSpyObj('NewsletterService', ['subscribe']);
    alertifyServiceSpy = jasmine.createSpyObj('AlertifyService', ['success', 'error']);
    routerSpy = jasmine.createSpyObj('Router', ['navigate']);
    queryParamsSubject = new Subject();

    await TestBed.configureTestingModule({
      declarations: [NewsletterEmailSubscribeComponent],
      providers: [
        { provide: NewsletterService, useValue: newsletterServiceSpy },
        { provide: AlertifyService, useValue: alertifyServiceSpy },
        { provide: Router, useValue: routerSpy },
        {
          provide: ActivatedRoute,
          useValue: { queryParams: queryParamsSubject.asObservable() },
        },
      ],
    }).compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(NewsletterEmailSubscribeComponent);
    component = fixture.componentInstance;
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should subscribe and navigate on successful subscription', fakeAsync(() => {
    const email = 'test@example.com';
    const token = 'abc123';
    newsletterServiceSpy.subscribe.and.returnValue(of({}));

    fixture.detectChanges();

    queryParamsSubject.next({ email, token });
    tick();

    expect(newsletterServiceSpy.subscribe).toHaveBeenCalledWith({ email, token });
    expect(alertifyServiceSpy.success).toHaveBeenCalledWith('Dziękujemy za zapisanie się do newslettera!', 0);
    expect(routerSpy.navigate).toHaveBeenCalledWith(['/']);
  }));

  it('should show error and navigate on subscription failure', fakeAsync(() => {
    const email = 'test@example.com';
    const token = 'abc123';
    const errorMsg = 'Subscription error';
    newsletterServiceSpy.subscribe.and.returnValue(throwError(() => errorMsg));

    fixture.detectChanges();

    queryParamsSubject.next({ email, token });
    tick();

    expect(newsletterServiceSpy.subscribe).toHaveBeenCalledWith({ email, token });
    expect(alertifyServiceSpy.error).toHaveBeenCalledWith(errorMsg);
    expect(routerSpy.navigate).toHaveBeenCalledWith(['/']);
  }));

  it('should show error and navigate if missing email or token', fakeAsync(() => {
    fixture.detectChanges();

    queryParamsSubject.next({ email: 'test@example.com' });
    tick();

    expect(alertifyServiceSpy.error).toHaveBeenCalledWith('Brak wymaganych parametrów do subskrypcji.');
    expect(routerSpy.navigate).toHaveBeenCalledWith(['/']);

    alertifyServiceSpy.error.calls.reset();
    routerSpy.navigate.calls.reset();

    queryParamsSubject.next({ token: 'abc123' });
    tick();

    expect(alertifyServiceSpy.error).toHaveBeenCalledWith('Brak wymaganych parametrów do subskrypcji.');
    expect(routerSpy.navigate).toHaveBeenCalledWith(['/']);
  }));
});
