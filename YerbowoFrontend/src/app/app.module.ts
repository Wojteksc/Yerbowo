import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { HttpClientModule } from '@angular/common/http';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';
import { JwtModule } from '@auth0/angular-jwt';
import { PaginationModule } from 'ngx-bootstrap/pagination';
import { SocialLoginModule, SocialAuthServiceConfig } from "angularx-social-login";
import { GoogleLoginProvider, FacebookLoginProvider } from 'angularx-social-login';

import { AppComponent } from './app.component';
import { HomeComponent } from './components/home/home.component';
import { NavComponent } from './components/nav/nav.component';
import { SearchComponent } from './components/search/search.component';
import { MenuComponent } from './components/menu/menu.component';
import { FooterComponent } from './components/footer/footer.component';
import { SliderComponent } from './components/slider/slider.component';
import { ProductTabsComponent } from './components/products/product-tabs/product-tabs.component';
import { ProductCardComponent } from './components/products/product-card/product-card.component';
import { appRoutes } from './routes';
import { ProductListComponent } from './components/products/product-list/product-list.component';
import { ProductListResolver } from './_resolvers/product-list.resolver';
import { ProductDetailComponent } from './components/products/product-detail/product-detail.component';
import { ProductDetailResolver } from './_resolvers/product-detail.resolver';
import { LoginComponent } from './components/auth/login/login.component';
import { AuthGuard } from './_guards/auth.guard';
import { AccountTabsComponent } from './components/account/account-tabs/account-tabs.component';
import { AccountEditComponent } from './components/account/account-edit/account-edit.component';
import { AccountEditResolver } from './_resolvers/account-edit.resolver';
import { AuthService } from './_services/auth.service';
import { AlertifyService } from './_services/alertify.service';
import { PreventUnsavedChanges } from './_guards/prevent.unsaved.changes.guard';
import { OrderService } from './_services/order.service';
import { OrderHistoryTableComponent } from './components/account/order-history-table/order-history-table.component';
import { OrderHistoryTableResolver } from './_resolvers/order-history-table.resolver';
import { OrderHistoryDetailComponent } from './components/account/order-history-detail/order-history-detail.component';
import { OrderHistoryDetailResolver } from './_resolvers/order-history-detail.resolver';
import { ErrorInterceptorProvider } from './_services/error.interceptor';
import { RegisterComponent } from './components/auth/register/register.component';
import { AddressListComponent } from './components/account/address-list/address-list.component';
import { AddressListResolver } from './_resolvers/address-list.resolver';
import { AddressCardComponent } from './components/account/address-card/address-card.component';
import { AddressAddComponent } from './components/account/address-add/address-add.component';
import { AddressEditComponent } from './components/account/address-edit/address-edit.component';
import { AddressEditResolver } from './_resolvers/address-edit.resolver';
import { AccountOptionCardComponent } from './components/account/account-option-card/account-option-card.component';
import { AccountOptionListComponent } from './components/account/account-option-list/account-option-list.component';
import { CartComponent } from './components/cart/cart.component';
import { CartResolver } from './_resolvers/cart.resolver';
import { EmailVerificationComponent } from './components/auth/email-verification/email-verification.component';
import { CommonModule } from '@angular/common';

export function tokkenGetter() {
  return localStorage.getItem('token');
}

let socialAuthServiceConfig = {
  provide: "SocialAuthServiceConfig",
  useValue: {
    autoLogin: true,
    providers: [
      {
        id: GoogleLoginProvider.PROVIDER_ID,
        provider: new GoogleLoginProvider("985347060101-sv911mn4704s2h81ldrhdlctnvjkqivj.apps.googleusercontent.com")
      },
      {
        id: FacebookLoginProvider.PROVIDER_ID,
        provider: new FacebookLoginProvider("721481622017748")
      }
    ]
  } as SocialAuthServiceConfig
}

export function provideConfig() {
  return socialAuthServiceConfig;
}

@NgModule({
  declarations: [
    AppComponent,
    HomeComponent,
    NavComponent,
    SearchComponent,
    MenuComponent,
    FooterComponent,
    SliderComponent,
    ProductTabsComponent,
    ProductCardComponent,
    ProductListComponent,
    ProductDetailComponent,
    LoginComponent,
    RegisterComponent,
    AccountEditComponent,
    AccountTabsComponent,
    OrderHistoryTableComponent,
    OrderHistoryDetailComponent,
    AddressCardComponent,
    AddressListComponent,
    AddressAddComponent,
    AddressEditComponent,
    AccountOptionCardComponent,
    AccountOptionListComponent,
    CartComponent,
    EmailVerificationComponent,
  ],
  imports: [
    RouterModule.forRoot(appRoutes, { relativeLinkResolution: 'legacy' }),
    BrowserModule,
    CommonModule,
    HttpClientModule,
    FormsModule,
    ReactiveFormsModule,
    PaginationModule.forRoot(),
    JwtModule.forRoot({
      config: {
        tokenGetter: tokkenGetter,
        whitelistedDomains: ['localhost:5000'],
        blacklistedRoutes: ['localhost:5000/api/auth']
      }
    }),
    SocialLoginModule,
  ],
  providers: [
    provideConfig(),
    ErrorInterceptorProvider,
    ProductListResolver,
    ProductDetailResolver,
    AuthGuard,
    AccountEditResolver,
    AuthService,
    AlertifyService,
    PreventUnsavedChanges,
    OrderService,
    OrderHistoryTableResolver,
    OrderHistoryDetailResolver,
    AddressListResolver,
    AddressEditResolver,
    CartResolver
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
