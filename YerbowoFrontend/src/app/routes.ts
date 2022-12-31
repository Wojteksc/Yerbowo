import { Routes } from '@angular/router';
import { HomeComponent } from './components/home/home.component';
import { ProductListComponent } from './components/products/product-list/product-list.component';
import { ProductListResolver } from './_resolvers/product-list.resolver';
import { ProductDetailComponent } from './components/products/product-detail/product-detail.component';
import { ProductDetailResolver } from './_resolvers/product-detail.resolver';
import { LoginComponent } from './components/auth/login/login.component';
import { AuthGuard } from './_guards/auth.guard';
import { AccountEditComponent } from './components/account/account-edit/account-edit.component';
import { AccountEditResolver } from './_resolvers/account-edit.resolver';
import { PreventUnsavedChanges } from './_guards/prevent.unsaved.changes.guard';
import { OrderHistoryTableResolver } from './_resolvers/order-history-table.resolver';
import { OrderHistoryTableComponent } from './components/account/order-history-table/order-history-table.component';
import { OrderHistoryDetailComponent } from './components/account/order-history-detail/order-history-detail.component';
import { OrderHistoryDetailResolver } from './_resolvers/order-history-detail.resolver';
import { RegisterComponent } from './components/auth/register/register.component';
import { AddressListComponent } from './components/account/address-list/address-list.component';
import { AddressListResolver } from './_resolvers/address-list.resolver';
import { AddressAddComponent } from './components/account/address-add/address-add.component';
import { AddressEditComponent } from './components/account/address-edit/address-edit.component';
import { AddressEditResolver } from './_resolvers/address-edit.resolver';
import { AccountOptionListComponent } from './components/account/account-option-list/account-option-list.component';
import { CartComponent } from './components/cart/cart.component';
import { CartResolver } from './_resolvers/cart.resolver';
import { EmailVerificationComponent } from './components/auth/email-verification/email-verification.component';


export const appRoutes: Routes = [
    { path: '', component: HomeComponent },
    { path: 'login', component: LoginComponent},
    { path: 'register', component: RegisterComponent},
    { path: 'sklep/:category/:subcategory', component: ProductListComponent, resolve: {products: ProductListResolver} },
    { path: 'sklep/:category/:subcategory/:product', component: ProductDetailComponent, resolve: {product: ProductDetailResolver}},
    { path: 'koszyk', component: CartComponent, resolve: {cart: CartResolver}},
    { path: 'confirmEmail', component: EmailVerificationComponent },
    {
        path: '',
        runGuardsAndResolvers: 'always',
        canActivate: [AuthGuard],
        children: [
            { path: 'moje-konto', component: AccountOptionListComponent},
            { path: 'moje-konto/dane', component: AccountEditComponent, resolve: {user: AccountEditResolver},
                canDeactivate: [PreventUnsavedChanges] },
            { path: 'moje-konto/zamowienia', component: OrderHistoryTableComponent, resolve: {orderHistory: OrderHistoryTableResolver}},
            { path: 'moje-konto/zamowienia/:id', component: OrderHistoryDetailComponent,
                resolve: {orderDetail: OrderHistoryDetailResolver}},
            { path: 'moje-konto/adresy', component: AddressListComponent, resolve: {addresses: AddressListResolver}},
            { path: 'moje-konto/adresy/:id/edycja', component: AddressEditComponent, resolve: {address: AddressEditResolver}, canDeactivate: [PreventUnsavedChanges]},
            { path: 'moje-konto/adresy/dodaj', component: AddressAddComponent},
            { path: 'moje-konto/obserwowane', component: AccountEditComponent},
        ]
    },
    { path: '**', redirectTo: '', pathMatch: 'full' },
];