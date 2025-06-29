import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { AccountRoutingModule } from './account-routing.module';
import { ReactiveFormsModule } from '@angular/forms';

import { AccountEditComponent } from './components/account-edit/account-edit.component';
import { AccountTabsComponent } from './components/account-tabs/account-tabs.component';
import { AddressListComponent } from './components/address-list/address-list.component';
import { AddressEditComponent } from './components/address-edit/address-edit.component';
import { AddressCardComponent } from './components/address-card/address-card.component';
import { AccountOptionCardComponent } from './components/account-option-card/account-option-card.component';
import { AccountOptionListComponent } from './components/account-option-list/account-option-list.component';
import { AddressAddComponent } from './components/address-add/address-add.component';
import { OrderHistoryTableComponent } from './components/order-history-table/order-history-table.component';
import { OrderHistoryDetailComponent } from './components/order-history-detail/order-history-detail.component';

import { AccountEditResolver } from './resolvers/account-edit.resolver';
import { AddressEditResolver } from './resolvers/address-edit.resolver';
import { AddressListResolver } from './resolvers/address-list.resolver';
import { OrderHistoryDetailResolver } from './resolvers/order-history-detail.resolver';
import { OrderHistoryTableResolver } from './resolvers/order-history-table.resolver';

import { AddressService } from './services/address.service';
import { OrderService } from './services/order.service';
import { UserService } from './services/user.service';
import { PreventUnsavedChangesAccountEditGuard } from './guards/prevent.unsaved.changes.account.edit.guard';

@NgModule({
  declarations: [
    AccountEditComponent,
    AccountOptionCardComponent,
    AccountOptionListComponent,
    AccountTabsComponent,
    AddressAddComponent,
    AddressCardComponent,
    AddressEditComponent,
    AddressListComponent,
    OrderHistoryDetailComponent,
    OrderHistoryTableComponent,
  ],
  imports: [
    CommonModule,
    ReactiveFormsModule,
    AccountRoutingModule,
  ],
  providers: [
    AccountEditResolver,
    AddressEditResolver,
    AddressListResolver,
    OrderHistoryDetailResolver,
    OrderHistoryTableResolver,
    AddressService,
    OrderService,
    UserService,
    PreventUnsavedChangesAccountEditGuard
  ]
})
export class AccountModule {}
