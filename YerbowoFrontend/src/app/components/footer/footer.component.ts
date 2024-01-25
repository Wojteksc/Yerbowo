import { Component, OnInit, ViewEncapsulation } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { AlertifyService } from 'src/app/_services/alertify.service';
import { NewsletterService } from 'src/app/_services/newsletter.service';

@Component({
  selector: 'app-footer',
  templateUrl: './footer.component.html',
  styleUrls: ['./footer.component.css'],
  encapsulation: ViewEncapsulation.None,
})
export class FooterComponent implements OnInit {

  newsletterForm: FormGroup;

  constructor(
    private newsletterService: NewsletterService,
    private fb: FormBuilder, 
    private alertify: AlertifyService) { }

  ngOnInit() {
    this.newsletterForm = this.fb.group({
      email: ['', [Validators.required, Validators.email]]
    });
  }

  get f() { return this.newsletterForm.controls; }

  invite() {
    if(this.newsletterForm.valid) {
      const email = this.newsletterForm.value;

      this.newsletterService.invite(email).subscribe(next => {
        this.alertify.success('Na podany adres e-mail wysłano zaproszenie do newslettera. Prosimy o potwierdzenie.');
      }, error => {
        this.alertify.error(error);
      });
    }
  }
}