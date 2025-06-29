import { ProductCard } from "src/app/products/models/productCard";

export interface HomeProducts {
  bestsellers: ProductCard[];
  news: ProductCard[];
  recommended: ProductCard[];
  promotions: ProductCard[];
}
