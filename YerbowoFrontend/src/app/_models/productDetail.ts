import { ProductState } from '../_enums/productState.enum';

export interface ProductDetail {
   id: number;
   code: string;
   name: string;
   description: string;
   price: number;
   oldPrice: number;
   stock: number;
   state: ProductState;
   image: string;
   category: string;
   subcategory: string;
}
