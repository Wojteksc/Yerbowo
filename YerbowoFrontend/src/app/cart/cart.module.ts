import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { SocialLoginModule } from '@abacritt/angularx-social-login';

import { SharedModule } from 'src/app/shared/shared.module';
import { CartRoutingModule } from './cart-routing.module';

import { CartResolver } from './resolvers/cart.resolver';

import { CartService } from './services/cart.service';

import { CartComponent } from './components/cart/cart.component';

@NgModule({
  declarations: [
    CartComponent,
  ],
  imports: [
    CommonModule,
    FormsModule,
    ReactiveFormsModule,
    SocialLoginModule,
    CartRoutingModule,
    SharedModule,
  ],
  providers: [
    CartResolver,
    CartService,
  ]
})
export class CartModule {}