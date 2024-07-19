import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { CartItem } from '../../models/cartItem.model';
import { Product } from '../../models/product.model';
import { CartServiceService } from '../cart-service.service';
import { AdminServiceService } from '../admin-service.service';

@Component({
  selector: 'app-cart',
  standalone: true,
  imports: [CommonModule,FormsModule],
  templateUrl: './cart.component.html',
  styleUrl: './cart.component.css'
})
export class CartComponent {
  products: Product[] = [];
  cartItems: CartItem[] = [];
  searchQuery: string = '';
  cartId: number = 0;
  totalAmount: number = 0;

  constructor( private cartService: CartServiceService) { }

  ngOnInit(): void {
    this.cartId = this.getCartIdFromLocalStorage();
    if (!this.cartId) {
      this.createCart();
    } else {
      this.loadCartItems();
    }
    this.loadProducts();
  }

  createCart(): void {
    this.cartService.createCart().subscribe(cart => {
      this.cartId = cart.id;
      this.saveCartIdToLocalStorage(this.cartId);
      this.loadCartItems();
    });
  }

  createNewCart(): void {
    this.createCart();

  }

  getCartIdFromLocalStorage(): number {
    const cartId = localStorage.getItem('cartId');
    return cartId ? +cartId : 0;
  }

  saveCartIdToLocalStorage(cartId: number): void {    
    localStorage.setItem('cartId', cartId.toString());
    this.loadCartItems();
  }

  loadProducts(): void {
    this.cartService.getVisibleProducts().subscribe(products => {
      this.products = products;
    });
  }

  loadCartItems(): void {    
    this.cartService.getCartItems(this.cartId).subscribe(cartItems => {            
      this.cartItems = cartItems;
      this.calculateTotalAmount();
    });
  }

  addToCart(product: Product): void {
    const cartItem: CartItem = {
      id: -1 ,
      productId: product.id,
      product,
      quantity: 1,
      cartId: this.cartId
    };

    this.cartService.addToCart(cartItem).subscribe(
      () => {
        this.loadCartItems();
      },
      (error) => {
        if (error.status === 400) {
          alert(error.error);
        } else if (error.status === 404) {
          alert(error.error);
        } else {
          alert('An unexpected error occurred.');
        }
      }
    );
  }

  updateCartItem(cartItem: CartItem): void {
    this.cartService.updateCartItem(cartItem).subscribe(() => {
      this.loadCartItems();
    });
  }

  removeCartItem(id: number): void {
    this.cartService.removeCartItem(id).subscribe(() => {
      this.loadCartItems();
    });
  }

  loadVisibleProducts(): void {
    if (this.searchQuery.trim() === '') {
      this.loadProducts();
      return;
    }
    this.cartService.searchVisibleProducts(this.searchQuery).subscribe(products => {
      this.products = products;
    });
  }

  searchProducts(): void {
    this.loadVisibleProducts();
  }
  
  calculateTotalAmount(): void {
    this.totalAmount = this.cartItems?.reduce((total, item) => total + (item.product.price * item.quantity), 0);
  }
}
