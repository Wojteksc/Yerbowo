import { Component, OnInit, OnDestroy, ViewEncapsulation } from '@angular/core';
import { AuthService } from 'src/app/auth/services/auth.service';
import { Router } from '@angular/router';
import { AlertifyService } from 'src/app/core/services/alertify.service';
import { FacebookLoginProvider, GoogleLoginProvider, SocialAuthService, SocialUser } from '@abacritt/angularx-social-login';
import { filter, Subject, switchMap, takeUntil } from 'rxjs';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['../../../shared/styles/form-center.css', './login.component.css'],
  encapsulation: ViewEncapsulation.None,
})
export class LoginComponent implements OnInit, OnDestroy {
  model = { email: '', password: '' };
  private destroy$ = new Subject<void>();

  constructor(
    private authService: AuthService,
    private router: Router,
    private alertify: AlertifyService,
    private socialAuthService: SocialAuthService
  ) {}

  ngOnInit(): void {
    this.listenToGoogleLogin();
  }

  ngOnDestroy(): void {
    this.destroy$.next();
    this.destroy$.complete();
  }

  signIn(): void {
    this.authService.login(this.model)
      .pipe(takeUntil(this.destroy$))
      .subscribe({
        next: () => this.handleLoginSuccess(),
        error: (err) => this.handleLoginError(err)
      });
  }

  private listenToGoogleLogin(): void {
    this.socialAuthService.authState
      .pipe(
        filter((user: SocialUser | null) => !!user && user.provider === GoogleLoginProvider.PROVIDER_ID),
        switchMap(user => this.authService.loginWithSocial(user!)),
        takeUntil(this.destroy$)
      )
      .subscribe({
        next: () => this.handleLoginSuccess(),
        error: (err) => this.handleLoginError(err)
      });
  }

  signInWithGoogle(): void {
    this.socialAuthService.signIn(GoogleLoginProvider.PROVIDER_ID);
  }

  async signInWithFB(): Promise<void> {
    try {
      const response = await this.socialAuthService.signIn(FacebookLoginProvider.PROVIDER_ID);
      await this.authService.loginWithSocial(response).pipe(takeUntil(this.destroy$)).toPromise();
      this.handleLoginSuccess();
    } catch {
      this.handleLoginError('Błąd podczas logowania przez Facebook');
    }
  }

  private handleLoginSuccess(): void {
    this.alertify.success('Pomyślnie zalogowano');
    this.router.navigate(['']);
  }

  private handleLoginError(message: string): void {
    this.alertify.error(message);
  }
}
