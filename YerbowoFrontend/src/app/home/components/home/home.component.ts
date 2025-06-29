import { Component, OnInit, OnDestroy, ViewEncapsulation } from '@angular/core';
import { NavigationStart, Router } from '@angular/router';
import { JwtHelperService } from '@auth0/angular-jwt';
import { filter, switchMap, takeUntil, Subject } from 'rxjs';
import { AuthService } from 'src/app/auth/services/auth.service';
import { CartService } from 'src/app/cart/services/cart.service';
import { DataService } from 'src/app/core/services/data.service';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css'],
  encapsulation: ViewEncapsulation.None,
})
export class HomeComponent implements OnInit, OnDestroy {
  totalCartProducts: any;
  private destroy$ = new Subject<void>();
  private readonly jwtHelper = new JwtHelperService();

  constructor(
    private authService: AuthService,
    private router: Router,
    private dataService: DataService,
    private cartService: CartService
  ) {}

  ngOnInit(): void {
    this.authService.restoreSession();
    this.handleNavigationCartSync();
    this.subscribeToCartTotal();
  }

  private handleNavigationCartSync(): void {
    this.router.events
      .pipe(
        filter(event => event instanceof NavigationStart),
        switchMap(() => this.cartService.getTotalCartProducts()),
        takeUntil(this.destroy$)
      )
      .subscribe(total => {
        this.dataService.changeTotalCartProducts(total);
      });
  }

  private subscribeToCartTotal(): void {
    this.dataService.cartTotal$
      .pipe(takeUntil(this.destroy$))
      .subscribe(total => {
        this.totalCartProducts = total;
      });
  }

  ngOnDestroy(): void {
    this.destroy$.next();
    this.destroy$.complete();
  }
}
