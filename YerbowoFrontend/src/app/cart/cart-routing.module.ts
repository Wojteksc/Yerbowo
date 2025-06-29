import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { CartComponent } from './components/cart/cart.component';
import { CartResolver } from './resolvers/cart.resolver';

const routes: Routes = [
  { path: '', component: CartComponent, resolve: {cart: CartResolver}, },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class CartRoutingModule {}