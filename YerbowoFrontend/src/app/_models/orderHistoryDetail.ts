import { AddressCard } from './addressCard';
import { OrderHistoryDetailItem } from './orderHistoryDetailItem';


export interface OrderHistoryDetail {
  id: number;
  totalCost: number;
  address: AddressCard;
  orderItems: OrderHistoryDetailItem[];
}
