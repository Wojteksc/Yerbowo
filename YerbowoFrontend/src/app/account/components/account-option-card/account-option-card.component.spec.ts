import { ComponentFixture, TestBed } from '@angular/core/testing';
import { AccountOptionCardComponent } from './account-option-card.component';
import { RouterTestingModule } from '@angular/router/testing';
import { By } from '@angular/platform-browser';

describe('AccountOptionCardComponent', () => {
  let component: AccountOptionCardComponent;
  let fixture: ComponentFixture<AccountOptionCardComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [RouterTestingModule],
      declarations: [AccountOptionCardComponent]
    }).compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(AccountOptionCardComponent);
    component = fixture.componentInstance;
    component.option = {
      title: 'Test Option',
      icon: 'fa-test-icon',
      link: '/test-link'
    };
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should render the option title', () => {
    const titleEl = fixture.debugElement.query(By.css('.card-title b')).nativeElement;
    expect(titleEl.textContent).toContain('Test Option');
  });

  it('should render the correct icon class', () => {
    const iconEl = fixture.debugElement.query(By.css('.card-icon i')).nativeElement;
    expect(iconEl.classList).toContain('fa-test-icon');
    expect(iconEl.classList).toContain('fa');
    expect(iconEl.classList).toContain('fa-5x');
  });

  it('should set routerLink correctly', () => {
    const linkEl = fixture.debugElement.query(By.css('.card-option-body'));
    expect(linkEl.attributes['ng-reflect-router-link']).toBe('/test-link');
  });
});