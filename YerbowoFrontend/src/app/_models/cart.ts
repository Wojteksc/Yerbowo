import { CartItem } from "./cartItem";

export interface Cart {
    items: CartItem[];
    sum: number;
    totalItems: number;
}
