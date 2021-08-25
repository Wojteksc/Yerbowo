import { Injectable } from '@angular/core';
import { CanDeactivate } from '@angular/router';
import { AccountEditComponent } from '../components/account/account-edit/account-edit.component';

@Injectable()
export class PreventUnsavedChanges implements CanDeactivate<AccountEditComponent> {
  canDeactivate(component: AccountEditComponent) {
    // if (component.accountForm.dirty) {
    //   return confirm('Opuścić strone? Wprowadzone zmiany mogą nie zostać zapisane.');
    // }
    return true;
  }
}
