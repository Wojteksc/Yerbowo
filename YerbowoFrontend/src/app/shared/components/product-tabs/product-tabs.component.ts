import { Component, OnInit, ViewEncapsulation } from '@angular/core';
import { HomeProducts } from 'src/app/shared/models/homeProducts';
import { ProductCard } from 'src/app/products/models/productCard';
import { HomeService } from 'src/app/home/services/home.service';

@Component({
  selector: 'app-product-tabs',
  templateUrl: './product-tabs.component.html',
  styleUrls: ['./product-tabs.component.css'],
  encapsulation: ViewEncapsulation.None,
})
export class ProductTabsComponent implements OnInit {
  bestsellers: ProductCard[] = [];
  news: ProductCard[] = [];
  recommended: ProductCard[] = [];
  promotions: ProductCard[] = [];

  constructor(private homeService: HomeService) { }

  ngOnInit() {
    this.loadProducts();
  }

  loadProducts() {
    this.homeService.getProducts().subscribe({
      next: (homeProducts: HomeProducts) => {
        this.bestsellers = homeProducts.bestsellers || [];
        this.news = homeProducts.news || [];
        this.recommended = homeProducts.recommended || [];
        this.promotions = homeProducts.promotions || [];
      },
      error: err => {
        this.bestsellers = [];
        this.news = [];
        this.recommended = [];
        this.promotions = [];
      }
    });
  }
}
