import { Component, OnInit, ViewEncapsulation } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { OrderHistoryDetail } from 'src/app/account/models/orderHistoryDetail';

@Component({
  selector: 'app-order-history-detail',
  templateUrl: './order-history-detail.component.html',
  styleUrls: ['./order-history-detail.component.css'],
  encapsulation: ViewEncapsulation.None,
})
export class OrderHistoryDetailComponent implements OnInit {
  order: OrderHistoryDetail | null = null;

  constructor(private activatedRoute: ActivatedRoute) {}

  ngOnInit(): void {
    this.activatedRoute.data.subscribe(data => {
      this.order = data['orderDetail'] ?? null;
    });
  }
}