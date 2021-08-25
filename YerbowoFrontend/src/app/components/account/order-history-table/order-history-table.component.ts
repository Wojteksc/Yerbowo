import { Component, OnInit, ViewEncapsulation } from '@angular/core';
import { OrderHistoryItem } from 'src/app/_models/orderHistoryItem';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-order-history-table',
  templateUrl: './order-history-table.component.html',
  styleUrls: ['./order-history-table.component.css'],
  encapsulation: ViewEncapsulation.None,
})
export class OrderHistoryTableComponent implements OnInit {
  orders: OrderHistoryItem[];

  constructor(private activatedRoute: ActivatedRoute) { }

  ngOnInit() {
    this.activatedRoute.data.subscribe(data => {
      this.orders = data['orderHistory'];
    });
  }
}
