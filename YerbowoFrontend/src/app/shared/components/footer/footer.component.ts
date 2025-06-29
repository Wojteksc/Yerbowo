import { Component, OnInit, ViewEncapsulation } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { AlertifyService } from 'src/app/core/services/alertify.service';
import { NewsletterService } from 'src/app/newsletter/services/newsletter.service';

@Component({
  selector: 'app-footer',
  templateUrl: './footer.component.html',
  styleUrls: ['./footer.component.css'],
  encapsulation: ViewEncapsulation.None,
})
export class FooterComponent implements OnInit {
  newsletterForm!: FormGroup;

  constructor(
    private newsletterService: NewsletterService,
    private fb: FormBuilder, 
    private alertify: AlertifyService
  ) {}

  ngOnInit(): void {
    this.newsletterForm = this.fb.group({
      email: ['', [Validators.required, Validators.email]],
    });
  }

  get f() {
    return this.newsletterForm.controls;
  }

  invite(): void {
    if (this.newsletterForm.invalid) {
      return;
    }
    const email = this.newsletterForm.value.email;
    this.newsletterService.invite(email).subscribe({
      next: (response) => this.alertify.success(response),
      error: (err) => this.alertify.error(err),
    });
  }
}
