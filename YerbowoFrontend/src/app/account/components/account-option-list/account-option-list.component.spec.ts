import { ComponentFixture, TestBed } from '@angular/core/testing';
import { AccountOptionListComponent } from './account-option-list.component';
import { Component, Input } from '@angular/core';
import { By } from '@angular/platform-browser';

@Component({
  selector: 'app-account-option-card',
  template: '<div class="mock-card">{{ option.title }}</div>'
})
class MockAccountOptionCardComponent {
  @Input() option: any;
}

describe('AccountOptionListComponent', () => {
  let component: AccountOptionListComponent;
  let fixture: ComponentFixture<AccountOptionListComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ AccountOptionListComponent, MockAccountOptionCardComponent ]
    }).compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(AccountOptionListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should have 5 options', () => {
    expect(component.options.length).toBe(5);
  });

  it('should render one card per option', () => {
    const cards = fixture.debugElement.queryAll(By.css('app-account-option-card'));
    expect(cards.length).toBe(component.options.length);

    expect(cards[0].nativeElement.textContent).toContain(component.options[0].title);
  });
});
