import { Component, EventEmitter, OnInit, Output, ViewEncapsulation } from '@angular/core';
import { ProductDetail } from 'src/app/_models/productDetail';
import { ProductService } from 'src/app/_services/product.service';
import { ActivatedRoute } from '@angular/router';
import { UntypedFormControl, UntypedFormGroup, Validators } from '@angular/forms';
import { CartService } from 'src/app/_services/cart.service';
import { AlertifyService } from 'src/app/_services/alertify.service';
import { DataService } from 'src/app/_services/data.service';

@Component({
  selector: 'app-product-detail',
  templateUrl: './product-detail.component.html',
  styleUrls: ['./product-detail.component.css'],
  encapsulation: ViewEncapsulation.None,
})
export class ProductDetailComponent implements OnInit {
  product: ProductDetail;
  productForm: UntypedFormGroup;
  @Output() onCountCart = new EventEmitter();
  totalCartProducts: any;

  constructor(private activatedRoute: ActivatedRoute,
              private cartService: CartService,
              private alertify: AlertifyService,
              private dataService: DataService) { }

  ngOnInit() {
    this.activatedRoute.data.subscribe(data => {
      this.product = data['product'];
    });

    window.scrollTo(0, 0);

    this.productForm = new UntypedFormGroup({
      id: new UntypedFormControl(this.product.id, Validators.required),
      quantity: new UntypedFormControl(1, Validators.required)
    });

    this.dataService.currentTotalMessage.subscribe(totalCartProducts => this.totalCartProducts = totalCartProducts)
  }

  onSubmit() {
    if(this.productForm.valid) {
      this.cartService.add(this.productForm.value.id, this.productForm.value.quantity).subscribe(response => {
        this.alertify.success('Dodano do koszyka');
        this.dataService.changeTotalCartProducts(response.totalItems);
      }, error => {
        this.alertify.error(error);
      });
    }
  }

}
