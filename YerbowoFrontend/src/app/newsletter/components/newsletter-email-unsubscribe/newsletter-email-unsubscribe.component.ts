import { Component, OnInit, ViewEncapsulation } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { AlertifyService } from 'src/app/core/services/alertify.service';
import { NewsletterService } from 'src/app/newsletter/services/newsletter.service';

@Component({
  selector: 'app-newsletter-email-unsubscribe',
  templateUrl: './newsletter-email-unsubscribe.component.html',
  styleUrls: ['./newsletter-email-unsubscribe.component.css'],
  encapsulation: ViewEncapsulation.None
})
export class NewsletterEmailUnsubscribeComponent implements OnInit {
  email?: string;
  token?: string;

  constructor(
    private newsletterService: NewsletterService,
    private alertify: AlertifyService,
    private activatedRoute: ActivatedRoute,
    private router: Router
  ) {}

  ngOnInit(): void {
    this.activatedRoute.queryParams.subscribe(params => {
      this.email = params['email'];
      this.token = params['token'];

      if (this.email && this.token) {
        this.unsubscribeFromNewsletter();
      } else {
        this.alertify.error('Brak wymaganych parametrów do wypisania z newslettera.');
        this.router.navigate(['/']);
      }
    });
  }

  private unsubscribeFromNewsletter(): void {
    this.newsletterService.unsubscribe({ email: this.email, token: this.token }).subscribe({
      next: () => {
        this.alertify.success('Pomyślnie usunięto adres e-mail z listy subskrybentów.', 0);
        this.router.navigate(['/']);
      },
      error: (error) => {
        this.alertify.error(error);
        this.router.navigate(['/']);
      }
    });
  }
}
