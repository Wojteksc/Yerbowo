import { ComponentFixture, TestBed } from '@angular/core/testing';
import { AccountTabsComponent } from './account-tabs.component';
import { AuthService } from 'src/app/auth/services/auth.service';
import { RouterTestingModule } from '@angular/router/testing';
import { By } from '@angular/platform-browser';

describe('AccountTabsComponent', () => {
  let component: AccountTabsComponent;
  let fixture: ComponentFixture<AccountTabsComponent>;
  let authServiceMock: Partial<AuthService>;

  beforeEach(async () => {
    authServiceMock = {
      decodedToken: { unique_name: 'jan.kowalski@example.com' }
    };

    await TestBed.configureTestingModule({
      imports: [RouterTestingModule],
      declarations: [AccountTabsComponent],
      providers: [
        { provide: AuthService, useValue: authServiceMock }
      ]
    }).compileComponents();

    fixture = TestBed.createComponent(AccountTabsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create the component', () => {
    expect(component).toBeTruthy();
  });

  it('should extract username correctly from decodedToken', () => {
    expect(component.userName).toBe('jan.kowalski');
  });

  it('should display username in titlecase in template', () => {
    const h3 = fixture.nativeElement.querySelector('h3');
    expect(h3.textContent).toContain('Jan.kowalski');
  });

  it('should render all 4 account tabs with correct routerLinks and text', () => {
    const links = fixture.debugElement.queryAll(By.css('.grid-account-tabs-item'));
    expect(links.length).toBe(4);

    const expected = [
      { path: '/moje-konto/dane', text: 'Moje konto' },
      { path: '/moje-konto/zamowienia', text: 'Historia zamówień' },
      { path: '/moje-konto/adresy', text: 'Adresy' },
      { path: '/moje-konto/obserwowane', text: 'Obserwowane' }
    ];

    links.forEach((linkDe, index) => {
      const routerLink = linkDe.attributes['ng-reflect-router-link'];
      expect(routerLink).toBe(expected[index].path);
      expect(linkDe.nativeElement.textContent.trim()).toBe(expected[index].text);
    });
  });
});
