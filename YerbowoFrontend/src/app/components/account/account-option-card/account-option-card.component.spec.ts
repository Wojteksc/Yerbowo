/* tslint:disable:no-unused-variable */
import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { By } from '@angular/platform-browser';
import { DebugElement } from '@angular/core';

import { AccountOptionCardComponent } from './account-option-card.component';

describe('AccountCardComponent', () => {
  let component: AccountOptionCardComponent;
  let fixture: ComponentFixture<AccountOptionCardComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ AccountOptionCardComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(AccountOptionCardComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
