import { Component, OnInit, ViewEncapsulation, Input } from '@angular/core';

@Component({
  selector: 'app-account-option-card',
  templateUrl: './account-option-card.component.html',
  styleUrls: ['./account-option-card.component.css'],
  encapsulation: ViewEncapsulation.None
})
export class AccountOptionCardComponent implements OnInit {
  @Input() option;
  constructor() { }

  ngOnInit() {
  }

}
