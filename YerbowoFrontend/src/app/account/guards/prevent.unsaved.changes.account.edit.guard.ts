import { Injectable } from '@angular/core';

import { AccountEditComponent } from '../components/account-edit/account-edit.component';

@Injectable()
export class PreventUnsavedChangesAccountEditGuard  {
  canDeactivate(component: AccountEditComponent) {
    if (component.accountForm.dirty) {
      return confirm('Opuścić strone? Wprowadzone zmiany mogą nie zostać zapisane.');
    }
    return true;
  }
}
