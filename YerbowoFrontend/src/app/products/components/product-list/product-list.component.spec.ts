import { ComponentFixture, TestBed } from '@angular/core/testing';
import { ProductListComponent } from './product-list.component';
import { ActivatedRoute } from '@angular/router';
import { ProductService } from 'src/app/products/services/product.service';
import { AlertifyService } from 'src/app/core/services/alertify.service';
import { of, throwError } from 'rxjs';
import { ProductCard } from 'src/app/products/models/productCard';
import { PaginatedResult } from 'src/app/shared/models/pagination';
import { Component, Input } from '@angular/core';

describe('ProductListComponent', () => {
  let component: ProductListComponent;
  let fixture: ComponentFixture<ProductListComponent>;

  const mockProducts: ProductCard[] = [
    { id: 1, name: 'Product A', categorySlug: 'cat', subcategorySlug: 'subcat', price: 10, oldPrice: 12, state: 0, image: 'img.jpg', slug: 'product-a', createdAt: new Date() }
  ];

  const paginatedMock: PaginatedResult<ProductCard[]> = {
    result: mockProducts,
    pagination: {
      currentPage: 1,
      itemsPerPage: 10,
      totalItems: 1,
      totalPages: 1
    }
  };

  const activatedRouteMock = {
    snapshot: {
      params: { category: 'herbata', subcategory: 'zielona' }
    },
    data: of({ products: paginatedMock })
  };

  const productServiceMock = {
    getProducts: jasmine.createSpy().and.returnValue(of(paginatedMock))
  };

  const alertifyMock = {
    error: jasmine.createSpy('error')
  };

  @Component({
    selector: 'app-product-card',
    template: ''
  })
  class MockProductCardComponent {
    @Input() product: any;
  }
  
  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [
        ProductListComponent,
        MockProductCardComponent
      ],
      providers: [
        { provide: ActivatedRoute, useValue: activatedRouteMock },
        { provide: ProductService, useValue: productServiceMock },
        { provide: AlertifyService, useValue: alertifyMock }
      ]
    }).compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(ProductListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create component and load initial products', () => {
    expect(component).toBeTruthy();
    expect(component.products.length).toBe(1);
    expect(component.pagination.totalItems).toBe(1);
    expect(component.category).toBe('herbata');
    expect(component.subcategory).toBe('zielona');
  });

  it('should return true if products exist', () => {
    expect(component.hasProducts).toBeTrue();
  });

  it('should return false if products array is empty', () => {
    component.products = [];
    expect(component.hasProducts).toBeFalse();
  });

  it('should load products on page change', () => {
    component.pageChanged({ page: 2 });
    expect(productServiceMock.getProducts).toHaveBeenCalledWith(2, 10, 'herbata', 'zielona');
  });

  it('should handle error when loadProducts fails', () => {
    productServiceMock.getProducts.and.returnValue(throwError(() => 'Błąd API'));
    component.loadProducts();
    expect(alertifyMock.error).toHaveBeenCalledWith('Błąd API');
  });
});