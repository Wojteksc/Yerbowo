import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { NewsletterRoutingModule } from './newsletter-routing.module';

import { SocialLoginModule } from '@abacritt/angularx-social-login';
import { SharedModule } from 'src/app/shared/shared.module';

import { NewsletterEmailSubscribeComponent } from './components/newsletter-email-subscribe/newsletter-email-subscribe.component';
import { NewsletterEmailUnsubscribeComponent } from './components/newsletter-email-unsubscribe/newsletter-email-unsubscribe.component';

@NgModule({
  declarations: [
    NewsletterEmailSubscribeComponent,
    NewsletterEmailUnsubscribeComponent,
],
imports: [
    CommonModule,
    FormsModule,
    ReactiveFormsModule,
    SocialLoginModule,
    NewsletterRoutingModule,
    SharedModule,
  ]
})
export class NewsletterModule {}