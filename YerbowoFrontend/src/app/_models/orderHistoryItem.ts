import { ProductImage } from './productImage';

export interface OrderHistoryItem {
  id: number;
  productImages: ProductImage[];
  date: string;
  total: number;
  status: string;
}
