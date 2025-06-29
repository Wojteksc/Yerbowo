import { ComponentFixture, TestBed } from '@angular/core/testing';
import { ProductTabsComponent } from './product-tabs.component';
import { HomeService } from 'src/app/home/services/home.service';
import { of, throwError } from 'rxjs';
import { ProductCard } from 'src/app/products/models/productCard';
import { ProductState } from 'src/app/shared/enums/productState.enum';
import { Component, Input } from '@angular/core';

@Component({
  selector: 'app-product-card',
  template: '<div>Mock Product Card</div>'
})
class MockProductCardComponent {
  @Input() product!: ProductCard;
}

describe('ProductTabsComponent', () => {
  let component: ProductTabsComponent;
  let fixture: ComponentFixture<ProductTabsComponent>;
  let homeServiceMock: any;

  const fakeProducts: ProductCard[] = [
    {
      id: 1,
      name: 'Produkt 1',
      categorySlug: 'cat1',
      subcategorySlug: 'sub1',
      slug: 'produkt-1',
      price: 20,
      oldPrice: 25,
      state: ProductState.Bestseller,
      image: 'img1.jpg',
      createdAt: new Date()
    }
  ];

  beforeEach(async () => {
    homeServiceMock = {
      getProducts: jasmine.createSpy('getProducts').and.returnValue(of({
        bestsellers: fakeProducts,
        news: fakeProducts,
        recommended: fakeProducts,
        promotions: fakeProducts
      }))
    };

    await TestBed.configureTestingModule({
      declarations: [ProductTabsComponent, MockProductCardComponent],
      providers: [
        { provide: HomeService, useValue: homeServiceMock }
      ]
    }).compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(ProductTabsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create component', () => {
    expect(component).toBeTruthy();
  });

  it('should call homeService.getProducts on init and populate arrays', () => {
    expect(homeServiceMock.getProducts).toHaveBeenCalled();
    expect(component.bestsellers.length).toBe(1);
    expect(component.news.length).toBe(1);
    expect(component.recommended.length).toBe(1);
    expect(component.promotions.length).toBe(1);
  });

  it('should handle error on getProducts', () => {
    homeServiceMock.getProducts.and.returnValue(throwError(() => new Error('Error')));
    component.loadProducts();
    expect(component.bestsellers.length).toBe(0);
    expect(component.news.length).toBe(0);
    expect(component.recommended.length).toBe(0);
    expect(component.promotions.length).toBe(0);
  });
});
