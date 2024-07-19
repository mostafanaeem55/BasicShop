import { Product } from './product.model';
import { Cart } from './cart.model';

export interface CartItem {
  id: number;
  cartId: number;
  productId: number;
  quantity: number;
  product: Product;
}