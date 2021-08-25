import { Component, OnInit, ViewEncapsulation } from '@angular/core';
import { AuthService } from 'src/app/_services/auth.service';

@Component({
  selector: 'app-account-tabs',
  templateUrl: './account-tabs.component.html',
  styleUrls: ['./account-tabs.component.css'],
  encapsulation: ViewEncapsulation.None,
})
export class AccountTabsComponent implements OnInit {

  constructor(public authService: AuthService) { }

  ngOnInit() {
  }

}
