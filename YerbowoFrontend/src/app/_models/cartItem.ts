import { CartProductItem } from "./cartProductItem";

export interface CartItem {
    product: CartProductItem;
    quantity: number;
}
