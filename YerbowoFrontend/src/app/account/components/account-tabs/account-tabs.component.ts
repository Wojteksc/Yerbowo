import { Component, OnInit, ViewEncapsulation } from '@angular/core';
import { AuthService } from 'src/app/auth/services/auth.service';

@Component({
  selector: 'app-account-tabs',
  templateUrl: './account-tabs.component.html',
  styleUrls: ['./account-tabs.component.css'],
  encapsulation: ViewEncapsulation.None,
})
export class AccountTabsComponent implements OnInit {

  userName: string | null = null;

  constructor(public authService: AuthService) { }

  ngOnInit(): void {
    const uniqueName = this.authService.decodedToken?.unique_name;
    this.userName = uniqueName ? uniqueName.split('@')[0] : null;
  }
}