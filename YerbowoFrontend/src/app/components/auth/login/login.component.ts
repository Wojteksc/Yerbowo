import { Component, OnInit, ViewEncapsulation } from '@angular/core';
import { AuthService } from 'src/app/_services/auth.service';
import { Router } from '@angular/router';
import { AlertifyService } from 'src/app/_services/alertify.service';
import { FacebookLoginProvider, SocialAuthService } from '@abacritt/angularx-social-login';
import { filter, Subscription, switchMap } from 'rxjs';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['../../../styles/form-center.css', './login.component.css'],
  encapsulation: ViewEncapsulation.None,
})
export class LoginComponent implements OnInit {
  model: any = {};
  user: any;
  subs: Subscription = new Subscription();
  
  constructor(private authService: AuthService, 
              private router: Router,
              private alertify: AlertifyService, 
              private socialAuthService: SocialAuthService) { }

  ngOnInit() {
    this.signInWithGoogle();
  }

  ngOnDestroy(): void {
    this.subs.unsubscribe();
  }

  signIn() {
    this.authService.login(this.model).subscribe({
      next: () => this.alertify.success('Pomyślnie zalogowano'),
      error: (e) => this.alertify.error(e),
      complete: () => this.router.navigate([''])
    });
  }

  signInWithGoogle() {
    let socialSub = this.socialAuthService.authState
    .pipe(
      filter((user) => user != null && user.provider == 'GOOGLE'),
      switchMap((user) =>
        this.authService.loginWithSocial(user)
      )
    )
    .subscribe(() => {
        this.alertify.success('Pomyślnie zalogowano');
        this.router.navigate(['/']);
    });
    this.subs.add(socialSub);
  }

  signInWithFB() {
    this.socialAuthService.signIn(FacebookLoginProvider.PROVIDER_ID).then(response => {
      const userData = Object.assign({}, response);
      this.authService.loginWithSocial(userData).subscribe({
        next: () => this.alertify.success('Pomyślnie zalogowano'),
        error: (e) => this.alertify.error(e),
        complete: () => this.router.navigate([''])
      })
    });
  }
}