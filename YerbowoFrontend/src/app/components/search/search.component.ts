import { Component, OnInit, ViewEncapsulation } from '@angular/core';
import { DataService } from 'src/app/_services/data.service';

@Component({
  selector: 'app-search',
  templateUrl: './search.component.html',
  styleUrls: ['./search.component.css'],
  encapsulation: ViewEncapsulation.None,
})
export class SearchComponent implements OnInit {
  totalCartProducts: any
  constructor(private dataService: DataService) { }

  ngOnInit() {
    this.dataService.currentTotalMessage.subscribe(totalCartProducts => this.totalCartProducts = totalCartProducts)
  }

}
