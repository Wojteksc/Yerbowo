import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

import { ProductDetailComponent } from './components/product-detail/product-detail.component';
import { ProductListComponent } from './components/product-list/product-list.component';
import { ProductListResolver } from './resolvers/product-list.resolver';
import { ProductDetailResolver } from './resolvers/product-detail.resolver';

const routes: Routes = [
  { path: ':category/:subcategory/:product', component: ProductDetailComponent, resolve: {product: ProductDetailResolver}  },
  { path: ':category/:subcategory', component: ProductListComponent, resolve: {products: ProductListResolver} },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class ProductsRoutingModule {}