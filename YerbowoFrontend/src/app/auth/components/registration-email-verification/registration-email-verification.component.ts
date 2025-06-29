import { Component, OnInit, ViewEncapsulation } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { AlertifyService } from 'src/app/core/services/alertify.service';
import { AuthService } from '../../services/auth.service';

@Component({
  selector: 'app-registration-email-verification',
  templateUrl: './registration-email-verification.component.html',
  styleUrls: [
    '../../../shared/styles/form-center.css',
    './registration-email-verification.component.css'
  ],
  encapsulation: ViewEncapsulation.None
})
export class RegistrationEmailVerificationComponent implements OnInit {
  private email!: string;
  private token!: string;

  constructor(
    private authService: AuthService,
    private alertify: AlertifyService,
    private activatedRoute: ActivatedRoute,
    private router: Router
  ) {}

  ngOnInit(): void {
    this.activatedRoute.queryParams.subscribe(params => {
      this.email = params['email'];
      this.token = params['token'];

      if (this.email && this.token) {
        this.verifyEmail();
      } else {
        this.alertify.error('Nieprawidłowy link aktywacyjny.');
        this.router.navigate(['/']);
      }
    });
  }

  private verifyEmail(): void {
    this.authService.confirmEmail({ email: this.email, token: this.token }).subscribe({
      next: () => {
        this.alertify.success('Adres e-mail został potwierdzony.', 0);
        this.router.navigate(['/']);
      },
      error: err => {
        this.alertify.error(err);
        this.router.navigate(['/']);
      }
    });
  }
}