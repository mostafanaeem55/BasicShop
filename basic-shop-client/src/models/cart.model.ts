import { CartItem } from './cartItem.model';

export interface Cart {
  id: number;
  cartItems: CartItem[];
}
