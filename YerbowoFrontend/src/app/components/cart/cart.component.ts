import { Component, EventEmitter, OnInit, Output, ViewEncapsulation } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Cart } from 'src/app/_models/cart';
import { AlertifyService } from 'src/app/_services/alertify.service';
import { CartService } from 'src/app/_services/cart.service';
import { DataService } from 'src/app/_services/data.service';

@Component({
  selector: 'app-cart',
  templateUrl: './cart.component.html',
  styleUrls: ['./cart.component.css'],
  encapsulation: ViewEncapsulation.None,
})
export class CartComponent implements OnInit {
  cart: Cart;
  value = 0;
  totalCartProducts: any;

  constructor(private activatedRoute: ActivatedRoute,
              private cartService: CartService,
              private alertify: AlertifyService,
              private dataService: DataService) { }

  ngOnInit() {
    this.activatedRoute.data.subscribe(data => {
      this.cart = data['cart'];
    })

    this.dataService.currentTotalMessage.subscribe(totalCartProducts => this.totalCartProducts = totalCartProducts)
  }

  isAnyCartItem(): boolean {
    return this.cart.items.length > 0;
  }

  saveRange(productId, newValue) {
    if(newValue < 1) {
      return;
    }
    this.updateCart(productId, newValue);
  }

  handleMinus(productId, newValue) {
    this.updateCart(productId, newValue);
  }

  handlePlus(productId, newValue) {
    if(this.isStockExceeded(productId, newValue)) {
      return;
    }
    this.updateCart(productId, newValue);
  }

  updateCart(productId, newValue) {
    this.cartService.update(productId, newValue).subscribe(response => {
      this.cart = response;
      this.dataService.changeTotalCartProducts(response.totalItems);
    }, error => {
      this.alertify.error(error);
    });
  }

  isStockExceeded(productId, newValue) {
    return this.cart.items.find(x => x.product.id == productId).quantity < newValue;
  }

  deleteProduct(productId) {
    this.cartService.remove(productId).subscribe(response => {
      this.cart = response;
      this.alertify.success("Produkt został usunięty z koszyka.")
      this.dataService.changeTotalCartProducts(response.totalItems);
    }, error => {
      this.alertify.error("Wystąpił błąd podczas usuwania produktu")
    })
  }
}
