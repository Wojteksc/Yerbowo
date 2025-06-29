import { ComponentFixture, TestBed } from '@angular/core/testing';
import { NotFoundComponent } from './not-found.component';
import { RouterTestingModule } from '@angular/router/testing';
import { By } from '@angular/platform-browser';

describe('NotFoundComponent', () => {
  let component: NotFoundComponent;
  let fixture: ComponentFixture<NotFoundComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ NotFoundComponent ],
      imports: [ RouterTestingModule ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(NotFoundComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create the NotFoundComponent', () => {
    expect(component).toBeTruthy();
  });

  it('should render the 404 message', () => {
    const compiled = fixture.nativeElement as HTMLElement;
    expect(compiled.querySelector('h1')?.textContent).toContain('404 - Nie znaleziono strony');
    expect(compiled.querySelector('p')?.textContent).toContain('Strona, której szukasz, nie istnieje.');
  });

  it('should have a router link back to home', () => {
    const linkDebugEl = fixture.debugElement.query(By.css('a'));
    expect(linkDebugEl.attributes['routerLink']).toBe('/');
    expect(linkDebugEl.nativeElement.textContent).toContain('Wróć do strony głównej');
  });
});
