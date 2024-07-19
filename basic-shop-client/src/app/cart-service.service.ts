import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { CartItem } from '../models/cartItem.model';
import { Product } from '../models/product.model';
import { Cart } from '../models/cart.model';

@Injectable({
  providedIn: 'root'
})
export class CartServiceService {
  private apiUrl = 'https://localhost:7144/api/Cart';

  constructor(private http: HttpClient) { }
  createCart(): Observable<Cart> {
    return this.http.post<Cart>(this.apiUrl, {});
  }

  getCartById(cartId: number): Observable<Cart> {
    return this.http.get<Cart>(`${this.apiUrl}/${cartId}`);
  }

  getCartItems(cartId: number): Observable<CartItem[]> {
    return this.http.get<CartItem[]>(`${this.apiUrl}/GetCartItems/${cartId}`);
  }
  getVisibleProducts(): Observable<Product[]> {
    return this.http.get<Product[]>(`${this.apiUrl}/VisibleProducts`);
  }
  addToCart(cartItem: CartItem): Observable<void> {
    return this.http.post<void>(`${this.apiUrl}/AddToCart`, cartItem);
  }

  updateCartItem(cartItem: CartItem): Observable<void> {
    return this.http.put<void>(`${this.apiUrl}/EditCartItem/${cartItem.id}`, cartItem);
  }

  removeCartItem(id: number): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/RemoveFromCart/${id}`);
  }
  searchVisibleProducts(searchQuery: string): Observable<Product[]> {
    return this.http.get<Product[]>(`${this.apiUrl}/SearchVisibleProducts?searchQuery=${searchQuery}`);
  }
  
}
