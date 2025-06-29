import { Component, OnInit, ViewEncapsulation } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { AlertifyService } from 'src/app/core/services/alertify.service';
import { NewsletterService } from 'src/app/newsletter/services/newsletter.service';

@Component({
  selector: 'app-newsletter-email-subscribe',
  templateUrl: './newsletter-email-subscribe.component.html',
  styleUrls: ['./newsletter-email-subscribe.component.css'],
  encapsulation: ViewEncapsulation.None,
})
export class NewsletterEmailSubscribeComponent implements OnInit {
  email?: string;
  token?: string;

  constructor(
    private newsletterService: NewsletterService,
    private alertify: AlertifyService,
    private activatedRoute: ActivatedRoute,
    private router: Router
  ) {}

  ngOnInit(): void {
    this.activatedRoute.queryParams.subscribe((params) => {
      this.email = params['email'];
      this.token = params['token'];

      if (this.email && this.token) {
        this.subscribeToNewsletter();
      } else {
        this.alertify.error('Brak wymaganych parametrów do subskrypcji.');
        this.router.navigate(['/']);
      }
    });
  }

  private subscribeToNewsletter(): void {
    this.newsletterService
      .subscribe({ email: this.email, token: this.token })
      .subscribe({
        next: () => {
          this.alertify.success('Dziękujemy za zapisanie się do newslettera!', 0);
          this.router.navigate(['/']);
        },
        error: (error) => {
          this.alertify.error(error);
          this.router.navigate(['/']);
        },
      });
  }
}
