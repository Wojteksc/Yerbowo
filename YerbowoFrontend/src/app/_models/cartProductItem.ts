import { ProductState } from '../_enums/productState.enum';

export interface CartProductItem {
    id: number;
    code: string;
    name: string;
    description: string;
    price: number;
    oldPrice: number;
    stock: number;
    state: ProductState;
    image: string;
    categorySlug: string;
    subcategorySlug: string;
    slug: string;
 }
