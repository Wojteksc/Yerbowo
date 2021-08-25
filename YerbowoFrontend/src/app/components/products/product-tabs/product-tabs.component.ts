import { Component, OnInit, ViewEncapsulation, Input } from '@angular/core';
import { HomeProducts } from 'src/app/_models/homeProducts';
import { ProductCard } from 'src/app/_models/productCard';
import { HomeService } from 'src/app/_services/home.service';

@Component({
  selector: 'app-product-tabs',
  templateUrl: './product-tabs.component.html',
  styleUrls: ['./product-tabs.component.css'],
  encapsulation: ViewEncapsulation.None,
})
export class ProductTabsComponent implements OnInit {
  homeProducts: HomeProducts;
  bestsellers: ProductCard[];
  news: ProductCard[];
  recommended: ProductCard[];
  promotions: ProductCard[];

  constructor(private homeService: HomeService) { }

  ngOnInit() {
    this.loadProducts();
  }

  loadProducts() {
    this.homeService.getProducts().subscribe((homeProducts: HomeProducts) => {
      this.bestsellers = homeProducts.bestsellers;
      this.news = homeProducts.news;
      this.recommended = homeProducts.recommended;
      this.promotions = homeProducts.promotions;
    }, error => {
    });
  }

}
