import { Component, OnInit, ViewEncapsulation } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { AddressCreate } from 'src/app/_models/addressCreate';
import { AddressService } from 'src/app/_services/address.service';
import { AuthService } from 'src/app/_services/auth.service';
import { AlertifyService } from 'src/app/_services/alertify.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-address-add',
  templateUrl: './address-add.component.html',
  styleUrls: ['../../../styles/form-center.css', './address-add.component.css'],
  encapsulation: ViewEncapsulation.None,
})
export class AddressAddComponent implements OnInit {
  address: AddressCreate;
  addressAddForm: FormGroup;

  submitted = false;

  constructor(private fb: FormBuilder, private addressService: AddressService,
    private authService: AuthService, private alertify: AlertifyService,
    private router: Router) { }

  ngOnInit() {
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
      email: ['', Validators.required],
      nip: [''],
      company: [''],
    });
  }

  get f() { return this.addressAddForm.controls; }

  createAddress() {
    this.submitted = true;
    if(this.addressAddForm.valid) {
      this.address = Object.assign({}, this.addressAddForm.value);

      this.addressService.createAddress(this.authService.decodedToken.sub, this.address).subscribe(() => {
        this.alertify.success('Pomyślnie utworzono adres.');
      }, error => {
        this.alertify.error(error);
      }, () => {
        this.router.navigate(['/moje-konto/adresy']);
      });
    }
  }
}
