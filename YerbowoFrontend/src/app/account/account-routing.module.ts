import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

import { PreventUnsavedChangesAccountEditGuard } from 'src/app/account/guards/prevent.unsaved.changes.account.edit.guard';

import { AccountEditResolver } from 'src/app/account/resolvers/account-edit.resolver';
import { OrderHistoryTableResolver } from 'src/app/account/resolvers/order-history-table.resolver';
import { OrderHistoryDetailResolver } from 'src/app/account/resolvers/order-history-detail.resolver';
import { AddressListResolver } from 'src/app/account/resolvers/address-list.resolver';
import { AddressEditResolver } from 'src/app/account/resolvers/address-edit.resolver';

import { AccountOptionListComponent } from './components/account-option-list/account-option-list.component';
import { AccountEditComponent } from './components/account-edit/account-edit.component';
import { OrderHistoryTableComponent } from './components/order-history-table/order-history-table.component';
import { OrderHistoryDetailComponent } from './components/order-history-detail/order-history-detail.component';
import { AddressListComponent } from './components/address-list/address-list.component';
import { AddressEditComponent } from './components/address-edit/address-edit.component';
import { AddressAddComponent } from './components/address-add/address-add.component';

const routes: Routes = [
    { 
        path: '', 
        component: AccountOptionListComponent
    },
    { 
        path: 'dane', 
        component: AccountEditComponent, 
        resolve: {user: AccountEditResolver},
        canDeactivate: [PreventUnsavedChangesAccountEditGuard] 
    },
    { 
        path: 'zamowienia', 
        component: OrderHistoryTableComponent, 
        resolve: {orderHistory: OrderHistoryTableResolver}
    },
    { 
        path: 'zamowienia/:id', 
        component: OrderHistoryDetailComponent,
        resolve: {orderDetail: OrderHistoryDetailResolver}
    },
    { 
        path: 'adresy', 
        component: AddressListComponent, 
        resolve: {addresses: AddressListResolver}
    },
    { 
        path: 'adresy/:id/edycja', 
        component: AddressEditComponent, 
        resolve: {address: AddressEditResolver}, 
        canDeactivate: [PreventUnsavedChangesAccountEditGuard]
    },
    { 
        path: 'adresy/dodaj', 
        component: AddressAddComponent
    },
    { 
        path: 'obserwowane', 
        component: AccountEditComponent
    },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class AccountRoutingModule {}
