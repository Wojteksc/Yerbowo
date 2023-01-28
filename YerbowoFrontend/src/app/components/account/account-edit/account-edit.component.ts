import { Component, OnInit, ViewEncapsulation, ViewChild, HostListener } from '@angular/core';
import { AuthService } from 'src/app/_services/auth.service';
import { ActivatedRoute } from '@angular/router';
import { User } from 'src/app/_models/user';
import { UserService } from 'src/app/_services/user.service';
import { AlertifyService } from 'src/app/_services/alertify.service';
import { UntypedFormGroup, UntypedFormBuilder, Validators } from '@angular/forms';

@Component({
  selector: 'app-account-edit',
  templateUrl: './account-edit.component.html',
  styleUrls: ['../../../styles/form-center.css', './account-edit.component.css'],
  encapsulation: ViewEncapsulation.None,
})
export class AccountEditComponent implements OnInit {
  user: User;
  accountForm: UntypedFormGroup;

  submitted = false;

  constructor(private route: ActivatedRoute, private userService: UserService,
    private authService: AuthService, private alertify: AlertifyService,
    private fb: UntypedFormBuilder) { }

  ngOnInit() {
    this.route.data.subscribe(data => {
      this.user = data['user'];
    });

    this.accountForm = this.fb.group({
      id: [this.user.id],
      firstName: [this.user.firstName, Validators.required],
      lastName: [this.user.lastName, Validators.required],
      companyName: [this.user.companyName],
      email: [this.user.email, [Validators.required, Validators.email]],
      confirmEmail: [this.user.email, [Validators.required, Validators.email]],
      newPassword: [''],
      confirmPassword: [''],
      currentPassword: ['', Validators.required]
    }, { validators: [this.emailMatchValidator, this.passwordMatchValidator] });
  }

  emailMatchValidator(g: UntypedFormGroup) {
    return g.get('email').value === g.get('confirmEmail').value ? null : { 'mismatchEmail': true };
  }

  passwordMatchValidator(g: UntypedFormGroup) {
    return g.get('newPassword').value === g.get('confirmPassword').value ? null : { 'mismatchPassword': true };
  }

  get f() { return this.accountForm.controls; }

  updateUser() {
    this.submitted = true;
    if(this.accountForm.valid) {
      this.user = Object.assign({}, this.accountForm.value);

      this.userService.updateUser(this.authService.decodedToken.sub, this.user).subscribe(next => {
        this.alertify.success('Pomyślnie zapisano zmiany');
      }, error => {
        this.alertify.error(error);
      });
    }
  }
}