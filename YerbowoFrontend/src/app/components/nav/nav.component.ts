import { Component, OnInit, ViewEncapsulation } from '@angular/core';
import { AuthService } from 'src/app/_services/auth.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-nav',
  templateUrl: './nav.component.html',
  styleUrls: ['./nav.component.css'],
  encapsulation: ViewEncapsulation.None,
})
export class NavComponent implements OnInit {

  constructor(public authService: AuthService, private router: Router) { }

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
    this.router.navigate(['']);
  }
}
