import { Component, OnInit, ViewEncapsulation } from '@angular/core';

@Component({
  selector: 'app-account-option-list',
  templateUrl: './account-option-list.component.html',
  styleUrls: ['./account-option-list.component.css'],
  encapsulation: ViewEncapsulation.None,
})
export class AccountOptionListComponent implements OnInit {
  
  options = [
    { title: 'Informacja', icon: 'fa-user-circle', link: '/moje-konto/dane'},
    { title: 'Adresy', icon: 'fa-address-book', link: '/moje-konto/adresy'},
    { title: 'Historia zamówień', icon: 'fa-history', link: '/moje-konto/zamowienia'},
    { title: 'Obserwowane', icon: 'fa-star', link: '/moje-konto/obserwowane'},
    { title: 'Kupony', icon: 'fa-gift', link: '/moje-konto/kupony'},
    ]

  constructor() { }

  ngOnInit() {
  }

}
