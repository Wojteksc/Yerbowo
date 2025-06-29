import { Component, OnInit, ViewEncapsulation } from '@angular/core';
import { UntypedFormGroup, UntypedFormBuilder, Validators } from '@angular/forms';
import { AddressCreate } from 'src/app/account/models/addressCreate';
import { AddressService } from 'src/app/account/services/address.service';
import { AuthService } from 'src/app/auth/services/auth.service';
import { AlertifyService } from 'src/app/core/services/alertify.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-address-add',
  templateUrl: './address-add.component.html',
  styleUrls: ['../../../shared/styles/form-center.css', './address-add.component.css'],
  encapsulation: ViewEncapsulation.None,
})
export class AddressAddComponent implements OnInit {
  addressAddForm: UntypedFormGroup;
  submitted = false;

  constructor(
    private fb: UntypedFormBuilder,
    private addressService: AddressService,
    private authService: AuthService,
    private alertify: AlertifyService,
    private router: Router
  ) {}

  ngOnInit(): void {
    this.initForm();
  }

  private initForm(): void {
    this.addressAddForm = this.fb.group({
      userId: [this.authService.decodedToken.sub],
      alias: ['', Validators.required],
      firstName: ['', Validators.required],
      lastName: ['', Validators.required],
      street: ['', Validators.required],
      buildingNumber: ['', Validators.required],
      apartmentNumber: [''],
      place: ['', Validators.required],
      postCode: ['', Validators.required],
      phone: ['', Validators.required],
      email: ['', [Validators.required, Validators.email]],
      nip: [''],
      company: [''],
    });
  }

  get f() {
    return this.addressAddForm.controls;
  }

  createAddress(): void {
    this.submitted = true;

    if (this.addressAddForm.invalid) {
      return;
    }

    const address: AddressCreate = { ...this.addressAddForm.value };

    this.addressService.createAddress(this.authService.decodedToken.sub, address).subscribe({
      next: () => this.alertify.success('Pomyślnie utworzono adres.'),
      error: (err) => this.alertify.error(err),
      complete: () => this.router.navigate(['/moje-konto/adresy']),
    });
  }
}
