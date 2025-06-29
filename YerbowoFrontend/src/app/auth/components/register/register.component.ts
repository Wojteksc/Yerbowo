import { Component, OnInit, ViewEncapsulation } from '@angular/core';
import { FormGroup, FormBuilder, Validators, ValidatorFn, AbstractControl } from '@angular/forms';
import { Router } from '@angular/router';

import { User } from 'src/app/shared/models/user';
import { AuthService } from 'src/app/auth/services/auth.service';
import { AlertifyService } from 'src/app/core/services/alertify.service';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['../../../shared/styles/form-center.css', './register.component.css'],
  encapsulation: ViewEncapsulation.None,
})
export class RegisterComponent implements OnInit {
  registerForm: FormGroup;
  submitted = false;

  constructor(
    private fb: FormBuilder,
    private authService: AuthService,
    private alertify: AlertifyService,
    private router: Router
  ) {}

  ngOnInit(): void {
    this.initForm();
  }

  private initForm(): void {
    this.registerForm = this.fb.group(
      {
        firstName: ['', Validators.required],
        lastName: ['', Validators.required],
        companyName: [''],
        email: ['', [Validators.required, Validators.email]],
        confirmEmail: ['', [Validators.required, Validators.email]],
        password: ['', Validators.required],
        confirmPassword: ['', Validators.required],
      },
      {
        validators: [this.matchValidator('email', 'confirmEmail', 'mismatchEmail'),
                     this.matchValidator('password', 'confirmPassword', 'mismatchPassword')],
      }
    );
  }

  private matchValidator(controlName: string, matchingControlName: string, errorKey: string): ValidatorFn {
    return (group: AbstractControl): { [key: string]: boolean } | null => {
      const control = group.get(controlName);
      const matchingControl = group.get(matchingControlName);

      if (control && matchingControl && control.value !== matchingControl.value) {
        return { [errorKey]: true };
      }
      return null;
    };
  }

  get f() {
    return this.registerForm.controls;
  }

  register(): void {
    this.submitted = true;

    if (this.registerForm.invalid) return;

    const user: User = { ...this.registerForm.value };

    this.authService.register(user).subscribe({
      next: () => {
        this.alertify.success(
          'Konto zostało założone. Aby zalogować się do sklepu, sprawdź pocztę i kliknij na link potwierdzający poprawność adresu e-mail.',
          0
        );
        this.router.navigate(['']);
      },
      error: (err) => this.alertify.error(err),
    });
  }
}
