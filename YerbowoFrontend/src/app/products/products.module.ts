import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { SocialLoginModule } from '@abacritt/angularx-social-login';
import { PaginationModule } from 'ngx-bootstrap/pagination';

import { SharedModule } from 'src/app/shared/shared.module';
import { ProductsRoutingModule } from './products-routing.module';

import { ProductDetailComponent } from './components/product-detail/product-detail.component';
import { ProductListComponent } from './components/product-list/product-list.component';

import { ProductListResolver } from './resolvers/product-list.resolver';
import { ProductDetailResolver } from './resolvers/product-detail.resolver';

import { ProductService } from './services/product.service';

@NgModule({
  declarations: [
    ProductDetailComponent,
    ProductListComponent,
],
imports: [
    CommonModule,
    FormsModule,
    ReactiveFormsModule,
    SocialLoginModule,
    ProductsRoutingModule,
    SharedModule,
    PaginationModule.forRoot()
  ],
  providers: [
    ProductService,
    ProductListResolver,
    ProductDetailResolver
  ]
})
export class ProductsModule {}