import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { MatSlideToggleModule } from '@angular/material/slide-toggle';
import { AdminServiceService } from '../admin-service.service';
import { Product } from '../../models/product.model';
@Component({
  selector: 'app-admin',
  standalone: true,
  imports: [CommonModule, FormsModule, MatSlideToggleModule],
  templateUrl: './admin.component.html',
  styleUrls: ['./admin.component.css']
})
export class AdminComponent implements OnInit {
  products: Product[] = [];
  searchQuery: string = '';
  newProduct: Partial<Product> = {};

  constructor(private adminService: AdminServiceService) { }

  ngOnInit(): void {
    this.loadProducts();
  }

  loadProducts(): void {
    this.adminService.getProducts().subscribe(products => {
      this.products = products;
    });
  }

  onSearch(): void {
    if (this.searchQuery.trim() === '') {
      this.loadProducts(); // Reload all products if search query is empty
    } else {
      this.adminService.searchProducts(this.searchQuery).subscribe(products => {
        this.products = products;
      });
    }
  }

  toggleVisibility(product: Product): void {
    product.visible = !product.visible;
    this.adminService.editProduct(product).subscribe(); // Update product visibility on the server
  }

  addNewProduct(name: string, price: number, quantity: number): void {
    const newProduct: Product = {
      id: 0, // Backend will assign the id
      name,
      price,
      quantity,
      visible: true
    };
    this.adminService.addProduct(newProduct).subscribe(product => {
      this.products.push(product);
      this.newProduct = {}; // Clear the form
    });
  }

  deleteProduct(id: number): void {
    this.adminService.deleteProduct(id).subscribe(() => {
      this.products = this.products.filter(product => product.id !== id);
    });
  }

  editProduct(product: Product): void {
    this.adminService.editProduct(product).subscribe();
  }
}
