import { Component, OnInit, ViewEncapsulation } from '@angular/core';
import { AuthService } from 'src/app/_services/auth.service';
import { Router } from '@angular/router';
import { AlertifyService } from 'src/app/_services/alertify.service';
import { AuthService as SocialAuthService } from 'angularx-social-login';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['../../styles/form-center.css', './login.component.css'],
  encapsulation: ViewEncapsulation.None,
})
export class LoginComponent implements OnInit {
  model: any = {};
  constructor(private authService: AuthService, private router: Router,
    private alertify: AlertifyService, private socialAuthService: SocialAuthService) { }

  ngOnInit() {
  }

  login() {
    this.authService.login(this.model).subscribe(next => {
      this.alertify.success('Pomyślnie zalogowano');
    }, error => {
      this.alertify.error(error);
    }, () => {
      this.router.navigate(['']);
    });
  }

  loginWithSocial(platform: string) {
    this.socialAuthService.signIn(platform).then(response => {
      const userData = Object.assign({}, response);
      this.authService.loginWithSocial(userData).subscribe(next => {
        this.alertify.success('Pomyślnie zalogowano');
      }, error => {
        this.alertify.error(error);
      }, () => {
        this.router.navigate(['']);
      });

    }, error => {
      //this.alertify.error(error);
    },);
  }

}
