import { Component, ViewEncapsulation, OnInit } from '@angular/core';
import { NavigationStart, Router } from '@angular/router';
import { JwtHelperService } from '@auth0/angular-jwt';
import { Subscription } from 'rxjs';
import { AuthService } from './_services/auth.service';
import { CartService } from './_services/cart.service';
import { DataService } from './_services/data.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css'],
  encapsulation: ViewEncapsulation.None
})
export class AppComponent implements OnInit {
  jwtHelper = new JwtHelperService();
  subscription: Subscription;
  totalCartProducts: any;

  constructor(private authService: AuthService, 
              private router: Router,
              private dataService: DataService,
              private cartService: CartService) {

    this.subscription = router.events.subscribe((event) => {
      if (event instanceof NavigationStart) {
         if(!router.navigated) {
            this.cartService.getTotalCartProducts().subscribe(totalCartProducts => {
              this.dataService.changeTotalCartProducts(totalCartProducts);
            });
         }
      }
    });
  }

  ngOnInit() {
    this.dataService.currentTotalMessage.subscribe(totalCartProducts => this.totalCartProducts = totalCartProducts);

    const token = localStorage.getItem('token');
    const photoUrl = localStorage.getItem('photoUrl');
    if (token) {
      this.authService.decodedToken = this.jwtHelper.decodeToken(token);
    }
    if (photoUrl) {
      this.authService.photoUrl = photoUrl;
    }
  }
}
