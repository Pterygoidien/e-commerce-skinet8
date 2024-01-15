import { Component, OnInit } from '@angular/core';
import { IProduct } from '../shared/models/product';
import { ShopService } from './shop.service';
import { IBrand } from '../shared/models/brand';
import { IType } from '../shared/models/type';
import { IPagination } from '../shared/models/pagination';

@Component({
  selector: 'app-shop',
  templateUrl: './shop.component.html',
  styleUrls: ['./shop.component.scss'],
})
export class ShopComponent implements OnInit {
  products: IProduct[] = [];
  brands: IBrand[] = [];
  types: IType[] = [];

  constructor(private shopService: ShopService) {}

  ngOnInit(): void {
    this.getProduts();
    this.getBrands();
    this.getTypes();
  }

  getProduts(): void {
    this.shopService.getProducts().subscribe({
      next: (response: IPagination<IProduct[]>) => {
        this.products = response.data;
      },
      error: (error) => {
        console.log(error);
      },
      complete: () => {
        console.log('request completed');
        console.log('extra statement');
      },
    });
  }

  getBrands(): void {
    this.shopService.getBrands().subscribe({
      next: (response: IBrand[]) => {
        this.brands = response;
      },
      error: (error) => {
        console.log(error);
      },
    });
  }
  getTypes(): void {
    this.shopService.getTypes().subscribe({
      next: (response: any) => {
        this.types = response;
      },
      error: (error) => {
        console.log(error);
      },
    });
  }
}
