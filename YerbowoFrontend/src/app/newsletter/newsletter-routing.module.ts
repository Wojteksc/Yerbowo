import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { NewsletterEmailSubscribeComponent } from './components/newsletter-email-subscribe/newsletter-email-subscribe.component';
import { NewsletterEmailUnsubscribeComponent } from './components/newsletter-email-unsubscribe/newsletter-email-unsubscribe.component';

const routes: Routes = [
  { path: 'subscribe', component: NewsletterEmailSubscribeComponent },
  { path: 'unsubscribe', component: NewsletterEmailUnsubscribeComponent },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class NewsletterRoutingModule {}