import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule, FormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';
import { PaginationModule } from 'ngx-bootstrap/pagination';

import { NotFoundComponent } from './components/errors/not-found/not-found.component';
import { FooterComponent } from './components/footer/footer.component';
import { NavComponent } from './components/nav/nav.component';
import { SearchComponent } from './components/search/search.component';
import { SliderComponent } from './components/slider/slider.component';
import { ProductTabsComponent } from './components/product-tabs/product-tabs.component';
import { ProductCardComponent } from './components/product-card/product-card.component';
import { MenuComponent } from './components/menu/menu.component';

@NgModule({
  declarations: [
    SliderComponent,
    ProductTabsComponent,
    ProductCardComponent,
    MenuComponent,
    NotFoundComponent,
    FooterComponent,
    NavComponent,
    SearchComponent,
  ],
  imports: [
    PaginationModule.forRoot(),
    CommonModule,
    RouterModule,
    ReactiveFormsModule,
    FormsModule
  ],
  exports: [
    CommonModule,
    RouterModule,
    ReactiveFormsModule,
    FormsModule,
    ProductCardComponent,
    ProductTabsComponent,
    SliderComponent,
    MenuComponent,
    NotFoundComponent,
    FooterComponent,
    NavComponent,
    SearchComponent,
  ]
})
export class SharedModule { }