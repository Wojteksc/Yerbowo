import { Component, OnInit, ViewEncapsulation } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Cart } from 'src/app/cart/models/cart';
import { AlertifyService } from 'src/app/core/services/alertify.service';
import { CartService } from 'src/app/cart/services/cart.service';
import { DataService } from 'src/app/core/services/data.service';

@Component({
  selector: 'app-cart',
  templateUrl: './cart.component.html',
  styleUrls: ['./cart.component.css'],
  encapsulation: ViewEncapsulation.None,
})
export class CartComponent implements OnInit {
  cart: Cart;
  totalCartProducts: any;

  constructor(
    private activatedRoute: ActivatedRoute,
    private cartService: CartService,
    private alertify: AlertifyService,
    private dataService: DataService
  ) {}

  ngOnInit(): void {
    this.activatedRoute.data.subscribe(data => this.cart = data['cart']);
    this.dataService.cartTotal$.subscribe(total => this.totalCartProducts = total);
  }

  isAnyCartItem(): boolean {
    return this.cart?.items?.length > 0;
  }

  saveRange(productId: number, newValue: number): void {
    if (newValue < 1) return;
    this.updateCart(productId, newValue);
  }

  handleMinus(productId: number, newValue: number): void {
    this.updateCart(productId, newValue);
  }

  handlePlus(productId: number, newValue: number): void {
    if (this.isStockExceeded(productId, newValue)) return;
    this.updateCart(productId, newValue);
  }

  private updateCart(productId: number, newValue: number): void {
    this.cartService.update(productId, newValue).subscribe({
      next: response => this.updateCartState(response),
      error: error => this.alertify.error(error),
    });
  }

  private updateCartState(updatedCart: Cart): void {
    this.cart = updatedCart;
    this.dataService.changeTotalCartProducts(updatedCart.totalItems);
  }

  isStockExceeded(productId: number, newValue: number): boolean {
    const item = this.cart.items.find(x => x.product.id === productId);
    return item ? item.quantity < newValue : false;
  }

  deleteProduct(productId: number): void {
    this.cartService.remove(productId).subscribe({
      next: response => {
        this.cart = response;
        this.alertify.success('Produkt został usunięty z koszyka.');
        this.dataService.changeTotalCartProducts(response.totalItems);
      },
      error: () => this.alertify.error('Wystąpił błąd podczas usuwania produktu'),
    });
  }
}