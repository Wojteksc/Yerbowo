import { Component, EventEmitter, OnInit, Output, ViewEncapsulation } from '@angular/core';
import { ProductDetail } from 'src/app/products/models/productDetail';
import { ActivatedRoute } from '@angular/router';
import { UntypedFormControl, UntypedFormGroup, Validators } from '@angular/forms';
import { CartService } from 'src/app/cart/services/cart.service';
import { AlertifyService } from 'src/app/core/services/alertify.service';
import { DataService } from 'src/app/core/services/data.service';

@Component({
  selector: 'app-product-detail',
  templateUrl: './product-detail.component.html',
  styleUrls: ['./product-detail.component.css'],
  encapsulation: ViewEncapsulation.None,
})
export class ProductDetailComponent implements OnInit {
  product: ProductDetail;
  productForm: UntypedFormGroup;
  @Output() onCountCart = new EventEmitter<any>();
  totalCartProducts: any;

  constructor(
    private activatedRoute: ActivatedRoute,
    private cartService: CartService,
    private alertify: AlertifyService,
    private dataService: DataService
  ) { }

  ngOnInit(): void {
    this.activatedRoute.data.subscribe(data => {
      this.product = data['product'];
      this.initForm();
    });

    window.scrollTo(0, 0);

    this.dataService.cartTotal$.subscribe(total => {
      this.totalCartProducts = total;
      this.onCountCart.emit(total);
    });
  }

  private initForm(): void {
    this.productForm = new UntypedFormGroup({
      id: new UntypedFormControl(this.product.id, Validators.required),
      quantity: new UntypedFormControl(1, [
        Validators.required,
        Validators.min(1),
        Validators.max(this.product.stock)
      ])
    });
  }

  onSubmit(): void {
    if (this.productForm.valid) {
      const { id, quantity } = this.productForm.value;
      this.cartService.add(id, quantity).subscribe({
        next: (response) => {
          this.alertify.success('Dodano do koszyka');
          this.dataService.changeTotalCartProducts(response.totalItems);
        },
        error: (error) => {
          this.alertify.error(error);
        }
      });
    }
  }
}
