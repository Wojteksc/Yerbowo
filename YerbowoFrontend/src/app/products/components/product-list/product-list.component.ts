import { Component, OnInit, ViewEncapsulation } from '@angular/core';
import { ProductCard } from 'src/app/products/models/productCard';
import { ProductService } from 'src/app/products/services/product.service';
import { ActivatedRoute } from '@angular/router';
import { Pagination, PaginatedResult } from 'src/app/shared/models/pagination';
import { AlertifyService } from 'src/app/core/services/alertify.service';

@Component({
  selector: 'app-product-list',
  templateUrl: './product-list.component.html',
  styleUrls: ['./product-list.component.css'],
  encapsulation: ViewEncapsulation.None,
})
export class ProductListComponent implements OnInit {
  products: ProductCard[] = [];
  subcategory = '';
  category = '';
  pagination: Pagination = { currentPage: 1, itemsPerPage: 10, totalItems: 0, totalPages: 0 };

  constructor(
    private route: ActivatedRoute,
    private productService: ProductService,
    private alertify: AlertifyService
  ) {}

  ngOnInit(): void {
    const routeData = this.route.snapshot.params;
    this.category = routeData['category'];
    this.subcategory = routeData['subcategory'];

    this.route.data.subscribe(data => {
      const paginatedResult: PaginatedResult<ProductCard[]> = data['products'];
      this.products = paginatedResult?.result || [];
      this.pagination = paginatedResult?.pagination || this.pagination;
    });
  }

  get hasProducts(): boolean {
    return this.products.length > 0;
  }

  pageChanged(event: any): void {
    this.pagination.currentPage = event.page;
    this.loadProducts();
  }

  loadProducts(): void {
    this.productService
      .getProducts(this.pagination.currentPage, this.pagination.itemsPerPage, this.category, this.subcategory)
      .subscribe({
        next: (response: PaginatedResult<ProductCard[]>) => {
          this.products = response.result;
          this.pagination = response.pagination;
        },
        error: error => this.alertify.error(error)
      });
  }
}