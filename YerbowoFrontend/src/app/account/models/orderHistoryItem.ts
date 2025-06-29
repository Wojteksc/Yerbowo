import { ProductImage } from '../../shared/models/productImage';

export interface OrderHistoryItem {
  id: number;
  productImages: ProductImage[];
  date: string;
  total: number;
  status: string;
}
