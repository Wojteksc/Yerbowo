import { Component, OnInit, ViewEncapsulation, Input, Output, EventEmitter } from '@angular/core';
import { ProductCard } from 'src/app/_models/productCard';
import { ProductState } from 'src/app/_enums/productState.enum';
import { FormBuilder, FormGroup } from '@angular/forms';
import { CartService } from 'src/app/_services/cart.service';
import { AlertifyService } from 'src/app/_services/alertify.service';
import { DataService } from 'src/app/_services/data.service';

@Component({
  selector: 'app-product-card',
  templateUrl: './product-card.component.html',
  styleUrls: ['./product-card.component.css'],
  encapsulation: ViewEncapsulation.None,
})
export class ProductCardComponent implements OnInit {
  @Input() product: ProductCard;
  productState: any = ProductState;
  cartForm: FormGroup;
  totalCartProducts: any;

  constructor(private fb: FormBuilder, 
    private cartService: CartService,
    private alertify: AlertifyService,
    private dataService: DataService) { }

  ngOnInit() {
    this.cartForm = this.fb.group({
      id: [this.product.id]
    })
    this.dataService.currentTotalMessage.subscribe(totalCartProducts => this.totalCartProducts = totalCartProducts)
  }

  add() {
    if(this.cartForm.valid) {    
      this.cartService.add(this.cartForm.value.id).subscribe(response => {
        this.alertify.success('Produkt został dodany do koszyka.');
        this.dataService.changeTotalCartProducts(response.totalItems);
      }, error => {
        this.alertify.error(error);
      });
    }
  }

}
