import { Component, OnInit, ViewEncapsulation } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { AlertifyService } from 'src/app/_services/alertify.service';
import { NewsletterService } from 'src/app/_services/newsletter.service';

@Component({
  selector: 'app-newsletter-email-subscribe',
  templateUrl: './newsletter-email-subscribe.component.html',
  styleUrls: ['./newsletter-email-subscribe.component.css'],
  encapsulation: ViewEncapsulation.None
})
export class NewsletterEmailSubscribeComponent implements OnInit {
  email: string;
  token: string;

  constructor(private newsletterService: NewsletterService,
    private alertify: AlertifyService,
    private activatedRoute: ActivatedRoute,
    private router: Router) { }

  ngOnInit() {
    this.activatedRoute.queryParams.subscribe(params => {
      this.email = params['email'];
      this.token = params['token'];
    });

    this.newsletterService.subscribe({email: this.email, token: this.token}).subscribe(response => {
      this.alertify.success("Pomyślnie powiodła się subskrypcja newslettera.", 0);
    }, error => {
      this.alertify.error(error);
    }, () => {
      this.router.navigate(['']);
    })
  }
}
