import { Component, OnInit, OnDestroy, ViewEncapsulation } from '@angular/core';
import { DataService } from 'src/app/core/services/data.service';
import { Subscription } from 'rxjs';

@Component({
  selector: 'app-search',
  templateUrl: './search.component.html',
  styleUrls: ['./search.component.css'],
  encapsulation: ViewEncapsulation.None,
})
export class SearchComponent implements OnInit, OnDestroy {
  totalCartProducts: any;
  private subscription?: Subscription;

  constructor(private dataService: DataService) { }

  ngOnInit() {
    this.subscription = this.dataService.cartTotal$.subscribe(total => {
      this.totalCartProducts = total;
    });
  }

  ngOnDestroy() {
    this.subscription?.unsubscribe();
  }
}
