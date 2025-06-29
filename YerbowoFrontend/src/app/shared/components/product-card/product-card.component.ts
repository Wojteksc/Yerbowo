import { Component, OnInit, OnDestroy, ViewEncapsulation, Input } from '@angular/core';
import { ProductCard } from 'src/app/products/models/productCard';
import { ProductState } from 'src/app/shared/enums/productState.enum';
import { UntypedFormBuilder, UntypedFormGroup, Validators } from '@angular/forms';
import { CartService } from 'src/app/cart/services/cart.service';
import { AlertifyService } from 'src/app/core/services/alertify.service';
import { DataService } from 'src/app/core/services/data.service';
import { Subscription } from 'rxjs';

@Component({
  selector: 'app-product-card',
  templateUrl: './product-card.component.html',
  styleUrls: ['./product-card.component.css'],
  encapsulation: ViewEncapsulation.None,
})
export class ProductCardComponent implements OnInit, OnDestroy {
  @Input() product!: ProductCard;
  productState = ProductState;
  cartForm!: UntypedFormGroup;
  totalCartProducts: any;

  private subscription = new Subscription();

  constructor(
    private fb: UntypedFormBuilder,
    private cartService: CartService,
    private alertify: AlertifyService,
    private dataService: DataService
  ) {}

  ngOnInit(): void {
    this.cartForm = this.fb.group({
      id: [this.product.id, Validators.required]
    });

    const sub = this.dataService.cartTotal$.subscribe(total => this.totalCartProducts = total);
    this.subscription.add(sub);
  }

  ngOnDestroy(): void {
    this.subscription.unsubscribe();
  }

  add(): void {
    if (this.cartForm.valid) {
      const productId = this.cartForm.value.id;
      this.cartService.add(productId).subscribe({
        next: response => {
          this.alertify.success('Produkt został dodany do koszyka.');
          this.dataService.changeTotalCartProducts(response.totalItems);
        },
        error: err => {
          this.alertify.error(err);
        }
      });
    }
  }
}