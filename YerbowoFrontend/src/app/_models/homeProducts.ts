import { ProductCard } from './productCard';

export interface HomeProducts {
  bestsellers: ProductCard[];
  news: ProductCard[];
  recommended: ProductCard[];
  promotions: ProductCard[];
}
