import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { AlertifyService } from 'src/app/_services/alertify.service';
import { AuthService } from '../../_services/auth.service';

@Component({
  selector: 'app-email-verification',
  templateUrl: './../home/home.component.html',
  styleUrls: ['./../home/home.component.css']
})
export class EmailVerificationComponent implements OnInit {
  email: string;
  token: string;

  constructor(private authService: AuthService,
    private alertify: AlertifyService,
    private activatedRoute: ActivatedRoute,
    private router: Router) { }

  ngOnInit() {

    this.activatedRoute.queryParams.subscribe(params => {
      this.email = params['email'];
      this.token = params['token'];
    });

    this.authService.verifyEmail({email: this.email, token: this.token}).subscribe(response => {
      this.alertify.success("Adres e-mail został potwierdzony.", 0);
    }, error => {
      this.alertify.error(error);
    }, () => {
      this.router.navigate(['']);
    })
  }
}