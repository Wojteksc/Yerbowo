import { ProductState } from '../_enums/productState.enum';

export interface ProductCard {
   id: number;
   name: string;
   categorySlug: string;
   subcategorySlug: string;
   price: number;
   oldPrice: number;
   state: ProductState;
   image: string;
   slug: string;
   createdAt: Date;
}
