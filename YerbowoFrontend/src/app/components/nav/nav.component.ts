import { Component, OnInit, ViewEncapsulation } from '@angular/core';
import { AuthService } from 'src/app/_services/auth.service';
import { Router } from '@angular/router';
import { SocialAuthService } from "@abacritt/angularx-social-login";

@Component({
  selector: 'app-nav',
  templateUrl: './nav.component.html',
  styleUrls: ['./nav.component.css'],
  encapsulation: ViewEncapsulation.None,
})
export class NavComponent implements OnInit {

  constructor(public authService: AuthService, 
    private router: Router,
    private socialAuthService: SocialAuthService) { }

  ngOnInit() {
  }

  loggedIn() {
    return this.authService.loggedIn();
  }

  logout() {
    localStorage.removeItem('token');
    localStorage.removeItem('photoUrl');
    this.authService.decodedToken = null;
    this.authService.photoUrl = null;
    this.socialAuthService.signOut();
    this.router.navigate(['/']);
  }
}