import { Component, OnInit, ViewEncapsulation } from '@angular/core';
import { ProductCard } from 'src/app/_models/productCard';
import { ProductService } from 'src/app/_services/product.service';
import { ActivatedRoute } from '@angular/router';
import { Pagination, PaginatedResult } from 'src/app/_models/pagination';
import { AlertifyService } from 'src/app/_services/alertify.service';

@Component({
  selector: 'app-product-list',
  templateUrl: './product-list.component.html',
  styleUrls: ['./product-list.component.css'],
  encapsulation: ViewEncapsulation.None,
})
export class ProductListComponent implements OnInit {
  products: ProductCard[];
  subcategory: string;
  category: string;
  pagination: Pagination;

  constructor(private activatedRoute: ActivatedRoute, private productService: ProductService,
    private alertify: AlertifyService) { }

  ngOnInit() {
    this.activatedRoute.data.subscribe(data => {
      this.subcategory = this.activatedRoute.snapshot.params['subcategory'];
      this.category = this.activatedRoute.snapshot.params['category'];
      this.products = data['products'].result;
      this.pagination = data['products'].pagination;
    });
  }

  isAnyProduct(): boolean {
    return this.products.length > 0;
  }

  pageChanged(event: any): void {
    this.pagination.currentPage = event.page;
    this.loadProducts();
  }

  loadProducts() {
    this.productService.getProducts(this.pagination.currentPage, this.pagination.itemsPerPage,
      this.category, this.subcategory)
      .subscribe((response: PaginatedResult<ProductCard[]>) => {
        this.products = response.result;
        this.pagination = response.pagination;
      }, error => {
        this.alertify.error(error);
      })
  }
}
