import { Component, OnInit, ViewEncapsulation } from '@angular/core';
import { User } from 'src/app/_models/user';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { AuthService } from 'src/app/_services/auth.service';
import { AlertifyService } from 'src/app/_services/alertify.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['../../styles/form-center.css','./register.component.css'],
  encapsulation: ViewEncapsulation.None,
})
export class RegisterComponent implements OnInit {
  user: User;
  registerForm: FormGroup;

  constructor(private authService: AuthService, private alertify: AlertifyService,
     private fb: FormBuilder, private router: Router) {  }

  ngOnInit() {

    this.registerForm = this.fb.group({
      firstName: ['', Validators.required],
      lastName: ['', Validators.required],
      companyName: [''],
      email: ['', [Validators.required, Validators.email]],
      confirmEmail: ['', [Validators.required, Validators.email]],
      password: ['', Validators.required],
      confirmPassword: ['', Validators.required]
    }, { validators: [this.emailMatchValidator, this.passwordMatchValidator] });
  }

  emailMatchValidator(g: FormGroup) {
    return g.get('email').value === g.get('confirmEmail').value ? null : { 'mismatchEmail': true };
  }

  passwordMatchValidator(g: FormGroup) {
    return g.get('password').value === g.get('confirmPassword').value ? null : { 'mismatchPassword': true };
  }

  get f() { return this.registerForm.controls; }

  register() {
    if(this.registerForm.valid) {
      this.user = Object.assign({}, this.registerForm.value);

      this.authService.register(this.user).subscribe(response => {
        this.alertify.success("Konto zostało założone. Aby zalogować się do sklepu, sprawdź pocztę i kliknij na link potwierdzający poprawność adresu e-mail.", 0);
      }, error => {
        this.alertify.error(error);
      }, () => {
        this.router.navigate(['']);
      });
    }
  }

}
